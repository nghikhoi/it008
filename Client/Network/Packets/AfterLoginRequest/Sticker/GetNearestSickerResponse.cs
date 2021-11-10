using System;
using System.Collections.Generic;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Sticker
{
    public class GetNearestSickerResponse : IPacket
    {
        public List<int> NearestSticker { get; set; } = new List<int>();

        public void Decode(IByteBuffer buffer)
        {
            int id = buffer.ReadInt();
            while (id > 0)
            {
                NearestSticker.Add(id);
                id = buffer.ReadInt();
            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            //TODO
            /*foreach (var sticker in NearestSticker)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageCore.Sticker.Sticker.LoadedStickers.TryGetValue(sticker, out var recentSticker);
                    ChatPage.Instance.spTabStickerContainner.AddStickerToRecentTab(recentSticker);

                });
            }*/
        }
    }
}
