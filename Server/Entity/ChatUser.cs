using CNetwork;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity.EntityProperty;
using ChatServer.Entity.Notification;
using ChatServer.IO.Entity;
using ChatServer.IO.Notification;
using ChatServer.Network;
using ChatServer.Network.Packets;
using ChatServer.Utils;

namespace ChatServer.Entity
{
    public class ChatUser : AbstractEntity
    {
        [BsonIgnore]
        public HashSet<ChatSession> sessions { get; set; } = new HashSet<ChatSession>();

        [BsonIgnore]
        ChatUserStore store = new ChatUserStore();

        [BsonElement("FirstName")]
        public String FirstName { get; set; }

        [BsonElement("LastName")]
        public String LastName { get; set; }

        [BsonIgnore] public string FullName => FirstName + " " + LastName;

        [BsonElement("Town")]
        public String Town { get; set; } = "Default";

        [BsonElement("Email")]
        public String Email { get; set; }

        [BsonElement("Password")]
        public String Password { get; set; }

        [BsonElement("DoB")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("Gender")]
        public Gender Gender { get; set; }

        //LastLogon is the time that last session logged in
        [BsonElement("LastLogon")]
        public DateTime LastLogon { get; set; } = DateTime.UtcNow;

        //LastLogoff is the time that last session logged out
        [BsonElement("LastLogoff")]
        public DateTime LastLogoff { get; set; } = DateTime.UtcNow;

        //Key is id of user who have a reationship with this user, Value is id of relationship
        [BsonElement("Relationship"), BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<Guid, Guid> Relationship { get; private set; } = new Dictionary<Guid, Guid>();

        [BsonElement("Conversations")]
        public List<Guid> ConversationID { get; set; } = new List<Guid>();
        
        [BsonElement("NearestStickers")]
        public HashStack<int> NearestStickers { get; private set; } = new HashStack<int>();

        [BsonElement("BoughtStickerPacks")]
        public HashStack<int> BoughtStickerPacks { get; private set; } = new HashStack<int>();        

        //True if this user has been banned
        [BsonElement("Banned")]
        public bool Banned { get; set; } = false;

        [BsonElement("Notifications")]
        public HashStack<Guid> Notifications { get; set; } = new HashStack<Guid>();

        private ChatTheme _chatTheme = new ChatTheme();
        [BsonElement("ChatThemeSettings")]
        public ChatTheme ChatTheme {
            get => _chatTheme;
            set => _chatTheme = value;
        }
        //Key is conversation id, value is the last time it have action
        [BsonIgnore]
        public ConcurrentDictionary<Guid, long> Conversations { get; private set; } = new ConcurrentDictionary<Guid, long>();

        //Key is conversation id, value is the last time it have action
        [BsonIgnore]
        public ConcurrentDictionary<Guid, long> LastBuzz { get; private set; } = new ConcurrentDictionary<Guid, long>();

        public ChatUser()
        {

        }

        public ChatUser(ChatUserProfile profile)
        {
            this.ID = profile.ID;
            this.Email = profile.Email;
            this.Password = profile.PassHashed;
            this.FirstName = profile.FirstName;
            this.LastName = profile.LastName;
            this.DateOfBirth = profile.DoB;
            this.Gender = profile.Gender;
            this.Banned = profile.Banned;
            this.LastLogoff = profile.LastLogoff;
        }

        public bool IsOnline()
        {
            return sessions.Count > 0;
        }

        public void Send(IPacket packet, params ChatSession[] ignoreSessions)
        {
            foreach(ChatSession session in sessions)
            {
                if (!ignoreSessions.Contains<ChatSession>(session))
                {
                    session.Send(packet);
                }
            }
        }

        public void SendOnly(IPacket packet, ChatSession session)
        {
            session.Send(packet);
        }

        public void Online()
        {
            UserOnline packet = new UserOnline();
            packet.TargetID = this.ID;

            foreach (var pair in Relationship.Where(q => Relation.Get(q.Value).RelationType == RelationType.Friend))
            {
                if (ChatUserManager.IsOnline(pair.Key))
                {
                    ChatUserManager.LoadUser(pair.Key).Send(packet);
                }
            }
        }

        public void Offline()
        {
            this.LastLogoff = DateTime.UtcNow;
            UserOffline packet = new UserOffline();
            packet.TargetID = this.ID;

            foreach (var pair in Relationship.Where(q => Relation.Get(q.Value).RelationType == RelationType.Friend))
            {
                if (ChatUserManager.IsOnline(pair.Key))
                {
                    ChatUserManager.LoadUser(pair.Key).Send(packet);
                }
            }
            
            this.Save();
            SimpleChatServer.GetServer().Logger.Info(String.Format("User {0} has logged out at {1}!", this.Email, this.LastLogoff.ToString()));
        }

        public void AddNotification(AbstractNotification notification)
        {
            if (notification == null)
                return;
            Notifications.Add(notification.Id);
            new NotificationStore().Save(notification);
            Save();
            if (ChatUserManager.IsOnline(ID))
            {
                GetNotificationsResponse send = new GetNotificationsResponse();
                send.Notifications.Add(notification);
                Send(send);
            }
        }

        public void Kick()
        {
            foreach(ChatSession session in sessions)
            {
                session.Disconnect();
            }
        }

        public Relation SetRelation(ChatUser target, RelationType relationType, bool isSource = true)
        {
            if (target == null)
            {
                throw new ArgumentNullException("Target must not be null!");
            }

            Relation relation;
            Guid relationID;

            if (this.Relationship.ContainsKey(target.ID))
            {
                this.Relationship.TryGetValue(target.ID, out relationID);
                relation = Relation.Get(relationID);
            } else
            {
                relation = new Relation(this.ID, target.ID, 1);
                this.Relationship.Add(target.ID, relation.ID);
                target.Relationship.Add(this.ID, relation.ID);
                this.Save();
                target.Save();
            }

            relation.RelationType = relationType;
            relation.Save();

            return relation;
        }

        public override bool Save()
        {
            bool result = store.Save(this);
            ChatUserProfile profile = ProfileCache.Instance.GetUserProfile(this.ID);

            //if (profile == null) return result;

            profile.Email = this.Email;
            profile.PassHashed = this.Password;
            profile.FirstName = this.FirstName;
            profile.LastName = this.LastName;
            profile.DoB = this.DateOfBirth;
            profile.Gender = this.Gender;
            profile.Banned = this.Banned;
            profile.LastLogoff = this.LastLogoff;

            return result;
        }

        public bool SaveChatTheme()
        {
            return store.UpdateRelations(this);
        }

    }
}
