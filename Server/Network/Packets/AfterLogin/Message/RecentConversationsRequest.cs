using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class RecentConversationsRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
            // throw new NotImplementedException();
        }

        public override IPacket createResponde(ChatSession session) {
            ChatSession chatSession = session as ChatSession;

            RecentConversationsResponse packet = new RecentConversationsResponse();

            foreach (var conversation in chatSession.Owner.Conversations)
            {
                packet.Conversations.Add(conversation.Key, conversation.Value);
            }

            return packet;
        }
    }
}
