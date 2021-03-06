using CNetwork;
using DotNetty.Codecs.Protobuf;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace UI.Network.Pipiline
{
    class ChannelInitializer : BasicChannelInitializer
    {
        /**
         * The time in seconds which are elapsed before a client is disconnected due to a read timeout.
         */
        private static int READ_IDLE_TIMEOUT = 20;

        /**
         * The time in seconds which are elapsed before a client is deemed idle due to a write timeout.
         */
        private static int WRITE_IDLE_TIMEOUT = 15;

        private ChatConnection connectionManager;

        public ChannelInitializer(ChatConnection connectionManager) : base(connectionManager)
        {
            this.connectionManager = connectionManager;
        }

        protected override void InitChannel(ISocketChannel channel)
        {
            IChannelPipeline pipeline = channel.Pipeline;

            //pipeline.AddLast(new LoggingHandler());
            pipeline.AddLast("framing-enc", new ProtobufVarint32LengthFieldPrepender());
            pipeline.AddLast("framing-dec", new ProtobufVarint32FrameDecoder());

            channel.Pipeline
                .AddLast("idle_timeout", new IdleStateHandler(READ_IDLE_TIMEOUT, WRITE_IDLE_TIMEOUT, 0));
            base.InitChannel(channel);
        }
    }
}
