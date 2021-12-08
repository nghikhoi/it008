using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest
{
    public class GetSelfIDResult : IPacket
    {
        public string ID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            ID = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            //TODO
            /*Application.Current.Dispatcher.Invoke(() =>
            {
                ChatModel.Instance.SelfID = ID;
            });*/
        }
    }
}
