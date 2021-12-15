using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace ChatServer.Network {
    public interface INetworkData
    {

        void Encode(IByteBuffer buffer);

        void Decode(IByteBuffer buffer);

    }
}
