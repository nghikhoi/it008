using System;
using ChatServer.Entity;
using ChatServer.Entity.Notification;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class AddFriendRequest : IPacket
    {
        public string TargetID { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            TargetID = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;

            SimpleChatServer.GetServer().Logger.Debug("Friend request to " + TargetID + " from " + chatSession.Owner.ID);
            ChatUser user;
            /*if (ChatUserManager.OnlineUsers.TryGetValue(Guid.Parse(TargetID), out user))
            {
                ForwardedFriendRequest packet = new ForwardedFriendRequest();
                packet.SenderID = chatSession.Owner.ID.ToString();
                packet.Name = chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;
                SimpleChatServer.GetServer().Logger.Debug("Friend request " + packet.Name);
                user.Send(packet);
            }*/

            /*string name = chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;*/

            //string encNoti = "mkfriend:" + chatSession.Owner.ID + ":" +
            //    chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;

            /*string encNoti = NotificationEncoder.Assemble(
                NotificationPrefixes.AddFriend,
                chatSession.Owner.ID.ToString(),
                name, name, "sent you a friend request.",
                false);*/
            CommunicateNotification notification = new FriendRequestNotification();
            notification.TargetUser = Guid.Parse(TargetID);
            notification.SenderUser = chatSession.Owner.ID;
            notification.SenderName = chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;
            notification.CreatedTime = new DateTime().Millisecond;
            notification.Id = Guid.NewGuid();

            user = ChatUserManager.LoadUser(Guid.Parse(TargetID));
            user.AddNotification(notification);
            user.Save();
        }
    }
}
