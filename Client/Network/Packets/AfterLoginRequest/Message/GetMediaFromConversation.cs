using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GetMediaFromConversation : IPacket
    {
        public string ConversationID { get; set; }
        public int MediaPosition { get; set; }
        public int Quantity { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID);
            byteBuf.WriteInt(MediaPosition);
            byteBuf.WriteInt(Quantity);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
