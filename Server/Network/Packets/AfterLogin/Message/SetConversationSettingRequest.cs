using System;
using System.Collections.Generic;
using System.Linq;
using ChatServer.Entity.Conversation;
using ChatServer.IO.Entity;
using ChatServer.IO.Message;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class SetConversationSettingRequest : AbstractRequestPacket
    {
        //Index: Key
        //Value: Data Decoder
        private static readonly List<Func<IByteBuffer, Action<AbstractConversation>>> DecoderMap = new List<Func<IByteBuffer, Action<AbstractConversation>>>();

        public Guid ConversationID { get; set; }
        public Queue<Action<AbstractConversation>> HandleList = new Queue<Action<AbstractConversation>>();

        public override void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            BoolBitMap map = new BoolBitMap();
            map.Decode(buffer);
            List<int> indexMaps = new List<int>();
            map.CopyIndexTo(indexMaps);
            foreach (var index in indexMaps)
            {
                HandleList.Enqueue(DecoderMap[index].Invoke(buffer));
            }
        }

        public override IPacket createResponde(ISession session)
        {
            ConversationStore store = new ConversationStore();
            AbstractConversation conversation = store.Load(ConversationID);
            while (HandleList.Count > 0)
            {
                HandleList.Dequeue().Invoke(conversation);
            }
            GetConversationSettingRequest request = new GetConversationSettingRequest();
            request.ConversationID = ConversationID;
            return request.createResponde(session);
        }
    }
}
