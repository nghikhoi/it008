using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace ChatServer.Entity.Notification {
    public class FriendRequestNotification : CommunicateNotification {
        public override NotificationType Type()
        {
            return NotificationType.FRIEND_REQUEST;
        }
    }
}
