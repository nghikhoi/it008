﻿using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Network.Packets.AfterLogin.DataPreparing
{
    public class ChatThemeGetRequest : RequestPacket
    {
        public void Decode(IByteBuffer buffer)
        {

        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;
            chatSession.Send(createResponde(session));
        }

        public IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            ChatThemeSetRequest response = new ChatThemeSetRequest();

            response.Theme = chatSession.Owner.ChatThemeSettings;

            session.Send(response);
            return response;
        }
    }
}
