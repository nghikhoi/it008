using System;
using ChatServer.Entity;
using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class LoginRequest : IPacket
    {
        public string Username { get; set; }
        public string Passhash { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            Username = ByteBufUtils.ReadUTF8(buffer);
            Passhash = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;

            LoginResult respone = new LoginResult();
            respone.Token = "~";

            Guid id = ProfileCache.Instance.ParseEmailToGuid(Username);

            if (id == Guid.Empty)
            {
                respone.StatusCode = ResponeCode.NotFound;
                chatSession.Send(respone);
                chatSession.Disconnect();
                return;
            }

            ChatUserProfile profile = ProfileCache.Instance.GetUserProfile(id);
            Passhash = HashUtils.MD5(Passhash + id);

            if (profile.PassHashed != Passhash)
            {
                respone.StatusCode = ResponeCode.Unauthorized;
                chatSession.Send(respone);
                chatSession.Disconnect();
                return;
            }

            if (profile.Banned)
            {
                respone.StatusCode = ResponeCode.Forbidden;
                chatSession.Send(respone);
                chatSession.Disconnect();
                return;
            }

            respone.StatusCode = ResponeCode.OK;
            respone.Token = chatSession.SessionID.ToString();
            chatSession.Send(respone);
            chatSession.FinalizeLogin(profile);
        }
    }
}
