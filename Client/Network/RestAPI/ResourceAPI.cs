using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using UI.Utils;

namespace UI.Network.RestAPI
{
    public static class ResourceAPI
    {
        private static readonly String currentVersion = ConfigurationManager.AppSettings["AppVersion"];

        public static readonly String ResourceDownloadUrl = "http://{0}:{1}/api/resource/v{2}/{3}";

        public delegate void ErrorHandler(Exception error);

        public static void DownloadResource(String savePath,
            DownloadProgressChangedEventHandler onProgressChange, AsyncCompletedEventHandler onDownloadComplete, 
            ErrorHandler errorHandler)
        {
            try
            {
                String fileNameParam = HashUtils.Base64Encode(savePath.Replace(TempUtil.ResourcePath, ""));

                String address = ChatConnection.Instance.WebHost;
                int port = ChatConnection.Instance.WebPort;
                Uri uri = new Uri(String.Format(ResourceDownloadUrl, address, port, currentVersion, fileNameParam));

                WebClient webClient = RestUtils.CreateWebClient();

                Directory.CreateDirectory(savePath.Replace(Path.GetFileName(savePath), "")); //Remove file name, create save path
                Directory.CreateDirectory(TempUtil.DownloadPath); //Create temp folder
                String temp = Path.Combine(TempUtil.DownloadPath, Guid.NewGuid().ToString()); //Create new temp file

                webClient.DownloadFileAsync(uri, temp);
                webClient.DownloadFileCompleted += (o, e) =>
                {
                    File.Move(temp, savePath);
                    if (onDownloadComplete != null)
                        onDownloadComplete(o, e);
                };

                //webClient.DownloadFileAsync(uri, savePath);
                if (onProgressChange != null)
                    webClient.DownloadProgressChanged += onProgressChange;

                webClient.DownloadFileCompleted += (o, e) =>
                {
                    RestUtils.Remove(webClient);
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (errorHandler != null) errorHandler(e);
            }
        }
    }
}
