using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest
{
    public class GetShortInfo : IPacket
    {
        public string ID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            //throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, ID);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            
        }
    }
}
