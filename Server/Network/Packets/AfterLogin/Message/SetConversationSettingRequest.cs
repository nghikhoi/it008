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

    public enum ConversationDataType {

        UPDATE_NICKNAMES, UPDATE_AVATAR

    }

    public class SetConversationSettingRequest : AbstractRequestPacket
    {
        //Index: Key
        //Value: Data Decoder
        private static readonly Dictionary<ConversationDataType, Func<IByteBuffer, Action<ChatSession, AbstractConversation>>> DecoderMap 
            = new Dictionary<ConversationDataType, Func<IByteBuffer, Action<ChatSession, AbstractConversation>>>()
            {
                { ConversationDataType.UPDATE_AVATAR, (buffer) => {
                    string fileId = ByteBufUtils.ReadUTF8(buffer);
                    return (session, conversation) =>
                    {
                        if (conversation is GroupConversation group)
                            group.ConversationAvatar = fileId;
                        AnnouncementMessage msg = new AnnouncementMessage()
                        {
                            Type = AnnouncementType.CHANGE_AVATAR,
                            Value = fileId
                        };
                        conversation.SendMessage(msg, session, false);
                    };
                } },
                { ConversationDataType.UPDATE_NICKNAMES, (buffer) =>
                {
                    Dictionary<Guid, string> Nicknames = new Dictionary<Guid, string>();
                    int count = buffer.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        Nicknames[Guid.Parse(ByteBufUtils.ReadUTF8(buffer))] = ByteBufUtils.ReadUTF8(buffer);
                    }
                    return (session, conversation) =>
                    {
                        foreach (var pair in Nicknames)
                        {
                            conversation.Nicknames[pair.Key] = pair.Value;
                            AnnouncementMessage msg = new AnnouncementMessage()
                            {
                                Type = AnnouncementType.CHANGE_NICKNAME,
                                Value = pair.Key + "=" + pair.Value
                            };
                            conversation.SendMessage(msg, session, false);
                        }
                    };
                } }
            };

        public Guid ConversationID { get; set; }
        public Queue<Action<ChatSession, AbstractConversation>> HandleList = new Queue<Action<ChatSession, AbstractConversation>>();

        public override void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            BoolBitMap map = new BoolBitMap();
            map.Decode(buffer);
            List<int> indexMaps = new List<int>();
            map.CopyIndexTo(indexMaps);
            foreach (var index in indexMaps)
            {
                HandleList.Enqueue(DecoderMap[(ConversationDataType) index].Invoke(buffer));
            }
        }

        public override IPacket createResponde(ChatSession session)
        {
            ConversationStore store = new ConversationStore();
            AbstractConversation conversation = store.Load(ConversationID);
            while (HandleList.Count > 0)
            {
                HandleList.Dequeue().Invoke((ChatSession) session, conversation);
            }
            GetConversationSettingRequest request = new GetConversationSettingRequest();
            request.ConversationID = ConversationID;
            return request.createResponde(session);
        }
    }
}
