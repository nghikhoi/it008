using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;
using UI.MVC;

namespace UI.Network.Packets.Register
{
    public class RegisterResult : IPacket
    {
        public int StatusCode { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            StatusCode = buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            Authentication module = ModuleContainer.GetModule<Authentication>();
            if (module == null) return;
            Application.Current.Dispatcher.Invoke(() => module.controller.RegisterResponse(StatusCode));
        }
    }
}
