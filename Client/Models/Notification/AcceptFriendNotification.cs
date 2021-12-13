namespace UI.Models.Notification {
    public class AcceptFriendNotification : CommunicateNotification {
        public override NotificationType Type()
        {
            return NotificationType.ACCEPT_FRIEND_RESPONDE;
        }
    }
}
