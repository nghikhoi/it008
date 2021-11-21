using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest
{
    public class GetFriendIDs : RequestPacket {
        public string FriendOfID { get; set; } = ChatModel.Instance.SelfID;

        public void Decode(IByteBuffer buffer)
        {
            // throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, FriendOfID);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            
        }
    }
}
