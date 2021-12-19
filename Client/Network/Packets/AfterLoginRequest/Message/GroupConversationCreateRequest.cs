using System;
using System.Collections.Generic;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Utils;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GroupConversationCreateRequest : RequestPacket {
        public List<Guid> Members { get; set; } = new List<Guid>();
        public string GroupName;
        public string ConversationId;
        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            byteBuf.WriteInt(Members.Count);
            Members.ForEach(id => ByteBufUtils.WriteUTF8(byteBuf, id.ToString()));
            if (!FastCodeUtils.NotEmptyStrings(GroupName))
                GroupName = "~";
            ByteBufUtils.WriteUTF8(byteBuf, GroupName);
            if (!FastCodeUtils.NotEmptyStrings(ConversationId))
                ConversationId = "~";
            ByteBufUtils.WriteUTF8(byteBuf, ConversationId);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
