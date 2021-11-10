using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Utils;

namespace UI.Network.Packets.AfterLoginRequest.Profile
{
    public class ModifyPassword : IPacket
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, HashUtils.MD5(OldPassword));
            ByteBufUtils.WriteUTF8(byteBuf, HashUtils.MD5(NewPassword));
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
