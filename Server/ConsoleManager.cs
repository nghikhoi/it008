﻿using CNetwork.Sessions;
using ChatServer.Network.Packets;
using ChatServer.Utils.ThreadUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Command;

namespace ChatServer
{
    public class ConsoleManager
    {
        static Thread consoleReader = null;
        static bool IsStop = false;

        public ConsoleManager()
        {
            Setup();
            consoleReader = new Thread(() =>
            {
                String input;
                while (!IsStop)
                {
                    try
                    {
                        input = Console.ReadLine();
                        DeletePrevConsoleLine();
                        SimpleChatServer.GetServer().Logger.Info(input);

                        if (input == null || input.Trim().Length == 0) continue;

                        CommandManager.Instance.ExecuteCommand(ConsoleSender.Instance, input);
                    } catch (Exception e)
                    {
                        SimpleChatServer.GetServer().Logger.Error(e);
                    }
                }
            });
            consoleReader.Start();
        }

        private void Setup()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "SimpleChat Server";
        }

        private void DeletePrevConsoleLine()
        {
            if (Console.CursorTop == 0) return;
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public static void Stop()
        {
            IsStop = true;
        }
    }
}
