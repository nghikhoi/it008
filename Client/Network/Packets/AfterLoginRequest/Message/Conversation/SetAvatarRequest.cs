using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    
    public class SetAvatarRequest : IPacket
    {
        public Guid ConversationID { get; set; }
        public string FileId { get; set; }

        public void Decode(IByteBuffer buffer) {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf) {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID.ToString());
            ByteBufUtils.WriteUTF8(byteBuf, FileId);
            return byteBuf;
        }

        public void Handle(ISession session) {
            throw new NotImplementedException();
        }
    }
}
