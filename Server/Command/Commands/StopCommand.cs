using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Command.Commands
{
    public class StopCommand : ICommandExecutor
    {
        public void Execute(ISender commandSender, string commandLabel, string[] args)
        {
            SimpleChatServer.GetServer().Logger.Info("Stopping...");
            ConsoleManager.Stop();
            Environment.Exit(0);
        }
    }
}
