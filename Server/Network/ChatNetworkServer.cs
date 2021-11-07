using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Network.Protocol;
using ChatServer.Utils.ThreadUtils;

namespace ChatServer.Network
{
    public abstract class ChatNetworkServer
    {
        protected SimpleChatServer Server { get; }
        protected ProtocolProvider protocolProvider { get; }

        protected CountdownLatch latch;         

        public ChatNetworkServer(SimpleChatServer server, ProtocolProvider protocolProvider, CountdownLatch latch)
        {
            this.Server = server;
            this.protocolProvider = protocolProvider;
            this.latch = latch;
        }

        public abstract Task Bind(IPEndPoint address);

        public virtual void OnBindSuccess(IPEndPoint address)
        {
            latch.Signal();
        }

        public abstract void OnBindFailure(IPEndPoint address, Exception e);

        public abstract void Shutdown();
    }
}
