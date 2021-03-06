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
using ChatServer.Network.RestAPI.Handler;

namespace ChatServer.Network.RestAPI.Controller
{
    public class AttachmentController : ApiController
    {
        public static String SavePath { get; } = "Uploaded/Attachment/{0}";

        [HttpPost, Route("api/message/attachment/{conversationID}")]
        public Dictionary<String, String> ConversationFileUpload(string conversationID)
        {
            ChatSession session = Verifier.SessionFromToken(Request);
            if (session == null)
                throw new UnauthorizedAccessException();

            Guid conversationGid = Guid.Parse(conversationID);
            if (!session.Owner.ConversationID.Contains(conversationGid))
                throw new UnauthorizedAccessException();

            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            Dictionary<String, String> result = FileHandler.Upload(Request, String.Format(SavePath, conversationID));

            return result;
        }

        [HttpGet, Route("api/message/attachment/{fileID}/{conversationID}")]
        public HttpResponseMessage GetConversationFile(string conversationID, string fileID)
        {
            ChatSession session = Verifier.SessionFromToken(Request);
            if (session == null)
                throw new UnauthorizedAccessException();

            Guid conversationGid = Guid.Parse(conversationID);
            if (!session.Owner.ConversationID.Contains(conversationGid))
                throw new UnauthorizedAccessException();

            if (!File.Exists(Path.Combine(String.Format(SavePath, conversationID), fileID)))
                throw new FileNotFoundException();

            String filePath = Path.Combine(String.Format(SavePath, conversationID), fileID);
            String fileName = AttachmentStore.Parse(Guid.Parse(fileID));

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            new CustomContentTypeProvider().TryGetContentType(fileName, out var contentType);

            if (contentType == null || contentType.Length <1)
            {
                contentType = "application/octet-stream";
            }

            if (Request.Headers.Range != null)
            {
                HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, contentType, ServerSettings.BUFFER_SIZE);
                partialResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
                return partialResponse;
            }
            else
            {
                HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
                fullResponse.Content = new StreamContent(stream, ServerSettings.BUFFER_SIZE);
                fullResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                fullResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
                return fullResponse;
            }
        }      
    }
}
