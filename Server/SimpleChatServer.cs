using CNetwork;
using DotNetty.Transport.Channels;
using log4net;
using ChatServer.Entity;
using ChatServer.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ChatServer.Command;
using ChatServer.IO.Storage;
using ChatServer.Network;
using ChatServer.Network.Protocol;
using ChatServer.Network.RestAPI;
using ChatServer.Utils.ThreadUtils;

namespace ChatServer
{
    public class SimpleChatServer
    {
        static SimpleChatServer instance = null;

        public IPAddress IP { get; set; }
        public int Port { get; set; }

        public Network.ChatServer Network { get; private set; }
        public RestServer RestAPI { get; private set; }

        public ILog Logger { get; } = LogManager.GetLogger("Main");

        private ProtocolProvider protocolProvider;

        public SessionRegistry SessionRegistry { get; }

        public SimpleChatServer()
        {
            instance = this;
            new ServerSettings();

            protocolProvider = new ProtocolProvider();
            SessionRegistry = new SessionRegistry();

            Mongo.StartService();
            ProfileCache.StartService();
            CommandManager.StartService();

            RegisterCommand();

            Sticker.StartService();

            StartNetworkService();
        }

        private void StartNetworkService()
        {
            CountdownLatch latch = new CountdownLatch(1);

            this.Network = new Network.ChatServer(this, protocolProvider, latch);
            _ = this.Network.Bind(new IPEndPoint(ServerSettings.SERVER_HOST, ServerSettings.SERVER_PORT));

            RestAPI = new RestServer();
            RestAPI.Start(ServerSettings.WEBSERVER_HOST, ServerSettings.WEBSERVER_PORT, latch);

            latch.Wait();

            new ConsoleManager();
        }

        public void RegisterCommand()
        {
            GetCommandManager().RegisterCommand("sample", new SampleCommand());
            GetCommandManager().RegisterCommand("ban", new BanCommand());
            GetCommandManager().RegisterCommand("unban", new UnbanCommand());
        }

        public static CommandManager GetCommandManager()
        {
            return CommandManager.Instance;
        }

        public static SimpleChatServer GetServer()
        {
            return instance;
        }
    }
}
