using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{

    public class SetNicknamesResponse : IPacket {

        public Guid ConversationID { get; set; }
        public Dictionary<Guid, string> Nicknames = new Dictionary<Guid, string>();

        public void Decode(IByteBuffer buffer) {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            int count = buffer.ReadInt();
            for (int i = 0; i < count; i++) {
                Nicknames[Guid.Parse(ByteBufUtils.ReadUTF8(buffer))] = ByteBufUtils.ReadUTF8(buffer);
            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf) {
            throw new NotImplementedException();
        }

        public void Handle(ISession session) {
            
        }

    }
}
