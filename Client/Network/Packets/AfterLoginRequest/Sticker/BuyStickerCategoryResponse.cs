using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Sticker
{
    public class BuyStickerCategoryResponse : IPacket
    {
        public int CateID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            CateID = buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            //TODO
            // da mua dc sticker
            /*if (!UI.MessageCore.Sticker.Sticker.LoadedCategories.ContainsKey(CateID)) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                UI.MessageCore.Sticker.Sticker.LoadedCategories.TryGetValue(CateID, out var stickerCate);
                ChatPage.Instance.spTabStickerContainner.AddTabSticker(stickerCate);
                ChatPage.Instance.spTabStickerContainner.RemoveCateInStore(stickerCate);
            });*/
        }
    }
}
