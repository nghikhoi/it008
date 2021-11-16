﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using UI.Network.Packets;
using UI.Network.Pipeline;
using UI.Network.Protocol;
using UI.MVC;

namespace UI.Network
{
    public class ChatConnection : IConnectionManager
    {
        public ClientSession Session { get; private set; }
        static ChatConnection instance;
        protected IEventLoopGroup workerGroup;
        protected Bootstrap bootstrap;
        protected IChannel Channel;
        protected ProtocolProvider protocolProvider;

        public String Host { get; private set; } = String.Empty;
        public int Port { get; private set; } = 1402;

        public String WebHost { get; private set; } = String.Empty;
        public int WebPort { get; private set; } = 1403;

        private bool CanReconnect = false;

        private ChatConnection(ProtocolProvider protocolProvider)
        {
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
            Session = new ClientSession(c, protocolProvider);
            return Session;
        }

        public void SessionInactivated(ISession session)
        {
            Console.WriteLine("Server has disconnected!!!");
            Application.Current.Dispatcher.Invoke(() =>
            {
                ModuleContainer.OnDisconnection(lostConnection: true);
            });
            if (CanReconnect)
            {
                ReconnectResquest packet = new ReconnectResquest();
                ChatModel model = ChatModel.Instance;
                packet.UserID = model.SelfID;
                packet.Hash = model.Hashed;
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
            Console.WriteLine("Connected to " + address);
        }

        public void OnBindFailure(IPEndPoint address, Exception e)
        {
        }

        public async Task Send(IPacket packet)
        {
            try
            {
                if (Session == null || !IsConnected())
                {
                    await Bind();
                }
                if (Session == null || !IsConnected())
                {
                    throw new ProtocolViolationException("Cannot connect to server");
                }
                Session.Send(packet);
            } catch 
            {
                if (packet is ReconnectResquest && CanReconnect)
                {
                    new Task(() =>
                    {
                        Console.WriteLine("Reconnecting...");
                        Thread.Sleep(3000);
                        Send(packet);
                    }).Start();
                }
                throw;
            }
        }

        public void OnResponse(int code)
        {
            if (code == 200)
            {
                CanReconnect = true;
                Console.WriteLine("Reconnected!");
            }
            else
                CanReconnect = false;
        }

        public static ChatConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChatConnection(new ProtocolProvider());
                }
                return instance;
            }
        }
    }
}
