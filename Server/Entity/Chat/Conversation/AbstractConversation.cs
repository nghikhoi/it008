using System;
using System.Collections.Generic;
using ChatServer.Cache.Core;
using ChatServer.IO.Entity;
using ChatServer.IO.Message;
using ChatServer.Network;
using ChatServer.Network.Packets;
using CNetwork;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ChatServer.Entity
{
    /// <summary>
    /// Pattern:
    ///     - Template method: Deploy between Conversation Classes
    ///     - Mediator: Deploy between user class and conversation class (ex: send, receive)
    ///     - Observer: Deploy between user class and conversation class (ex: join, leave, play music...)
    ///     - Bridge: Deploy between message class and conversation class
    /// </summary>
    [BsonKnownTypes(typeof(SingleConversation), typeof(GroupConversation))]
    public abstract class AbstractConversation : IConversation
    {
        ConversationStore store = new ConversationStore();
        MessageStore messageStore = new MessageStore();

        [BsonId]
        public Guid ID { get; set; }

        [BsonElement("LastActive")]
        public long LastActive { get; set; }

        [BsonElement("Color")]
        public int Color { get; set; } = -16744320;

        [BsonElement("Members")]
        public HashSet<Guid> Members { get; set; } = new HashSet<Guid>();

        [BsonElement("MessagesID")]
        public List<Guid> MessagesID { get; set; } = new List<Guid>();

        [BsonElement("MediaID")]
        public List<Guid> MediaID { get; set; } = new List<Guid>();

        [BsonElement("AttachmentID")]
        public List<Guid> AttachmentID { get; set; } = new List<Guid>();
        [BsonElement(nameof(Nicknames)), BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<Guid, string> Nicknames { get; set; } = new Dictionary<Guid, string>();

        [BsonIgnore]
        public LRUCache<Guid, IMessage> LoadedMessages { get; set; } = new LRUCache<Guid, IMessage>(100, 10);

        public void SendIfOnline(IPacket packet)
        {
            foreach (var member in Members)
            {
                if (ChatUserManager.IsOnline(member))
                {
                    ChatUserManager.OnlineUsers[member].Send(packet);
                }
            }
        }

        public void SendMessage(AbstractMessage message, ChatSession chatSession, bool IgnoreSelf = true)
        {
            ChatUser user;
            Guid messageID = Guid.NewGuid();
            (message as AbstractMessage).ID = messageID;
            (message as AbstractMessage).Author = chatSession.Owner.ID;

            //Store
            if (message is VideoMessage || message is ImageMessage)
                MediaID.Add((message as AbstractMessage).ID);
            if (message is AttachmentMessage)
                AttachmentID.Add((message as AbstractMessage).ID);
            if (message is StickerMessage)
            {
                chatSession.Owner.NearestStickers.Add((message as StickerMessage).StickerID);
                chatSession.Owner.Save();
            }

            MessagesID.Add((message as AbstractMessage).ID);
            LoadedMessages.AddReplace((message as AbstractMessage).ID, message);

            store.UpdateMessagesList(this);
            messageStore.Save((message as AbstractMessage), this.ID);

            foreach (Guid userID in Members)
            {
                this.LastActive = DateTime.Now.Ticks;

                if (ChatUserManager.OnlineUsers.TryGetValue(userID, out user))
                {
                    //if (userID.CompareTo(chatSession.Owner.ID) == 0)
                    //    continue;

                    SendMessageResponse packet = new SendMessageResponse();
                    packet.ConversationID = this.ID.ToString();
                    packet.Message = message;
                    packet.SenderID = chatSession.Owner.ID.ToString();
                    if (!IgnoreSelf)
                        user.Send(packet);
                    else
                        user.Send(packet, chatSession);

                    user.Conversations.TryRemove(ID, out long lact);
                    user.Conversations.TryAdd(ID, LastActive);
                }
            }
        }

        public void UpdateLastActive(ChatSession chatSession)
        {
            LastActive = long.MaxValue;
            foreach (var member in Members)
            {
                if (member.Equals(chatSession.Owner.ID)) continue;

                if (ChatUserManager.OnlineUsers.ContainsKey(member))
                {
                    LastActive = 0;
                    break;
                }

                ChatUser user = new ChatUserStore().Load(member);
                LastActive = Math.Min(LastActive, (long) Math.Ceiling((DateTime.UtcNow - user.LastLogoff).TotalMinutes));
            }
        }

        public void UpdateColor(int color, ChatUser sender)
        {
            //Check change condition
            if (true)
            {
                this.Color = color;
                store.Save(this);

                ChangeBubbleChatColor packet = new ChangeBubbleChatColor()
                {
                    ConversationID = this.ID.ToString(),
                    Color = color
                };

                foreach (Guid memberID in Members)
                {
                    if (!ChatUserManager.IsOnline(memberID)) continue;
                    ChatUser user = ChatUserManager.LoadUser(memberID);
                    user.Send(packet);
                }
            } else
            {
                ChangeBubbleChatColor packet = new ChangeBubbleChatColor()
                {
                    ConversationID = this.ID.ToString(),
                    Color = this.Color
                };
                sender.Send(packet);
            }
        }
    }
}
