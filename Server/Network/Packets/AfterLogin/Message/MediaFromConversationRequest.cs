using System;
using ChatServer.Entity;
using ChatServer.IO.Message;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;

namespace ChatServer.Network.Packets
{
    public class MediaFromConversationRequest : AbstractRequestPacket
    {
        public Guid ConversationID { get; set; }
        public int MediaPosition { get; set; }
        public int Quantity { get; set; }

        public override void Decode(IByteBuffer buffer)
        {
            ConversationID = Guid.Parse(ByteBufUtils.ReadUTF8(buffer));
            MediaPosition = buffer.ReadInt();
            Quantity = buffer.ReadInt();
        }

        public override IPacket createResponde(ChatSession session) {
            ChatSession chatSession = session as ChatSession;

            AbstractConversation conversation = new ConversationStore().Load(ConversationID);
            if (conversation == null) return null;

            MediaFromConversationResponse packet = new MediaFromConversationResponse();

            if (conversation.MediaID.Count > 0)
            {
                for (int i = MediaPosition; i >= Math.Max(0, MediaPosition - Quantity + 1); --i)
                {
                    AbstractMessage mediaMessage = new MessageStore().Load(conversation.MediaID[i], ConversationID);

                    string fileID, fileName;
                    fileID = fileName = "~";

                    if (mediaMessage is ImageMessage)
                    {
                        fileID = (mediaMessage as ImageMessage).FileID;
                        fileName = (mediaMessage as ImageMessage).FileName;
                    }
                    else if (mediaMessage is VideoMessage)
                    {
                        fileID = (mediaMessage as VideoMessage).FileID;
                        fileName = (mediaMessage as VideoMessage).FileName;
                    }

                    if (fileID.Equals("~") || fileName.Equals("~")) continue;
                    packet.Positions.Add(i);
                    packet.FileIDs.Add(fileID);
                    packet.FileNames.Add(fileName);
                }
            }

            return packet;
        }
    }
}
