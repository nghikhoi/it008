using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity.Conversation;
using ChatServer.Entity.Message;
using ChatServer.IO.Entity;
using ChatServer.IO.Message;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    
    public class SetAvatarRequest : IPacket
    {

        public Guid ConversationID { get; set; }
        public string FileId { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            FileId = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ConversationStore store = new ConversationStore();
            AbstractConversation conversation = store.Load(ConversationID);
            if (conversation is GroupConversation group) {
                group.ConversationAvatar = FileId;

                AnnouncementMessage msg = new AnnouncementMessage() {
                    Type = AnnouncementType.CHANGE_AVATAR,
                    Value = FileId
                };
                conversation.SendMessage(msg, (ChatSession) session, false);
            }

            SetAvatarResponse response = new SetAvatarResponse() {
                FileId = this.FileId,
                ConversationID = this.ConversationID
            };

            conversation.SendIfOnline(response);
        }
    }
}
