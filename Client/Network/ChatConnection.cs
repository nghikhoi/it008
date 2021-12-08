using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using UI.Network.Packets;
using UI.Network.Pipeline;
using UI.Network.Protocol;
using UI.Services;

namespace UI.Network
{
    public class ChatConnection : IConnectionManager
    {
        static ChatConnection instance;
        public static ChatConnection Instance {
            get => instance;
            set => instance = value;
        }
        public ClientSession Session { get; private set; }
        protected IEventLoopGroup workerGroup;
        protected Bootstrap bootstrap;
        protected IChannel Channel;
        protected ProtocolProvider protocolProvider;
        private PacketRespondeListener _packetRespondeListener;
        private RespondeManager _respondeManager;
        private IAppSession _appSession;

        public String Host { get; private set; } = String.Empty;
        public int Port { get; private set; } = 1402;

        public String WebHost { get; private set; } = String.Empty;
        public int WebPort { get; private set; } = 1403;

        private bool CanReconnect = false;

        public ChatConnection(IAppSession appSession, ProtocolProvider protocolProvider, PacketRespondeListener packetRespondeListener, RespondeManager respondeManager) {
            Instance = this;
            _appSession = appSession;
            _packetRespondeListener = packetRespondeListener;
            _respondeManager = respondeManager;
            _packetRespondeListener.LoginRespondeEvent += ((session, responde) => ConnectResponde(responde.StatusCode));
            _packetRespondeListener.ReconnectRespondeEvent += ((session, responde) => ConnectResponde(responde.StatusCode));

            this.Host = "127.0.0.1";
            //this.Port = 8080;

            this.WebHost = "127.0.0.1";
            //this.WebPort = 22;

            this.protocolProvider = protocolProvider;
            this.bootstrap = new Bootstrap();
            this.workerGroup = new MultithreadEventLoopGroup();

            bootstrap
                .Group(workerGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ChannelInitializer(this));
        }

        private void ConnectResponde(int code) {
            if (code == 200) {
                CanReconnect = true;
                Console.WriteLine("Reconnected!");
            } else CanReconnect = false;
        }

        public async Task Bind()
        {
            IPAddress address = Dns.GetHostAddresses(Host)[0];
            await Bind(new IPEndPoint(address, Port));
        }

        public async Task Bind(IPEndPoint address)
        {
            try
            {
                Channel = await this.bootstrap.ConnectAsync(address);
                OnBindSuccess(address);
            }
            catch (Exception e)
            {
                OnBindFailure(address, e);
            }
        }

        public bool IsConnected()
        {
            return Channel != null && Channel.Active;
        }

        public ISession NewSession(IChannel c)
        {
            Session = new ClientSession(c, protocolProvider, _respondeManager, _packetRespondeListener);
            return Session;
        }

        public void SessionInactivated(ISession session)
        {
            Console.WriteLine("Server has disconnected!!!");
            if (CanReconnect)
            {
                ReconnectResquest packet = new ReconnectResquest();
                packet.UserID = _appSession.SessionID;
                packet.Hash = _appSession.SessionKey;
                Send(packet);
            }
        }

        public void Shutdown()
        {
            Channel.CloseAsync();
            bootstrap.Group().ShutdownGracefullyAsync();
        }
        public void OnBindSuccess(IPEndPoint address)
        {
            Console.WriteLine("Connected to " + address + ". Session ID: " + Session.SessionID);
        }

        public void OnBindFailure(IPEndPoint address, Exception e)
        {
            
        }

        private void TryReconnect(IPacket sendingPacket = null) {
            if (CanReconnect && sendingPacket is ReconnectResquest) {
                new Task(() => {
                    Console.WriteLine("Reconnecting...");
                    Thread.Sleep(3000);
                    Send(sendingPacket);
                }).Start();
            }
        }
        
        public async Task<TExpectPacket> Send<TExpectPacket>(IPacket packet) where TExpectPacket : IPacket {
            try
            {
                if (Session == null || !IsConnected()) {
                    await Bind();
                }
                if (Session == null || !IsConnected()) {
                    throw new ProtocolViolationException("Cannot connect to server");
                }
                return await Session.Send<TExpectPacket>(packet);
            } catch {
                TryReconnect(packet);
                throw;
            }
        }

        public async Task Send(IPacket packet) {
            try {
                if (Session == null || !IsConnected()) {
                    await Bind();
                }
                if (Session == null || !IsConnected()) {
                    throw new ProtocolViolationException("Cannot connect to server");
                }
                Session.Send(packet);
            } catch {
                TryReconnect(packet);
                throw;
            }
        }

    }
}
