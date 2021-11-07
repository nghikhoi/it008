using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Utils.ThreadUtils;

namespace ChatServer.Network.RestAPI
{
    public class RestServer
    {
        private IDisposable _webapp;

        public void Start(String ip, int port, CountdownLatch latch)
        {
            try
            {
                _webapp = WebApp.Start<Startup>(String.Format("http://{0}:{1}", ip, port));
                SimpleChatServer.GetServer().Logger.Info("Bind success. REST Server is listening on " + ip + ":" + port);
                latch.Signal();
            } catch (Exception e)
            {
                SimpleChatServer.GetServer().Logger.Error(e);
            }
        }

        public void Stop()
        {
            _webapp?.Dispose();
        }
    }
}
