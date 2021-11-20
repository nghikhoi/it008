using System;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace UI.Network.Packets.AfterLoginRequest.Profile
{
    /// <summary>
    /// This packet gets user-self profile in order to allow him/her modify his/her private information
    /// The information will be displayed in Setting tab
    /// </summary>
    public class GetSelfProfile : RequestPacket
    {
        public void Decode(IByteBuffer buffer)
        {

        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
