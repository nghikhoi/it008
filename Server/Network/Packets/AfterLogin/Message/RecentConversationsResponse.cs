using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class RecentConversationsResponse : IPacket
    {
        public Dictionary<Guid, long> Conversations { get; private set; } = new Dictionary<Guid, long>();

        public void Decode(IByteBuffer buffer)
        {
            // throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            foreach(var conversation in Conversations)
            {
                ByteBufUtils.WriteUTF8(byteBuf, conversation.Key.ToString());
                byteBuf.WriteLong(conversation.Value);
            }

            ByteBufUtils.WriteUTF8(byteBuf, "~");
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            // throw new NotImplementedException();
        }
    }
}
