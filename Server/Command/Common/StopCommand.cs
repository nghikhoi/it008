using System;

namespace ChatServer.Command
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
