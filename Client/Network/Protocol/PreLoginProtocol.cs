using UI.Network.Packets;
using UI.Network.Packets.Login;
using UI.Network.Packets.Ping;
using UI.Network.Packets.Register;

namespace UI.Network.Protocol
{
    public class PreLoginProtocol : ChatProtocol
    {
        public PreLoginProtocol() : base("PreLogin")
        {
            Inbound(0x0, new PingRespone());
            Outbound(0x0, new Ping4Send());

            Inbound(0x02, new LoginResult());
            Outbound(0x02, new LoginData());

            Inbound(0x03, new RegisterResult());
            Outbound(0x03, new RegisterData());

            Inbound(0x04, new ReconnectResponse());
            Outbound(0x04, new ReconnectResquest());
        }
    }
}
