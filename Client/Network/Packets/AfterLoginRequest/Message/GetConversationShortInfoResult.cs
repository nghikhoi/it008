using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GetConversationShortInfoResult : IPacket
    {
        public string ConversationID { get; set; }
        public string ConversationName { get; set; }
        public string ConversationAvatar { get; set; }
        public long LastActive { get; set; }
        public Dictionary<Guid, string> Nicknames { get; set; } = new Dictionary<Guid, string>();
        public HashSet<string> Members { get; set; } = new HashSet<string>();
        public string OnlineUser { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = ByteBufUtils.ReadUTF8(buffer);
            ConversationName = ByteBufUtils.ReadUTF8(buffer);
            ConversationAvatar = ByteBufUtils.ReadUTF8(buffer);
            LastActive = buffer.ReadLong();
            OnlineUser = ByteBufUtils.ReadUTF8(buffer);
            int Count = buffer.ReadInt();
            for (int i = 0; i < Count; i++)
            {
                Guid id = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
                string value = ByteBufUtils.ReadUTF8(buffer);
                Nicknames.Add(id, value);
            }
            // Get the number of members in this conversation
            string temp = ByteBufUtils.ReadUTF8(buffer);
            while (temp != "~") {
                Members.Add(temp);
                temp = ByteBufUtils.ReadUTF8(buffer);
            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
        }
    }
}
