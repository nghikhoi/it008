using System;
using System.Collections.Generic;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Sticker
{
    public class GetBoughtStickerPacksResponse : IPacket
    {
        public List<int> BoughtStickerPacks { get; set; } = new List<int>();

        public void Decode(IByteBuffer buffer)
        {
            int id = buffer.ReadInt();

            while (id > 0)
            {
                BoughtStickerPacks.Add(id);
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
            /*foreach (var Cate in MessageCore.Sticker.Sticker.LoadedCategories )
            {
                //Cate da duoc mua
                if (BoughtStickerPacks.Contains(Cate.Key))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        UI.MessageCore.Sticker.Sticker.LoadedCategories.TryGetValue(Cate.Key, out var stickerCate);
                        ChatPage.Instance.spTabStickerContainner.AddTabSticker(stickerCate);
                    });
                }
                else
                {
                    // cate chua duoc mua
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        UI.MessageCore.Sticker.Sticker.LoadedCategories.TryGetValue(Cate.Key, out var stickerCate);
                        ChatPage.Instance.spTabStickerContainner.AddToStoreList(stickerCate);
                    });
                }
            }*/

        }
    }
}
