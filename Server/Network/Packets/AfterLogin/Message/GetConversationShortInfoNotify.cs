using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetConversationShortInfoNotify : IPacket
    {

        public Guid ConversationId { get; set; }

        public void Decode(IByteBuffer buffer) {
            ConversationId = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public IByteBuffer Encode(IByteBuffer byteBuf) {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationId.ToString());
            return byteBuf;
        }

        public void Handle(ISession session)
        {

        }
    }
}
