using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GetConversationShortInfoResponse : IPacket
    {
        public string ConversationID { get; set; }
        public string ConversationName { get; set; }
        public string ConversationAvatar { get; set; }
        public long LastActive { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID);
            ByteBufUtils.WriteUTF8(byteBuf, ConversationName);
            ByteBufUtils.WriteUTF8(byteBuf, ConversationAvatar);
            byteBuf.WriteLong(LastActive);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
