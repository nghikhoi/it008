using System;
using CNetwork.Sessions;
using UI.Network.Packets;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.Packets.AfterLoginRequest.Sticker;
using UI.Network.Packets.Login;
using UI.Network.Packets.Register;

namespace UI.Network {

	public delegate void LoginRespondeHandler(ISession session, LoginResult responde);
	public delegate void ReconnectRespondeHandler(ISession session, ReconnectResponse responde);
	public delegate void RegisterRespondeHandler(ISession session, RegisterResult responde);
	public delegate void ReceiveMessageHandler(ISession session, ReceiveMessage responde);
    public delegate void ReceiveNotificationHandler(ISession session, GetNotificationsResult responde);
    public delegate void FinalizeAcceptedFriendHandler(ISession session);
    public delegate void UserOnlineHandler(ISession session, UserOnlineReceive response);
    public delegate void UserOfflineHandler(ISession session, UserOfflineReceive response);
	public delegate void BuyStickerResponseHandler(ISession session, BuyStickerCategoryResponse responde);
    public delegate void BubbleChatColorResponseHandler(ISession session, BubbleChatColorSetRequest responde);
    public delegate void GroupAddResponseHandler(ISession session, GroupConversationAddedResponse response);
	public class PacketRespondeListener {

		public event LoginRespondeHandler LoginRespondeEvent;
		internal void OnLoginResponde(ISession session, LoginResult responde) {
			LoginRespondeEvent?.Invoke(session, responde);
		}
		public event ReconnectRespondeHandler ReconnectRespondeEvent;
		internal void OnReconnectResponde(ISession session, ReconnectResponse responde) {
			ReconnectRespondeEvent?.Invoke(session, responde);
		}
		public event RegisterRespondeHandler RegisterRespondeEvent;
		internal void OnRegisterResponde(ISession session, RegisterResult responde) {
			RegisterRespondeEvent?.Invoke(session, responde);
		}
		public event ReceiveMessageHandler ReceiveMessageEvent;
		internal void OnReceiveMessage(ISession session, ReceiveMessage responde) {
			ReceiveMessageEvent?.Invoke(session, responde);
		}
        public event ReceiveNotificationHandler ReceiveNotificationEvent;
        internal void OnReceiveNotification(ISession session, GetNotificationsResult responde) {
            ReceiveNotificationEvent?.Invoke(session, responde);
        }
        public event FinalizeAcceptedFriendHandler FinalizeAcceptedFriendEvent;
        internal void OnFinalizeAcceptedFriend(ISession session) {
	        FinalizeAcceptedFriendEvent?.Invoke(session);
        }
        public event BuyStickerResponseHandler BuyStickerResponseEvent;
        internal void OnBuyStickerResponse(ISession session, BuyStickerCategoryResponse responde) {
            BuyStickerResponseEvent?.Invoke(session, responde);
        }
        public event UserOnlineHandler UserOnlineEvent;
        internal void OnUserOnlineResponse(ISession session, UserOnlineReceive responde) {
            UserOnlineEvent?.Invoke(session, responde);
        }
        public event UserOfflineHandler UserOfflineEvent;
        internal void OnUserOfflineResponse(ISession session, UserOfflineReceive responde) {
            UserOfflineEvent?.Invoke(session, responde);
        }
        public event BubbleChatColorResponseHandler BubbleChatColorEvent;
        internal void OnBubbleChatColorResponse(ISession session, BubbleChatColorSetRequest responde) {
            BubbleChatColorEvent?.Invoke(session, responde);
        }
        public event GroupAddResponseHandler GroupAddResponseEvent;
        internal void OnGroupAddResponseResponse(ISession session, GroupConversationAddedResponse responde) {
            GroupAddResponseEvent?.Invoke(session, responde);
        }

    }
}