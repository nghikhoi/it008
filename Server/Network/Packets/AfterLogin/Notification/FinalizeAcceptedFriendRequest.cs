using System;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class FinalizeAcceptedFriendRequest : IPacket
    {
        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
