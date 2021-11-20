﻿using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Network.Packets.AfterLogin.Sticker
{
    public class GetNearestSickerRequest : RequestPacket
    {
        public void Decode(IByteBuffer buffer)
        {

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
            ChatSession chatSession = session as ChatSession;
            GetNearestSickerResponse response = new GetNearestSickerResponse();
            for (int i = 0; i < 20 && i < chatSession.Owner.NearestStickers.Count; i++)
            {
                response.NearestSticker.Add(chatSession.Owner.NearestStickers[i]);
            }

            return response;
        }
    }
}
