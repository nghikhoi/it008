﻿using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.IO.Entity;
using ChatServer.IO.Message;
using ChatServer.MessageCore.Conversation;

namespace ChatServer.Network.Packets.AfterLogin.Message
{
    public class GetConversationShortInfoRequest : RequestPacket
    {
        public Guid ConversationID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            session.Send(createResponde(session));
        }

        public IPacket createResponde(ISession session) {
            GetConversationShortInfoResponse packet = new GetConversationShortInfoResponse();

            packet.ConversationID = ConversationID.ToString();

            ChatSession chatSession = session as ChatSession;
            AbstractConversation conversation = new ConversationStore().Load(ConversationID);

            int cnt = 0;
            packet.ConversationName = "";
            if (string.IsNullOrEmpty(conversation.ConversationName) || conversation.ConversationName.Equals("~"))
            {
                foreach (var member in conversation.Members)
                {
                    if (member.CompareTo(chatSession.Owner.ID) == 0) continue;
                    string name = new ChatUserStore().Load(member).FirstName;
                    packet.ConversationName += name + ", ";
                    cnt++;

                    if (cnt >= 2) break;
                }

                if (cnt >= 2)
                    packet.ConversationName += "and " + (conversation.Members.Count - 3) + "others...";
                else
                    packet.ConversationName = packet.ConversationName.Replace(", ", "");
            }

            conversation.UpdateLastActive(chatSession);
            packet.LastActive = conversation.LastActive;
            return packet;
        }
    }
}
