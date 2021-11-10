using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;

namespace UI.Network.Packets.Login
{
    public class LoginResult : IPacket
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
            Authentication module = ModuleContainer.GetModule<Authentication>();
            if (module == null) return;
            if (StatusCode == 200)
            {
                (session as ClientSession).LoggedIn(Token);
            }
            Application.Current.Dispatcher.Invoke(() => module.controller.LoginResponde(StatusCode));
            ChatConnection.Instance.OnResponse(StatusCode);
        }
    }
}
