using CNetwork;
using CNetwork.Protocols;
using CNetwork.Sessions;
using DotNetty.Transport.Channels;
using ChatServer.Network.Packets.Login;
using ChatServer.Network.Packets.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;
using ChatServer.Entity.Meta.Profile;
using ChatServer.Network.Protocol;

namespace ChatServer.Network
{
    public class ChatSession : BasicSession
    {
        public SimpleChatServer Server { get; }

        ProtocolProvider protocolProvider; 

        IConnectionManager connectionManager;

        public ChatUser Owner { get; set; }

        public Guid SessionID { get; set; } = Guid.NewGuid();

        public ChatSession(SimpleChatServer server, IChannel channel, ProtocolProvider protocolProvider, IConnectionManager connectionManager) 
            : base(channel, protocolProvider.HandShake)
        {
            Server = server;
            this.protocolProvider = protocolProvider;
            this.connectionManager = connectionManager;
        }

        public void FinalizeLogin(ChatUserProfile profile)
        {
            try
            {
                Owner = ChatUserManager.LoadUser(profile.ID);
            } catch
            {
                this.Disconnect();
            }

            ChatUserManager.MakeOnline(Owner);

            Owner.LastLogon = DateTime.UtcNow;
            Owner.sessions.Add(this);

            Protocol = protocolProvider.MainProtocol;

            //Notify
            Server.Logger.Info(String.Format("User {0} has logged in at {1}", Owner.Email, getAddress()));
        }

        public override void Disconnect()
        {
            base.Disconnect();
        }

        private void UpdatePipeline(String key, IChannelHandler channelHandler)
        {
            Channel.Pipeline.Replace(key, key, channelHandler);
        }
    }
}
