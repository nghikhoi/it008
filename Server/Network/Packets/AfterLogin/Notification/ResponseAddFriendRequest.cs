using System;
using ChatServer.Command;
using ChatServer.Entity;
using ChatServer.Entity.Notification;
using ChatServer.IO.Entity;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class ResponseAddFriendRequest : IPacket
    {
        public Guid TargetID { get; set; }
        public bool Accepted { get; set; }
        public Guid NotificationId { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            TargetID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            Accepted = buffer.ReadBoolean();
            NotificationId = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session) {
            ChatSession chatSession = session as ChatSession;

            if (Accepted) {
                string command = "sample mkfriend " + chatSession.Owner.Email + " " + new ChatUserStore().Load(TargetID).Email;
                Command.CommandManager.Instance.ExecuteCommand(ConsoleSender.Instance, command);

                chatSession.Send(new FinalizeAcceptedFriendRequest());
                ChatUser user;
                if (ChatUserManager.OnlineUsers.TryGetValue(TargetID, out user)) {
                    
                }

                // string encNoti = "acfriend:" + chatSession.Owner.ID + ":" +
                //    chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;

                /*string name = chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;
                string encNoti = NotificationEncoder.Assemble(
                    NotificationPrefixes.AcceptedFriend,
                    chatSession.Owner.ID.ToString(),
                    name, name, "accepted your friend request.",
                    false);*/

                CommunicateNotification noti = new AcceptFriendNotification();
                noti.CreatedTime = new DateTime().Millisecond;
                noti.Id = Guid.NewGuid();
                noti.TargetUser = TargetID;
                noti.SenderName = chatSession.Owner.FirstName + " " + chatSession.Owner.LastName;
                noti.SenderUser = chatSession.Owner.ID;

                user = new ChatUserStore().Load(TargetID);
                user.AddNotification(noti);
                user.Save();

                chatSession.Owner.Notifications.Remove(NotificationId);
                chatSession.Owner.Save();
            }
        }
    }
}
