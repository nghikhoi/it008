using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity;
using ChatServer.IO.Message;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class GroupConversationAddedResponse : IPacket
    {
        public Guid GroupId { get; set; }

        public void Decode(IByteBuffer buffer) {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf) {
            ByteBufUtils.WriteUTF8(byteBuf, GroupId.ToString());
            return byteBuf;
        }

        public void Handle(ISession session) {
            throw new NotImplementedException();
        }
    }
}
