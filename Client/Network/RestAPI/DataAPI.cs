using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ChatServer.Utils;
using CNetwork;
using DotNetty.Buffers;
using UI.Network.Packets;
using UI.Network.Packets.AfterLoginRequest;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.Packets.AfterLoginRequest.Profile;
using UI.Network.Packets.AfterLoginRequest.Search;
using UI.Network.Packets.AfterLoginRequest.Sticker;

namespace UI.Network.RestAPI {

	public class RequestMap {
		
		public static readonly Dictionary<Type, string> Map = new Dictionary<Type, string>() {
			{ typeof(ChatThemeGetRequest), "chattheme" },
			{ typeof(GetDisplayedProfile), "profile" },
			{ typeof(GetFriendIDs), "friends" },
			{ typeof(GetSelfID), "selfid" },
			{ typeof(GetSelfProfile), "selfprofile" },
			{ typeof(GetShortInfo), "shortprofile" },
			{ typeof(ConversationFromID), "conversation" },
			{ typeof(GetConversationShortInfo), "conversationinfo" },
			{ typeof(GetMediaFromConversation), "conversationmedia" },
			{ typeof(GetMessageFromConversation), "conversationmessages" },
			{ typeof(RecentConversations), "recentconversation" },
			{ typeof(SingleConversationFrUserID), "singleconversation" },
			{ typeof(GetNotifications), "notifications" },
			{ typeof(SearchUser), "search" },
			{ typeof(GetBoughtStickerPacksRequest), "stickerpacks" },
			{ typeof(GetNearestSickerRequest), "recentsticker" },
			{ typeof(UpdateSelfProfile), "updateprofile" }
		};

		public static string getId(IPacket packet) {
			return Map[packet.GetType()];
		}
		
	}
	
	public class DataAPI {
		
		private static readonly String DataURL = "http://{0}:{1}/api/data/{2}/{3}";

		public static void getData<R, T>(Action<T> resultHandler = null) 
			where T : IPacket 
			where R : RequestPacket {
			getData(Activator.CreateInstance<R>(), resultHandler);
		}
		
		public static void getData<T>(IPacket request, Action<T> resultHandler = null) where T : IPacket {
			new Task(() =>
			{

				using (WebClient client = new WebClient())
				{
					String address = ChatConnection.Instance.WebHost;
					int port = ChatConnection.Instance.WebPort;
					IByteBuffer buffer = ByteBufferUtil.DefaultAllocator.Buffer();
					request.Encode(buffer);
					string requestData = PacketUtil.encode(buffer);
					string id = RequestMap.getId(request);
					string path = String.Format(DataURL, address, port, id, requestData);
					if (App.IS_LOCAL_DEBUG) {
						Console.WriteLine("[DataAPI] Try to get data from path: " + path);
						return;
					}
					
					client.Headers.Add(ClientSession.HeaderToken, ChatConnection.Instance.Session.SessionID);
					string hashed = client.DownloadString(path);

					buffer = PacketUtil.decode(hashed);
					T responde = Activator.CreateInstance<T>();
					responde.Decode(buffer);

					if (resultHandler != null)
					{
						resultHandler.Invoke(responde);
					}
					else {
						responde.Handle(ChatConnection.Instance.Session);
					}
				}
			}).Start();
		}
		
	}
	
}