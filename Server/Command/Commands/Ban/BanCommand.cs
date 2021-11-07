using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;
using ChatServer.Entity.Meta.Profile;

namespace ChatServer.Command.Commands.Ban
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
