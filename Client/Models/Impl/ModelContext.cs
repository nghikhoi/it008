using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UI.Models.Message;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Search;
using UI.Network.RestAPI;
using UI.Services;
using UI.Utils;
using UI.ViewModels;

namespace UI.Models.Impl {
    public class ModelContext : IModelContext
    {

        private readonly ChatConnection _connection;
        private readonly PacketRespondeListener _packetListener;
        private readonly IAppSession _appSession;
        private readonly IModelFactory _factory;

        private readonly Dictionary<Guid, AbstractConversation> ConversationsCacheMap = new Dictionary<Guid, AbstractConversation>();
        private readonly List<Guid> RecentConversations = new List<Guid>();
        private readonly List<UserShortInfo> Friends = new List<UserShortInfo>();

        public ModelContext(ChatConnection connection, PacketRespondeListener packetListener, IAppSession appSession, IModelFactory factory)
        {
            _connection = connection;
            _packetListener = packetListener;
            _appSession = appSession;
            _factory = factory;
            InitPacketListener();
        }

        private void InitPacketListener()
        {
            _packetListener.ReceiveMessageEvent += (session, responde) =>
            {
                AbstractMessage message = responde.Message;
                message.SenderID = responde.SenderID;
                GetConversation(Guid.Parse(responde.ConversationID)).AddMessage(message);
                if (message is MediaAbstractMessage media)
                    GetConversation(Guid.Parse(responde.ConversationID)).AddMedia(media);
            };
            _packetListener.BubbleChatColorEvent += (session, responde) =>
            {
                GetConversation(Guid.Parse(responde.ConversationID)).Color = ColorUtils.IntToColor(responde.BubbleColor);
            };
            _packetListener.UserOnlineEvent += (session, response) =>
            {
                Guid id = Guid.Parse(response.UserID);
                foreach (var conversation in ConversationsCacheMap.Values)
                {
                    conversation.UpdateStatus(id, true);
                }
            };
            _packetListener.UserOfflineEvent += (session, response) => {
                Guid id = Guid.Parse(response.UserID);
                foreach (var conversation in ConversationsCacheMap.Values) {
                    conversation.UpdateStatus(id, false);
                }
            };
        }

        public void UpdateColor(Guid ConversationId, Color color)
        {
            BubbleChatColorSetRequest request = new BubbleChatColorSetRequest()
            {
                ConversationID = ConversationId.ToString(),
                BubbleColor = ColorUtils.ColorToInt(color)
            };
            _connection.Send(request);
        }
        public void SendMessage(Guid ConversationId, AbstractMessage message)
        {
            if (message == null) return;

            SendMessage packet = new SendMessage();
            packet.Message = message;
            packet.ConversationID = ConversationId.ToString();
            _connection.Send(packet);

            GetConversation(ConversationId).AddMessage(message);
            if (message is MediaAbstractMessage media)
                GetConversation(ConversationId).AddMedia(media);
        }

        public AbstractConversation GetConversation(Guid id)
        {
            if (ConversationsCacheMap.ContainsKey(id))
            {
                return ConversationsCacheMap[id];
            }

            lock (ConversationsCacheMap)
            {
                if (ConversationsCacheMap.ContainsKey(id)) {
                    return ConversationsCacheMap[id];
                }
                GetConversationShortInfo packet = new GetConversationShortInfo();
                packet.ConversationID = id.ToString();
                DataAPI.getData<GetConversationShortInfoResult>(packet, infoResult =>
                {
                    AbstractConversation model = _factory.Create<AbstractConversation>();
                    model.ID = id;
                    model.Name = infoResult.ConversationName;
                    model.LastActive = infoResult.LastActive;
                    model.Nicknames = infoResult.Nicknames;
                    model.Members.Clear();
                    if (infoResult.OnlineUser != "~")
                        model.UpdateStatus(Guid.Parse(infoResult.OnlineUser), true);
                    foreach (var resultMember in infoResult.Members) {
                        model.Members.Add(Guid.Parse(resultMember));
                    }
                    ConversationsCacheMap[id] = model;
                });
            }

            return ConversationsCacheMap[id];
        }

        public void LoadConversation(Guid id)
         {
            AbstractConversation conversation = GetConversation(id);
            if (conversation.Loaded) return;
            ConversationFromID request = new ConversationFromID
            {
                ConversationID = id.ToString()
            };
            DataAPI.getData<ConversationFromIDResult>(request, result => {
                conversation.LastMessID = result.LastMessID;

                conversation.LastMediaID = result.LastMediaID;
                conversation.LastMediaIDBackup = result.LastMediaID;

                conversation.LastAttachmentID = result.LastAttachmentID;

                conversation.Color = ColorUtils.IntToColor(result.BubbleColor);
                Console.WriteLine(ColorUtils.ColorToInt(conversation.Color));
                LoadMessages(conversation, 20, true);
                LoadMedias(conversation, 20, true);
                LoadAttachments(conversation, 20, true);
                conversation.Loaded = true;
            });
        }

        private void LoadMessages(AbstractConversation conversation, int quantity = 10, bool FirstLoad = false)
        {
            if (conversation.LastMessID < 0)
                return;

            GetMessageFromConversation msgPacket = new GetMessageFromConversation();
            msgPacket.ConversationID = conversation.ID.ToString();
            msgPacket.MessagePosition = conversation.LastMessID;
            msgPacket.Quantity = quantity;
            msgPacket.LoadConversation = FirstLoad;
            conversation.LastMessID -= quantity;
            DataAPI.getData<GetMessageFromConversationResult>(msgPacket, result => {
                for (int i = 0; i < result.SenderID.Count; ++i) {
                    result.Content[i].SenderID = result.SenderID[i];
                    conversation.AddMessage(result.Content[i], true);
                }
            });
        }

        public void LoadMessages(Guid id, int quantity = 10)
        {
            LoadMessages(GetConversation(id), quantity);
        }

        private void LoadMedias(AbstractConversation conversation, int quantity = 5, bool FirstLoad = false)
        {
            if (conversation.LastMediaID < 0) return;
            GetMediaFromConversation msgPacket = new GetMediaFromConversation
            {
                ConversationID = conversation.ID.ToString(),
                MediaPosition = conversation.LastMediaID,
                Quantity = quantity
            };
            conversation.LastMediaID -= quantity;
            DataAPI.getData<GetMediaFromConversationResult>(msgPacket, result => {
                for (var i = 0; i < result.Positions.Count; i++) {
                    int position = result.Positions[i];
                    string fileName = result.FileNames[i];
                    string fileId = result.FileIDs[i];
                    MediaAbstractMessage message = null;
                    if (MediaInfo.IsVideoFileName(fileName)) {
                        message = new VideoMessage();
                    } else {
                        message = new ImageMessage();
                    }
                    message.FileName = fileName;
                    message.FileID = fileId;
                    conversation.AddMedia(message, true);
                }
            });
        }

        public void LoadMedias(Guid id, int quantity = 5) {
            LoadMedias(GetConversation(id), quantity);
        }

        private void LoadAttachments(AbstractConversation conversation, int quantity = 5, bool FirstLoad = false) {

        }

        public void LoadAttachments(Guid id, int quantity = 5) {

        }

        public AbstractConversation GetConversationFromUserId(Guid guid)
        {
            Guid conversationId = default;
            SingleConversationFrUserID packet = new SingleConversationFrUserID {
                UserID = guid.ToString()
            };
            DataAPI.getData<SingleConversationFrUserIDResult>(packet, result => {
                conversationId = Guid.Parse(result.ConversationID);
            });
            if (conversationId == default) {
                return null;
            }
            return GetConversation(conversationId);
        }

        public List<UserShortInfo> GetFriendList(bool reload = false)
        {
            if (reload)
            {
                GetFriendIDs request = new GetFriendIDs
                {
                    FriendOfID = _appSession.SessionID
                };
                Friends.Clear();
                DataAPI.getData<GetFriendIDsResult>(request, friendsResult => {
                    friendsResult.ids.ForEach(id => {
                        GetShortInfo packet = new GetShortInfo();
                        packet.ID = id;
                        DataAPI.getData<GetShortInfoResult>(packet, result => {
                            UserShortInfo info = result.info;
                            Friends.Add(info);
                        });
                    });
                });
            }
            return Friends;
        }

        public List<UserShortInfo> Search(string searchString)
        {
            List<UserShortInfo> infos = new List<UserShortInfo>();
            SearchUser packet = new SearchUser
            {
                Email = searchString
            };
            DataAPI.getData<SearchUserResult>(packet, result =>
            {
                infos = result.Results;
            });
            return infos;
        }

        public List<Guid> GetRecentConversations(bool reload = false)
        {
            if (reload)
            {
                RecentConversations.Clear();
                DataAPI.getData<RecentConversations, RecentConversationsResult>(recentResult =>
                {
                    foreach (var conversationsKey in recentResult.Conversations.Keys)
                    {
                        RecentConversations.Add(Guid.Parse(conversationsKey));
                    }
                });
            }

            return RecentConversations;
        }
    }
}
