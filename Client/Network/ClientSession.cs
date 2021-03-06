using System;
using System.Threading.Tasks;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Transport.Channels;
using UI.Annotations;
using UI.Network.Packets;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.Packets.AfterLoginRequest.Sticker;
using UI.Network.Packets.Login;
using UI.Network.Packets.Register;
using UI.Network.Protocol;

namespace UI.Network
{
    public class ClientSession : BasicSession
    {
        ProtocolProvider protocolProvider;
        [CanBeNull] private readonly PacketRespondeListener _listener;
        private readonly RespondeManager _respondeManager;

        public String SessionID { get; private set; } = "~";

        public static string HeaderToken { get; private set; } = "ChatVerifier";

        public ClientSession(IChannel channel, ProtocolProvider protocolProvider, RespondeManager respondeManager, PacketRespondeListener listener = null) : base(channel, protocolProvider.Test)
        {
            this.protocolProvider = protocolProvider;
            _respondeManager = respondeManager;
            _listener = listener;
            _listener.LoginRespondeEvent += (session, responde) => LoggedIn(responde.Token);
            _listener.ReconnectRespondeEvent += (session, responde) => LoggedIn(responde.Token);
        }

        public override void Send(IPacket packet)
        {
            base.Send(packet);
        }

        public async Task<TExpectResponde> Send<TExpectResponde>(IPacket packet) where TExpectResponde : IPacket {
            Task<IPacket> respondeTask = _respondeManager.CreateRespondeWaiter<TExpectResponde>(packet);
            Send(packet);
            IPacket responde = await respondeTask;
            return (TExpectResponde) responde;
        }

        public override void MessageReceived(IPacket message) {
            if (!_respondeManager.RespondToResponse(message))
                base.MessageReceived(message);
            if (message is LoginResult) {
                _listener?.OnLoginResponde(this, message as LoginResult);
            } else if (message is RegisterResult) {
                _listener?.OnRegisterResponde(this, message as RegisterResult);
            } else if (message is ReconnectResponse) {
                _listener?.OnReconnectResponde(this, message as ReconnectResponse);
            } else if (message is ReceiveMessage) {
                _listener?.OnReceiveMessage(this, message as ReceiveMessage);
            } else if (message is GetNotificationsResult) {
                _listener?.OnReceiveNotification(this, message as GetNotificationsResult);
            } else if (message is FinalizeAcceptedFriendRequestReceive) {
                _listener?.OnFinalizeAcceptedFriend(this);
            } else if (message is BuyStickerCategoryResponse) {
                _listener?.OnBuyStickerResponse(this, message as BuyStickerCategoryResponse);
            } else if (message is UserOnlineReceive userOnlineReceive) {
                _listener?.OnUserOnlineResponse(this, userOnlineReceive);
            } else if (message is UserOfflineReceive userOfflineReceive) {
                _listener?.OnUserOfflineResponse(this, userOfflineReceive);
            } else if (message is BubbleChatColorSetRequest bubbleChatColor) {
                _listener?.OnBubbleChatColorResponse(this, bubbleChatColor);
            } else if (message is GroupConversationAddedResponse groupConversationAdded) {
                _listener?.OnGroupAddResponseResponse(this, groupConversationAdded);
            } else if (message is SetNicknamesResponse nicknamesResponse) {
                _listener?.OnSetNicknamesResponse(this, nicknamesResponse);
            } else if (message is SetAvatarResponse avatarResponse) {
                _listener?.OnSetAvatarResponse(this, avatarResponse);
            } else if (message is GetConversationShortInfoNotify shortInfoNotify) {
                _listener?.OnConversationShortInfoNotify(this, shortInfoNotify);
            }
        }

        public void LoggedIn(String token)
        {
            SessionID = token;
            Protocol = protocolProvider.MainProtocol;
            Console.WriteLine("Session Token Update: " + token);
        }
    }
}
