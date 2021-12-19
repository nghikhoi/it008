using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GroupConversationAddedResponse : IPacket
    {
        public Guid GroupId { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            GroupId = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public IByteBuffer Encode(IByteBuffer byteBuf) {
            throw new NotImplementedException();
        }

        public void Handle(ISession session) {
           
        }
    }
}
