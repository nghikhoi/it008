using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Notification
{
    public class ResponseFriendRequest : IPacket
    {
        public string TargetID { get; set; }
        public bool Accepted { get; set; }
        public int NotiPosition { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, TargetID);
            byteBuf.WriteBoolean(Accepted);
            byteBuf.WriteInt(NotiPosition);
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
