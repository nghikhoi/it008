using ChatServer.Network.RestAPI.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ChatServer.IO.Message;

namespace ChatServer.Network.RestAPI.Controller
{
    public class ProfileController : ApiController
    {
        public static String AvatarPath { get; } = "Uploaded/Profile/Avatar";

        [HttpPost, Route("api/profile/avatar")]
        public void AvatarUpload()
        {
            ChatSession session = Verifier.SessionFromToken(Request);
            if (session == null)
                throw new UnauthorizedAccessException();

            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            String fileName;
            Guid id = session.Owner.ID;

            Directory.CreateDirectory(AvatarPath);
            var streamProvider = new MultipartFormDataStreamProvider(AvatarPath);

            Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(p =>
            {
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        continue;
                    }

                    fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    AttachmentStore.Map(id, fileName);

                    if (File.Exists(Path.Combine(AvatarPath, id.ToString())))
                    {
                        File.Delete(Path.Combine(AvatarPath, id.ToString()));
                    }
                    File.Move(fileData.LocalFileName, Path.Combine(AvatarPath, id.ToString()));
                }
            }).Wait();
        }

        [HttpGet, Route("api/profile/avatar/{userID}")]
        public HttpResponseMessage GetAvatar(string userID)
        {
            ChatSession session = Verifier.SessionFromToken(Request);
            if (session == null)
                throw new UnauthorizedAccessException();

            String filePath = Path.Combine(AvatarPath, userID);
            String fileName;

            if (!File.Exists(Path.Combine(AvatarPath, userID)))
            {
                if (session.Owner.Gender == Entity.EntityProperty.Gender.Female)
                {
                    filePath = "Resource/Default/Avatar_Female.jpg";
                } else
                {
                    filePath = "Resource/Default/Avatar_Male.jpg";
                }
                fileName = "Avatar.jpg";
            } else
            {
                fileName = AttachmentStore.Parse(Guid.Parse(userID));
            }

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            new CustomContentTypeProvider().TryGetContentType(fileName, out var contentType);

            if (contentType == null || contentType.Length < 1)
            {
                contentType = "application/octet-stream";
            }

            if (Request.Headers.Range != null)
            {
                HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, contentType, ServerSettings.BUFFER_SIZE);
                return partialResponse;
            }
            else
            {
                HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
                fullResponse.Content = new StreamContent(stream, ServerSettings.BUFFER_SIZE);
                fullResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return fullResponse;
            }
        }

        [HttpGet, Route("api/profile/avatar")]
        public HttpResponseMessage GetAvatar()
        {
            ChatSession session = Verifier.SessionFromToken(Request);
            if (session == null)
                throw new UnauthorizedAccessException();
            return GetAvatar(session.Owner.ID.ToString());
        }
    }
}
