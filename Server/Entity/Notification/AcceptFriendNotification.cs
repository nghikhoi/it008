using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Entity.Notification {
    public class AcceptFriendNotification : CommunicateNotification {
        public override NotificationType Type()
        {
            return NotificationType.ACCEPT_FRIEND_RESPONDE;
        }
    }
}
