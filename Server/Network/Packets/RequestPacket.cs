using System;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets {
	
	public interface RequestPacket : IPacket {

		Action<Action<IPacket>> createRespondeAction(ISession session);

	}

	public abstract class AbstractRequestPacket : RequestPacket {
		public abstract void Decode(IByteBuffer buffer);

		public IByteBuffer Encode(IByteBuffer byteBuf) {
			throw new NotImplementedException();
		}

		public void Handle(ISession session) {
			createRespondeAction(session).Invoke(session.Send);
		}

		public Action<Action<IPacket>> createRespondeAction(ISession session) {
			return action =>
            {
                IPacket response = createResponde((ChatSession) session);
                if (response == null)
                    throw new NullReferenceException("Response packet null.");
                action.Invoke(response);
            };
		}

		public abstract IPacket createResponde(ChatSession session);

	}
	
}