using System;
using System.Collections.Generic;
using ChatServer.Entity.EntityProperty;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class FriendsListRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {

        }

        public override IPacket createResponde(ChatSession session) {
            ChatSession chatSession = session as ChatSession;

            FriendsListResponse response = new FriendsListResponse();

            Relation rela;
            foreach (KeyValuePair<Guid, Guid> pair in chatSession.Owner.Relationship)
            {
                if ((rela = Relation.Get(pair.Value)) != null && rela.RelationType == RelationType.Friend)
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
