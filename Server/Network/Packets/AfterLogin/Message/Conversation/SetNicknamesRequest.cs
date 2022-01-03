using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity;
using ChatServer.IO.Entity;
using ChatServer.IO.Message;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    
    public class SetNicknamesRequest : IPacket
    {

        public Guid ConversationID { get; set; }
        public Dictionary<Guid, string> Nicknames = new Dictionary<Guid, string>();

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            int count = buffer.ReadInt();
            for (int i = 0; i < count; i++) {
                Nicknames[Guid.Parse(ByteBufUtils.ReadUTF8(buffer))] = ByteBufUtils.ReadUTF8(buffer);
            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ConversationStore store = new ConversationStore();
            AbstractConversation conversation = store.Load(ConversationID);
            foreach (var pair in Nicknames) {
                AnnouncementMessage msg = new AnnouncementMessage() {
                    Type = AnnouncementType.CHANGE_NICKNAME,
                    Value = conversation.Nicknames[((ChatSession) session).Owner.ID] + " đã đổi tên của " + conversation.Nicknames[pair.Key] + " thành " + pair.Value
                };
                conversation.Nicknames[pair.Key] = pair.Value;
                conversation.SendMessage(msg, (ChatSession) session, false);
            }
            SetNicknamesResponse response = new SetNicknamesResponse() {
                Nicknames = this.Nicknames,
                ConversationID = this.ConversationID
            };

            conversation.SendIfOnline(response);
        }
    }
}
