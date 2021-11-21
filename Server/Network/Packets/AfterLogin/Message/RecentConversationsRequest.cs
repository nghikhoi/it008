using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Ocsp;

namespace ChatServer.Network.Packets.AfterLogin.Message
{
    public class RecentConversationsRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {
            // throw new NotImplementedException();
        }

        public override IPacket createResponde(ISession session) {
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
