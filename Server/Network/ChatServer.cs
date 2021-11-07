using CNetwork;
using CNetwork.Sessions;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Network.Pipeline;
using ChatServer.Network.Protocol;
using ChatServer.Utils.ThreadUtils;

namespace ChatServer.Network
{
    public class ChatServer : ChatSocketServer, IConnectionManager
    {
        public ChatServer(SimpleChatServer server, ProtocolProvider protocolProvider, CountdownLatch latch) : base(server, protocolProvider, latch)
        {
            this.bootstrap.ChildHandler(new ChannelInitializer(this));
        }

        public override void OnBindSuccess(IPEndPoint address)
        {
            base.OnBindSuccess(address);
            Server.IP = address.Address;
            Server.Port = address.Port;
            Server.Logger.Info("Bind success. Server is listening on " + Server.IP + ":" + Server.Port);
        }

        public override void OnBindFailure(IPEndPoint address, Exception e)
        {
            Server.Logger.Error("Bind Failured", e);
        }

        public ISession NewSession(IChannel c)
        {
            ChatSession session = new ChatSession(Server, c, protocolProvider, this);
            Server.SessionRegistry.Add(session);
            return session;
        }

        public void SessionInactivated(ISession session)
        {
            ChatSession chatSession = session as ChatSession;

            Server.SessionRegistry.Remove(chatSession.SessionID);

            if (chatSession.Owner == null) return;

            chatSession.Owner.sessions.Remove(chatSession);

            Server.Logger.Info(String.Format("Session {0} of user {1} has disconnected.", chatSession.getAddress(), chatSession.Owner.Email));

            if (!chatSession.Owner.IsOnline())
            {
                ChatUserManager.MakeOffline(chatSession.Owner);
            }
        }
    }
}
