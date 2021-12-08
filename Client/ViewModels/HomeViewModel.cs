
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CNetwork.Sessions;
using UI.Models;
using UI.Models.Message;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Sticker;
using UI.Network.RestAPI;
using UI.Services;
using UI.ViewModels.Notifications;
using UI.Command;

namespace UI.ViewModels {
    public class HomeViewModel : InitializableViewModel {

        #region Properties

        private ObservableCollection<ConversationViewModel> _conversations;
        public ObservableCollection<ConversationViewModel> Conversations { get => _conversations; set => _conversations = value; }
        private string _searchingString;
        public string SearchingString {
            get => _searchingString;
            set {
                _searchingString = value;
                OnPropertyChanged(nameof(SearchingString));
            }
        }
        private ObservableCollection<FriendConversationViewModel> _searchingConversations;
        public ObservableCollection<FriendConversationViewModel> SearchingFriendList { get => _searchingConversations; set => _searchingConversations = value; }
        private ObservableCollection<FriendConversationViewModel> _friendList;
        public ObservableCollection<FriendConversationViewModel> FriendList { get => _friendList; set => _friendList = value; }
        private ObservableCollection<FriendConversationViewModel> _originFriendList;
        public ObservableCollection<FriendConversationViewModel> OriginFriendList { get => _originFriendList; set => _originFriendList = value; }
        
        private ObservableCollection<NotificationViewModel> _notifications;
        public ObservableCollection<NotificationViewModel> Notifications { get => _notifications; set => _notifications = value; }

        private ConversationViewModel _selectedConversation;
        public ConversationViewModel SelectedConversation {
            get => _selectedConversation;
            set {
                _selectedConversation = value;
                OnPropertyChanged(nameof(SelectedConversation));
            }
        }

        private ProfileViewModel _profile;
        public ProfileViewModel Profile {
            get => _profile;
            set {
                _profile = value;
                OnPropertyChanged(nameof(Profile));
            }
        }

        private NotificationPageViewModel _notificationPage;
        public NotificationPageViewModel NotificationPage {
            get => _notificationPage;
            set {
                _notificationPage = value;
                OnPropertyChanged(nameof(NotificationPage));
            }
        }

        private readonly IViewModelFactory _viewModelFactory;
        private readonly IAuthenticator authenticator;
        private readonly IUserProfileHolder _userProfileHolder;
        private readonly PacketRespondeListener _respondeListener;

        #endregion

        #region Command

        public ICommand SearchCommand;
        public ICommand ProfileInitalizeCommand { get; private set; }

        #endregion

        public HomeViewModel(PacketRespondeListener respondeListener, IViewModelFactory viewModelFactory, IAuthenticator authenticator, IUserProfileHolder userProfileHolder) : base() {
            _conversations = new ObservableCollection<ConversationViewModel>();
            _searchingConversations = new ObservableCollection<FriendConversationViewModel>();
            _friendList = new ObservableCollection<FriendConversationViewModel>();
            _originFriendList = new ObservableCollection<FriendConversationViewModel>();
            _notifications = new ObservableCollection<NotificationViewModel>();
            
            _respondeListener = respondeListener;
            _respondeListener.ReceiveMessageEvent += ReceiveMessage;
            this.authenticator = authenticator;
            _userProfileHolder = userProfileHolder;
            this._viewModelFactory = viewModelFactory;

            InitializeCommand.Execute(null);
            ProfileInitalizeCommand = new InitializeCommand(o => { });
        }

        public override void Dispose() {
            _respondeListener.ReceiveMessageEvent -= ReceiveMessage;
            base.Dispose();
        }

        private ConversationViewModel ListSearchFor(string conversationId) {
            return Conversations.Where(vm => string.CompareOrdinal(vm.ConversationId, conversationId) == 0).First();
        }

        protected void ReceiveMessage(ISession session, ReceiveMessage receiveMessage) {
            ReceiveMessage(receiveMessage.ConversationID, receiveMessage.Message);
        }

        public void ReceiveMessage(string conversationId, AbstractMessage message) {
            ConversationViewModel conversation = ListSearchFor(conversationId);
            if (conversation == null)
                return;
            conversation.ReceiveMessage(message);
        }

        protected override void Initialize(object parameter = null) {
            InitData();
            Profile = _viewModelFactory.Create<ProfileViewModel>();
        }

        private void InitData() {
            if (App.IS_LOCAL_DEBUG)
                return;
            initSelfId();
            /*Task.WaitAll(DataAPI.getData<GetSelfProfile, GetSelfProfileResult>(),
                DataAPI.getData<GetBoughtStickerPacksRequest, GetBoughtStickerPacksResponse>(),
                loadRecentConversation(),
                loadFriends());*/
            _userProfileHolder.LoadProfile();
            DataAPI.getData<GetBoughtStickerPacksRequest, GetBoughtStickerPacksResponse>();
            LoadRecentConversation();
            LoadFriends();
        }

        private void LoadRecentConversation() { 
             DataAPI.getData<RecentConversations, RecentConversationsResult>(recentResult => {
                Conversations.Clear();
                recentResult.Conversations.Keys.ToList().ForEach(key => {
                    GetConversationShortInfo packet = new GetConversationShortInfo();
                    packet.ConversationID = key;
                    DataAPI.getData<GetConversationShortInfoResult>(packet, infoResult => {
                        ConversationViewModel viewModel = _viewModelFactory.Create<ConversationViewModel>();
                        viewModel.ConversationId = infoResult.ConversationID;
                        viewModel.ConversationName = infoResult.ConversationName;
                        viewModel.LastActive = infoResult.LastActive;
                        viewModel.SelectAction += SelectConversation;
                        Conversations.Add(viewModel);
                    });
                });
             });
        }

        private void LoadFriends() {
            GetFriendIDs request = new GetFriendIDs();
            request.FriendOfID = authenticator.CurrentSession.SessionID;
            DataAPI.getData<GetFriendIDsResult>(request, friendsResult => {
                OriginFriendList.Clear();
                friendsResult.ids.ForEach(id => {
                    GetShortInfo packet = new GetShortInfo();
                    packet.ID = id;
                    DataAPI.getData<GetShortInfoResult>(packet, result => {
                        UserShortInfo info = result.info;
                        FriendConversationViewModel viewModel = _viewModelFactory.Create<FriendConversationViewModel>();
                        viewModel.ConversationId = info.ConversationID;
                        viewModel.ConversationName = info.FirstName + " " + info.LastName;
                        viewModel.LastActive = info.LastActive;
                        viewModel.UserId = info.ID;
                        viewModel.Relationship = Relationship.Friend;
                        viewModel.SelectAction += SelectConversation;
                        OriginFriendList.Add(viewModel);
                    });
                });
            });
        }

        private void SelectConversation(ConversationViewModel viewModel) {
            this.SelectedConversation = viewModel;
        }
        
        private void initSelfId() {
            DataAPI.getData<GetSelfID, GetSelfIDResult>(result => {
                authenticator.CurrentSession.SessionID = result.ID;
            });
        }
        
        #region Search
        public void SearchRecentConversation(string s) {
            /*List<UserShortInfo> searchlist = new List<UserShortInfo>();
            Dictionary<string, ConversationCache> conversations = ChatModel.Instance.Conversations;
            foreach (var con in conversations)
            {
                if (s.Contains(con.Value.ConversationName))
                {
                    UserShortInfo result = new UserShortInfo();
                    result.ConversationID = con.Key;
                    result.FirstName = con.Value.ConversationName;
                    result.LastActive = con.Value.LastActiveTime;
                    searchlist.Add(result);
                }
            }
            //TODO show recent*/
        }
        public void SearchAction(string s)
        {
            /*SearchUser packet = new SearchUser();
            packet.Email = s;
            DataAPI.getData<SearchUserResult>(packet, result => {
                ShowSearchResult(result.Results);
            });*/
        }

        public void ShowSearchResult(List<UserShortInfo> list) {
            /*view.clear_friend_list();
            foreach (var item in list)
            {
                addShortInfo(item);
            }*/
        }

        #endregion

    }
}