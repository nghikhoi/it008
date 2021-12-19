using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    
    public class SetNicknamesRequest : IPacket
    {
        public Guid ConversationID { get; set; }
        public Dictionary<Guid, string> Nicknames { get; set; } = new Dictionary<Guid, string>();

        public void Decode(IByteBuffer buffer) {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf) {
            ByteBufUtils.WriteUTF8(byteBuf, ConversationID.ToString());
            byteBuf.WriteInt(Nicknames.Count);
            foreach (var pair in Nicknames) {
                ByteBufUtils.WriteUTF8(byteBuf, pair.Key.ToString());
                ByteBufUtils.WriteUTF8(byteBuf, pair.Value);
            }
            return byteBuf;
        }

        public void Handle(ISession session) {
            throw new NotImplementedException();
        }
    }
}
