using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{

    public class SetAvatarResponse : IPacket {
        public Guid ConversationID { get; set; }
        public string FileId { get; set; }

        public void Decode(IByteBuffer buffer) {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            FileId = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf) {
            throw new NotImplementedException();
        }

        public void Handle(ISession session) {
            
        }
    }
}
