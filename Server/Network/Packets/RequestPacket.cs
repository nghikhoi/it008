using CNetwork;
using CNetwork.Sessions;

namespace ChatServer.Network.Packets {
	
	public interface RequestPacket : IPacket {

		IPacket createResponde(ISession session);

	}
}