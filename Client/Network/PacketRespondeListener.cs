using System;
using CNetwork.Sessions;
using UI.Network.Packets;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.Login;
using UI.Network.Packets.Register;

namespace UI.Network {

	public delegate void LoginRespondeHandler(ISession session, LoginResult responde);
	public delegate void ReconnectRespondeHandler(ISession session, ReconnectResponse responde);
	public delegate void RegisterRespondeHandler(ISession session, RegisterResult responde);
	public delegate void ReceiveMessageHandler(ISession session, ReceiveMessage responde);
	
	public class PacketRespondeListener {

		public event LoginRespondeHandler LoginRespondeEvent;
		internal void OnLoginResponde(ISession session, LoginResult responde) {
			LoginRespondeEvent?.Invoke(session, responde);
		}
		public event ReconnectRespondeHandler ReconnectRespondeEvent;
		internal void OnReconnectResponde(ISession session, ReconnectResponse responde) {
			ReconnectRespondeEvent?.Invoke(session, responde);
		}
		public event RegisterRespondeHandler RegisterRespondeEvent;
		internal void OnRegisterResponde(ISession session, RegisterResult responde) {
			RegisterRespondeEvent?.Invoke(session, responde);
		}
		public event ReceiveMessageHandler ReceiveMessageEvent;
		internal void OnReceiveMessage(ISession session, ReceiveMessage responde) {
			ReceiveMessageEvent?.Invoke(session, responde);
		}

	}
}