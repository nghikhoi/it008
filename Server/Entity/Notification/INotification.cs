using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Entity.Notification {
    public interface INotification {

        Guid Id { get; set; }

        Guid TargetUser { get; set; }

        long CreatedTime { get; set; }

        NotificationType Type();

    }
}
