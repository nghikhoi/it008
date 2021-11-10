using System;
using System.Collections.Generic;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Message
{
    public class GetMediaFromConversationResult : IPacket
    {
        public List<string> FileIDs { get; set; } = new List<string>();
        public List<string> FileNames { get; set; } = new List<string>();
        public List<int> Positions { get; set; } = new List<int>();

        public void Decode(IByteBuffer buffer)
        {
            string id = ByteBufUtils.ReadUTF8(buffer);
            while (!id.Equals("~"))
            {
                FileIDs.Add(id);
                FileNames.Add(ByteBufUtils.ReadUTF8(buffer));
                Positions.Add(buffer.ReadInt());
                id = ByteBufUtils.ReadUTF8(buffer);
            }
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            Console.WriteLine("New media player");
            Conversation conversation = null; //TODO
            var module = ModuleContainer.GetModule<ChatContainer>();
            module.controller.AddShortInfoConversation(conversation);
            /*Application.Current.Dispatcher.Invoke(() =>
            {
                //MainWindow.Instance.MediaPlayerWindow = new MediaPlayerWindow();
                var app = MainWindow.chatApplication;

                if (app.model.MediaWindows.ContainsKey(app.model.currentSelectedConversation))
                {
                    if (app.model.MediaWindows[app.model.currentSelectedConversation] != null)
                    {
                        var mediaPlayer = app.model.MediaWindows[app.model.currentSelectedConversation];

                        for (int i = 0; i < FileIDs.Count; ++i)
                        {
                            mediaPlayer.MediaPlayer.AddMediaItem
                            (
                                conversationID: app.model.currentSelectedConversation,
                                fileID: FileIDs[i],
                                fileName: FileNames[i],
                                position: Positions[i],
                                reachedRight: app.model.Conversations[app.model.currentSelectedConversation].LastMediaID < 0
                            );
                            Console.WriteLine("Added");
                        }

                        mediaPlayer.MediaPlayer.FillGallery();
                    }
                }
            });*/
        }
    }
}
