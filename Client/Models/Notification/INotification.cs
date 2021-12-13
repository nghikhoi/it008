using System;

namespace UI.Models.Notification {
    public interface INotification {

        Guid Id { get; set; }

        Guid TargetUser { get; set; }

        long CreatedTime { get; set; }

        NotificationType Type();

    }
}
