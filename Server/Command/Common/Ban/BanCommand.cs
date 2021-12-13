using System;
using ChatServer.Entity;

namespace ChatServer.Command
{
    public class BanCommand : ICommandExecutor
    {
        public void Execute(ISender commandSender, string commandLabel, string[] args)
        {
            if (args.Length < 2) return;

            Guid id = ProfileCache.Instance.ParseEmailToGuid(args[1].ToLower());

            if (id.Equals(Guid.Empty))
            {
                SimpleChatServer.GetServer().Logger.Error("Email is not exists.");
                return;
            }

            ChatUser user = ChatUserManager.LoadUser(id);

            user.Banned = true;

            user.Save();

            if (user.IsOnline())
            {
                user.Kick();
            }
        }
    }
}
