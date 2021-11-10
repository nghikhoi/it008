using System;

namespace UI.Network.RestAPI
{
    public static class StreamAPI
    {
        private static readonly String MediaStreamUrl = "http://{0}:{1}/api/message/media/{2}/{3}/{4}";
        private static readonly String MediaThumbnailUrl = "http://{0}:{1}/api/message/media/thumbnail/{2}/{3}/{4}";

        public static String GetMediaURL(String fileID, String conversationID)
        {
            String address = ChatConnection.Instance.WebHost;
            int port = ChatConnection.Instance.WebPort;
            String token = ChatConnection.Instance.Session.SessionID;

            return String.Format(MediaStreamUrl, address, port, fileID, conversationID, token);
        }

        public static String GetMediaThumbnailURL(String fileID, String conversationID)
        {
            String address = ChatConnection.Instance.WebHost;
            int port = ChatConnection.Instance.WebPort;
            String token = ChatConnection.Instance.Session.SessionID;

            return String.Format(MediaThumbnailUrl, address, port, fileID, conversationID, token);
        }
    }
}
