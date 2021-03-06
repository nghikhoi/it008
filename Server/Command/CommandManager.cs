using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Command
{
    public class CommandManager
    {
        SortedDictionary<String, ICommandExecutor> registeredCommands = null;
        SortedDictionary<String, ICommandExecutor> defaultCommands = null;

        SimpleChatServer server = null;

        private CommandManager()
        {
            this.server = SimpleChatServer.GetServer();
            registeredCommands = new SortedDictionary<string, ICommandExecutor>(StringComparer.OrdinalIgnoreCase);
            defaultCommands = new SortedDictionary<string, ICommandExecutor>(StringComparer.OrdinalIgnoreCase);

            RegisterAllDefaultCommands();
        }

        private void RegisterAllDefaultCommands()
        {
            defaultCommands.Add(DefaultCommands.STOP, new StopCommand());
        }

        public void RegisterCommand(string commandLabel, ICommandExecutor executor)
        {
            try
            {
                if (commandLabel == null) return;
                commandLabel = commandLabel.Trim();

                if (commandLabel.Length == 0)
                {
                    Exception e = new Exception(String.Format("The command must have at least 1 character (excluding spaces)!"));
                    throw e;
                }
                if (executor == null)
                {
                    Exception e = new Exception(String.Format("Executor can't be null!!!"));
                    throw e;
                }
                if (registeredCommands.ContainsKey(commandLabel))
                {
                    Exception e = new Exception(String.Format("The command \"{0}\" already exists!!!", commandLabel));
                    throw e;
                }
                registeredCommands.Add(commandLabel, executor);
                server.Logger.Info(String.Format("  - Command {0} registered successfully.", commandLabel));
            } catch (Exception e)
            {
                server.Logger.Error(e);
            }
        }

        public void ExecuteCommand(ISender sender, String command)
        {
            try
            {
                if (command == null) return;
                command = command.Trim();
                if (command.Length < 1) return;

                String[] args = command.Split(' ');
                String commandLabel = args[0];

                if (!registeredCommands.ContainsKey(commandLabel) && !defaultCommands.ContainsKey(commandLabel))
                {
                    server.Logger.Error(String.Format("Command \"{0}\" is not exists!!!", commandLabel));
                    return;
                }

                if (registeredCommands.ContainsKey(commandLabel))
                {
                    registeredCommands[commandLabel].Execute(sender, commandLabel, args);
                    return;
                }

                if (defaultCommands.ContainsKey(commandLabel))
                {
                    defaultCommands[commandLabel].Execute(sender, commandLabel, args);
                    return;
                }
            } catch (Exception e)
            {
                server.Logger.Error(e);
                throw;
            }
        }

        public void Unregister(string commandLabel)
        {
            commandLabel = commandLabel.Trim();
            if (registeredCommands.ContainsKey(commandLabel))
            {
                registeredCommands.Remove(commandLabel);
            }
        }

        public void UnregisterAll()
        {
            registeredCommands.Clear();
        }

        public static void StartService(bool forceRestart = false)
        {
            try
            {
                if (Instance == null || forceRestart)
                {
                    Instance = new CommandManager();
                    SimpleChatServer.GetServer().Logger.Info("Command Service started successfully");
                }
            } catch (Exception e)
            {
                SimpleChatServer.GetServer().Logger.Error(e);
            }
        }

        public static CommandManager Instance { get; private set; }
    }
}
