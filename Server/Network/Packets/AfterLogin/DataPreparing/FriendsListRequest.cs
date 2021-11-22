using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using ChatServer.IO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity.EntityProperty;

namespace ChatServer.Network.Packets.AfterLogin.DataPreparing
{
    public class FriendsListRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {

        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;

            FriendsListResponse response = new FriendsListResponse();

            Relation rela;
            foreach (KeyValuePair<Guid, Guid> pair in chatSession.Owner.Relationship)
            {
                if ((rela = Relation.Get(pair.Value)) != null && rela.RelationType == Relation.Type.Friend)
                {
                    bool isOnline = ChatUserManager.OnlineUsers.ContainsKey(pair.Key);
                    if (isOnline)
                        response.Friends.Insert(0, pair.Key.ToString());
                    else
                        response.Friends.Add(pair.Key.ToString());
                }
            }

            return response;
        }
    }
}
