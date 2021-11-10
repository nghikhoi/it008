using System;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;

namespace UI.Network.Packets
{
    public class ReconnectResponse : IPacket
    {
        public int StatusCode { get; set; }
        public string Token { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            StatusCode = buffer.ReadInt();
            Token = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            if (StatusCode == 200)
            {
                (session as ClientSession).LoggedIn(Token);
                ModuleContainer.OnDisconnection();
            }
            ChatConnection.Instance.OnResponse(StatusCode);
        }
    }
}
