using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Models.Message;
using UI.Network.RestAPI;

namespace UI.Models {
    public class MessagePackage
    {

        private readonly Queue<object> MessageValues = new Queue<object>();
        private readonly Queue<BubbleType> MessageTypes = new Queue<BubbleType>();

        private readonly IModelContext Model;
        private readonly Guid ConversationId;
        private readonly Guid SenderId;

        public MessagePackage(IModelContext model, Guid conversationId, Guid senderId)
        {
            Model = model;
            ConversationId = conversationId;
            SenderId = senderId;
        }

        public MessagePackage AddTextMessage(params string[] texts)
        {
            foreach (var text in texts)
            {
                MessageValues.Enqueue(text);
                MessageTypes.Enqueue(BubbleType.Text);
            }
            return this;
        }

        public MessagePackage AddEmoji(params string[] texts)
        {
            foreach (var text in texts)
            {
                MessageValues.Enqueue(text);
                MessageTypes.Enqueue(BubbleType.Emoji);
            }
            return this;
        }

        public MessagePackage AddAttachment(params string[] paths)
        {
            foreach (var path in paths)
            {
                MessageValues.Enqueue(path);
                MessageTypes.Enqueue(BubbleType.Attachment);
            }

            return this;
        }

        public MessagePackage AddVideo(params string[] paths) {
            foreach (var path in paths) {
                MessageValues.Enqueue(path);
                MessageTypes.Enqueue(BubbleType.Video);
            }

            return this;
        }

        public MessagePackage AddImage(params string[] paths) {
            foreach (var path in paths) {
                MessageValues.Enqueue(path);
                MessageTypes.Enqueue(BubbleType.Image);
            }

            return this;
        }

        public MessagePackage AddSticker(params int[] ids) {
            foreach (var id in ids) {
                MessageValues.Enqueue(id);
                MessageTypes.Enqueue(BubbleType.Sticker);
            }

            return this;
        }

        private async Task<AbstractMessage> BuildMessage(BubbleType type, object value)
        {
            AbstractMessage result;
            switch (type)
            {
                case BubbleType.Attachment:
                {
                    TaskCompletionSource<AttachmentMessage> source = new TaskCompletionSource<AttachmentMessage>();
                    FileAPI.UploadAttachment(this.ConversationId.ToString(), new List<string>() { value as string }, map =>
                    {
                        foreach (string name in map.Keys)
                        {
                            string id = map[name];
                            source.SetResult(new AttachmentMessage()
                            {
                                FileName = name,
                                FileID = id
                            });
                        }

                    }, error => { });
                    result = await source.Task;
                    break;
                }
                case BubbleType.Image:
                {
                    TaskCompletionSource<ImageMessage> source = new TaskCompletionSource<ImageMessage>();
                    FileAPI.UploadMedia(this.ConversationId.ToString(), new List<string>() { value as string }, map => {
                        foreach (string name in map.Keys) {
                            string id = map[name];
                            source.SetResult(new ImageMessage() {
                                FileName = name,
                                FileID = id
                            });
                        }

                    }, error => { });
                    result = await source.Task;
                    break;
                    }
                case BubbleType.Video:
                {
                    TaskCompletionSource<VideoMessage> source = new TaskCompletionSource<VideoMessage>();
                    FileAPI.UploadMedia(this.ConversationId.ToString(), new List<string>() { value as string }, map => {
                        foreach (string name in map.Keys) {
                            string id = map[name];
                            source.SetResult(new VideoMessage() {
                                FileName = name,
                                FileID = id
                            });
                        }

                    }, error => { });
                    result = await source.Task;
                    break;
                    }
                case BubbleType.Text:
                {
                    result = new TextMessage()
                    {
                        Message = value as string
                    };
                    break;
                }
                case BubbleType.Emoji:
                    {
                        result = new TextMessage()
                        {
                            Message = value as string
                        };
                        (result as TextMessage).isEmoji = true;
                        break;
                    }
                case BubbleType.Sticker:
                {
                    result = new StickerMessage()
                    {
                        Sticker = Sticker.LoadedStickers[(int) value]
                    };
                    break;
                }
                default: return null;
            }
            result.SenderID = SenderId.ToString();
            return result;
        }

        public MessagePackage Send()
        {
            new Task(async () => {
                while (MessageTypes.Count > 0)
                {
                    BubbleType type = MessageTypes.Dequeue();
                    object value = MessageValues.Dequeue();

                    AbstractMessage message = await BuildMessage(type, value);
                    Model.SendMessage(ConversationId, message);
                }
            }).Start();
            return new MessagePackage(Model, ConversationId, SenderId);
        }

    }
}
