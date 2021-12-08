using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using ChatServer.MessageCore.Message;
using ChatServer.Network.Packets;
using ChatServer.Network.Packets.AfterLogin.DataPreparing;
using ChatServer.Network.Packets.AfterLogin.Message;
using ChatServer.Network.Packets.AfterLogin.Notification;
using ChatServer.Network.Packets.AfterLogin.Search;
using ChatServer.Network.Packets.AfterLogin.Setting;
using ChatServer.Network.Packets.AfterLogin.Sticker;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.RestAPI.Controller
{

    class PacketMap {

        public static readonly Dictionary<string, Type> Map = new Dictionary<string, Type>() {
                { "chattheme", typeof(ChatThemeGetRequest) },
                { "profile", typeof(DisplayedProfileRequest) },
                { "friends", typeof(FriendsListRequest) },
                { "selfid", typeof(GetSelfIDRequest) },
                { "selfprofile", typeof(GetSelfProfileRequest) },
                { "shortprofile", typeof(ShortProfileRequest) },
                { "conversation", typeof(ConversationFrIDRequest) },
                { "conversationinfo", typeof(GetConversationShortInfoRequest) },
                { "conversationmedia", typeof(MediaFromConversationRequest) },
                { "conversationmessages", typeof(MessageFromConversationRequest) },
                { "recentconversation", typeof(RecentConversationsRequest) },
                { "singleconversation", typeof(SingleConversationFrUserIDRequest) },
                { "notifications", typeof(GetNotificationsRequest) },
                { "search", typeof(UserSearchRequest) },
                { "stickerpacks", typeof(GetBoughtStickerPacksRequest) },
                { "recentsticker", typeof(GetNearestSickerRequest) },
                { "updateprofile", typeof(UpdateSelfProfileRequest) },
                { "modifypassword", typeof(ModifyPasswordRequest) }
        };

        public static Action<Action<IPacket>> getResponde(string id, ChatSession session, IByteBuffer buffer) {
            RequestPacket request = (RequestPacket) Activator.CreateInstance(Map[id]);
            request.Decode(buffer);
            return request.createRespondeAction(session);
        }

    }
    
    public class DataController : ApiController
    {

        [HttpGet, Route("api/data/{requestId}/{requestData?}")]
        public string DataRequest(string requestId, string requestData = "") {
            ChatSession session = Verifier.SessionFromToken(Request);
            if (session == null)
                throw new UnauthorizedAccessException();

            IByteBuffer buffer = PacketUtil.decode(requestData);
            string result = null;
            PacketMap.getResponde(requestId, session, buffer).Invoke(responde => {
                IByteBuffer respondeBuffer = ByteBufferUtil.DefaultAllocator.Buffer();
                responde.Encode(respondeBuffer);
                result = PacketUtil.encode(respondeBuffer);
            });

            return result;
        }

    }
}
