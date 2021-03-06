using System;
using ChatServer.Entity;
using ChatServer.Entity.EntityProperty;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class RegisterRequest : IPacket
    {
        public string Email { get; set; }
        public string PassHashed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DoB { get; set; }
        public Gender Gender { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            Email = ByteBufUtils.ReadUTF8(buffer);
            PassHashed = ByteBufUtils.ReadUTF8(buffer);
            FirstName = ByteBufUtils.ReadUTF8(buffer);
            LastName = ByteBufUtils.ReadUTF8(buffer);
            DoB = new DateTime(buffer.ReadLong());
            Gender = (Gender)buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;

            RegisterResult responePacket = new RegisterResult();

            if (ProfileCache.Instance.ParseEmailToGuid(Email) != Guid.Empty)
            {
                responePacket.StatusCode = ResponeCode.Conflict;
                chatSession.Send(responePacket);
                chatSession.Disconnect();
                return;
            }

            Guid id = Guid.NewGuid();

            ChatUser user = new ChatUser()
            {
                ID = id,
                Email = this.Email,
                Password = HashUtils.MD5(PassHashed + id),
                FirstName = this.FirstName,
                LastName = this.LastName,
                DateOfBirth = this.DoB,
                Gender = this.Gender
            };

            bool added = user.Save();
            if (added)
            {
                responePacket.StatusCode = ResponeCode.OK;
                SimpleChatServer.GetServer().Logger.Info(String.Format("Account {0} has registered successfully.", user.Email));
            } else
            {
                responePacket.StatusCode = ResponeCode.NotFound;
            }

            chatSession.Send(responePacket);
            chatSession.Disconnect();
        }
    }
}
