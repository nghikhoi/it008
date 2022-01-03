using ChatServer.Entity;
using ChatServer.Entity.EntityProperty;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class ChatThemeSetRequest : IPacket
    {
        public ChatTheme Theme { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            Theme = new ChatTheme();
            Theme.BackgroundId = ByteBufUtils.ReadUTF8(buffer);
            Theme.BackgroundBlur = buffer.ReadInt();

            Theme.BackgroundColor = buffer.ReadInt();
            Theme.Use = (BackgroundType)buffer.ReadInt();

            Theme.IconColor = buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            ByteBufUtils.WriteUTF8(byteBuf, Theme.BackgroundId);
            byteBuf.WriteInt(Theme.BackgroundBlur);

            byteBuf.WriteInt(Theme.BackgroundColor);

            byteBuf.WriteInt((int)Theme.Use);

            byteBuf.WriteInt(Theme.IconColor);

            return byteBuf;
        }

        public void Handle(ISession session)
        {
            ChatSession chatSession = session as ChatSession;
            ChatUser user = chatSession.Owner;

            user.ChatTheme.BackgroundId = Theme.BackgroundId;
            user.ChatTheme.BackgroundBlur = Theme.BackgroundBlur;

            user.ChatTheme.BackgroundColor = Theme.BackgroundColor;

            user.ChatTheme.Use = Theme.Use;

            user.ChatTheme.IconColor = Theme.IconColor;

            user.SaveChatTheme();

            user.Send(this, chatSession);
        }
    }
}
