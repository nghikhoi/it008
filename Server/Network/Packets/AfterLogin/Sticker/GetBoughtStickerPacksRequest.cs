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
    public class GetBoughtStickerPacksRequest : AbstractRequestPacket
    {
        public override void Decode(IByteBuffer buffer)
        {

        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;
            GetBoughtStickerPacksResponse response = new GetBoughtStickerPacksResponse();
            response.BoughtStickerPacks.AddRange(chatSession.Owner.BoughtStickerPacks);
            return response;
        }
    }
}
