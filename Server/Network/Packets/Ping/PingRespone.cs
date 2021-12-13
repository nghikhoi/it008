using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class PingRespone : IPacket
    {
        public bool ForceUpdate { get; set; }
        public bool Runnable { get; set; }
        public string CurrentVersion { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            byteBuf.WriteBoolean(Runnable);
            byteBuf.WriteBoolean(ForceUpdate);
            ByteBufUtils.WriteUTF8(byteBuf, CurrentVersion);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
