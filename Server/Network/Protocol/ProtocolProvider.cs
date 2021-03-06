using CNetwork.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Network.Protocol
{
    public class ProtocolProvider
    {
        public HandShakeProtocol HandShake { get; }
        public AfterLoginProtocol MainProtocol { get; }
        public ProtocolProvider()
        {
            this.HandShake = new HandShakeProtocol();
            this.MainProtocol = new AfterLoginProtocol();
        }
    }
}
