using System;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Transport.Channels;
using UI.Network.Protocol;

namespace UI.Network
{
    public class ClientSession : BasicSession
    {
        ProtocolProvider protocolProvider;

        public String SessionID { get; private set; } = "~";

        public static string HeaderToken { get; private set; } = "ChatVerifier";

        public ClientSession(IChannel channel, ProtocolProvider protocolProvider) : base(channel, protocolProvider.Test)
        {
            this.protocolProvider = protocolProvider;
        }

        public override void Send(IPacket packet)
        {
            base.Send(packet);
        }

        public void LoggedIn(String token)
        {
            SessionID = token;
            Protocol = protocolProvider.MainProtocol;
            Console.WriteLine("Session Token Update: " + token);
        }
    }
}
