namespace UI.Models.Notification {
    public class FriendRequestNotification : CommunicateNotification {
        public override NotificationType Type()
        {
            return NotificationType.FRIEND_REQUEST;
        }
    }
}
