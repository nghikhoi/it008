using ChatServer.Utils;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class ModifyPasswordRequest : AbstractRequestPacket
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            OldPassword = ByteBufUtils.ReadUTF8(buffer);
            NewPassword = ByteBufUtils.ReadUTF8(buffer);
        }

        public override IPacket createResponde(ISession session) {
            ChatSession chatSession = session as ChatSession;

            SimpleChatServer.GetServer().Logger.Debug(chatSession.Owner.Password);
            SimpleChatServer.GetServer().Logger.Debug(HashUtils.MD5(OldPassword + chatSession.Owner.ID)); 
            SimpleChatServer.GetServer().Logger.Debug(HashUtils.MD5(OldPassword)); 

            bool result = false;
            if (HashUtils.MD5(OldPassword + chatSession.Owner.ID).Equals(chatSession.Owner.Password))
            {
                chatSession.Owner.Password = HashUtils.MD5(NewPassword + chatSession.Owner.ID);
                chatSession.Owner.Save();
                result = true;
            }

            ModifyPasswordResponse packet = new ModifyPasswordResponse();
            packet.Result = result;
            return packet;
        }
    }
}
