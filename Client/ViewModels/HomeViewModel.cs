using CNetwork.Sessions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using UI.Command;
using UI.Models;
using UI.Models.Message;
using UI.Models.Notification;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.Packets.AfterLoginRequest.Search;
using UI.Network.Packets.AfterLoginRequest.Sticker;
using UI.Network.RestAPI;
using UI.Services;
using UI.Services.Common;
using UI.Utils;
using UI.ViewModels.Search;

namespace UI.ViewModels {
    public class HomeViewModel : InitializableViewModel {

        #region Properties

        public string UserID
        {
            get => _appSession.SessionID;
            set
            {
                OnPropertyChanged(nameof(UserID));
            }
        }

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

        private ProfileViewModel _userProfile;
        public ProfileViewModel UserProfile {
            get => _userProfile;
            set {
                _userProfile = value;
                OnPropertyChanged(nameof(UserProfile));
            }
        }

        private int _notificationCount;
        public int NotificationCount
        {
            get => _notificationCount;
            set
            {
                _notificationCount = value;
                OnPropertyChanged(nameof(NotificationCount));
                OnPropertyChanged(nameof(DisplayBadget));
            }
        }

        public bool DisplayBadget => NotificationCount > 0;

        private NotificationPageViewModel _notificationPage;
        public NotificationPageViewModel NotificationPage {
            get => _notificationPage;
            set {
                _notificationPage = value;
                OnPropertyChanged(nameof(NotificationPage));
            }
        }

        private string _themeBackground = @"../../Resources/Images/avt.jpg";
        public string themeBackground
        {
            get => _themeBackground;
            set
            {
                _themeBackground = value;
            }
        }

        private StickerContainerViewModel _stickerContainer;
        public StickerContainerViewModel StickerContainer {
            get => _stickerContainer;
            set {
                _stickerContainer = value;
                OnPropertyChanged(nameof(StickerContainer));
            }
        }

        private SnackbarMessageQueue _snackbarQueue;
        public SnackbarMessageQueue SnackbarQueue
        {
            get => _snackbarQueue;
            set
            {
                _snackbarQueue = value;
                OnPropertyChanged(nameof(SnackbarQueue));
            }
        }

        private HomeDialogState dialogState;
        public ViewModelBase DialogViewModel {
            get => dialogState.CurrentViewModel;
            set {
                dialogState.CurrentViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }

        private readonly IViewModelFactory _viewModelFactory;
        private readonly IAuthenticator authenticator;
        private readonly IUserProfileHolder _userProfileHolder;
        private readonly PacketRespondeListener _respondeListener;
        private readonly IAppSession _appSession;
        private readonly IModelContext _model;

        #endregion

        #region Command

        public ICommand SearchCommand { get; private set; }
        public ICommand ProfileInitalizeCommand { get; private set; }
        public ICommand OpenCreateGroupDialog { get; private set; }

        #endregion

        public HomeViewModel(PacketRespondeListener respondeListener, IAppSession appSession, IModelContext model, IViewModelFactory viewModelFactory
            , IAuthenticator authenticator, IUserProfileHolder userProfileHolder, HomeDialogState dialogState) : base() {
            _conversations = new ObservableCollection<ConversationViewModel>();
            _searchingConversations = new ObservableCollection<FriendConversationViewModel>();
            _friendList = new ObservableCollection<FriendConversationViewModel>();
            _originFriendList = new ObservableCollection<FriendConversationViewModel>();
            _notifications = new ObservableCollection<NotificationViewModel>();
            _appSession = appSession;
            this.dialogState = dialogState;
            this.dialogState.StateChanged += () => OnPropertyChanged(nameof(DialogViewModel));

            SnackbarQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            _respondeListener = respondeListener;
            _respondeListener.ReceiveNotificationEvent += ReceiveNotification;
            _respondeListener.FinalizeAcceptedFriendEvent += session => App.Current.Dispatcher.Invoke(() =>
            {
                SendSnackbar("Chấp nhận lời mời kết bạn thành công!");
                LoadFriends();
            }); //Update friend list
            _respondeListener.ReceiveMessageEvent += (session, responde) =>
                App.Current.Dispatcher.Invoke(() => ReceiveMessage(responde));
            this.authenticator = authenticator;
            _userProfileHolder = userProfileHolder;
            this._viewModelFactory = viewModelFactory;
            this._model = model;
            this._model.NewConversationEvent += guid => App.Current.Dispatcher.Invoke(() => NewConversationAdded(guid));

            InitializeCommand.Execute(null);
            SearchCommand = new RelayCommand<object>(null, o => SearchAction(SearchingString));
            //ProfileInitalizeCommand = new InitializeCommand(o => UserProfile = _viewModelFactory.Create<ProfileViewModel>());
            UserProfile = _viewModelFactory.Create<ProfileViewModel>();
            NotificationPage = _viewModelFactory.Create<NotificationPageViewModel>();
            StickerContainer = _viewModelFactory.Create<StickerContainerViewModel>();
            StickerContainer.OnStickerClick += s =>
            {
                if (SelectedConversation != null)
                    SelectedConversation.SendStickerMessage(s);
            };
            OpenCreateGroupDialog = new RelayCommand<object>(null, o =>
            {
                DialogViewModel = _viewModelFactory.Create<SearchViewModel>();
                (DialogViewModel as SearchViewModel).AcceptEvent += CreateGroup;
            });
        }

        public override void Dispose() {
            base.Dispose();
        }

        private void CreateGroup(List<UserShortInfo> infos)
        {
            GroupConversationCreateRequest request = new GroupConversationCreateRequest()
            {
                Members = infos.Select(info => Guid.Parse(info.ID)).ToList()
            };
            request.Members.Add(Guid.Parse(_appSession.SessionID));
            
            ChatConnection.Instance.Session.Send(request);
        }

        private ConversationViewModel ListSearchFor(string conversationId) {
            return Conversations.First(vm => string.CompareOrdinal(vm.ConversationId, conversationId) == 0);
        }

        private void MoveToTop(string conversationId)
        {
            for (var i = 0; i < Conversations.Count; i++) {
                ConversationViewModel conversation = Conversations[i];
                if (string.CompareOrdinal(conversationId, conversation.ConversationId) == 0) {
                    Conversations.RemoveAt(i);
                    Conversations.Insert(0, conversation);
                    return;
                }
            }
        }

        public void ReceiveMessage(ReceiveMessage msg)
        {
            MoveToTop(msg.ConversationID);
        }


        public void SendSnackbar(params string[] msgs)
        {
            SnackbarQueue.Enqueue(string.Join("\n", msgs));
        }

        protected void ReceiveNotification(ISession session, GetNotificationsResult result)
        {
            Queue<string[]> snackbars = new Queue<string[]>(); 

            bool shouldRefreshFriends = result.Notifications.Any(noti => noti is AcceptFriendNotification);
            foreach (AbstractNotification n in result.Notifications) {
                if (n is AcceptFriendNotification accept)
                {
                    shouldRefreshFriends = true;
                    snackbars.Enqueue(new string[] { $"Bạn và {accept.SenderName} đã trở thành bạn bè!" });
                }
            }
            if (shouldRefreshFriends)
                App.Current.Dispatcher.Invoke(() => {
                    while (snackbars.Count > 0)
                        SendSnackbar(snackbars.Dequeue());
                    LoadFriends();
                });
        }

        protected override void Initialize(object parameter = null) {
            InitData();
        }

        private void InitData() {
            if (App.IS_LOCAL_DEBUG)
                return;
            initSelfId();
            _userProfileHolder.LoadProfile();
            DataAPI.getData<GetBoughtStickerPacksRequest, GetBoughtStickerPacksResponse>();
            LoadRecentConversation();
            LoadFriends();
        }

        private void LoadRecentConversation()
        {
            _model.GetRecentConversations(true);
        }

        private void NewConversationAdded(Guid id)
        {
            AbstractConversation conversation = _model.GetConversation(id);
            ConversationViewModel viewModel = conversation is GroupConversation ? _viewModelFactory.Create<GroupConversationViewModel>()
                : _viewModelFactory.Create<ConversationViewModel>();
            viewModel.Conversation = conversation;
            viewModel.SelectAction += SelectConversation;
            viewModel.SendMessageAction += vm => MoveToTop(vm.ConversationId);
            Conversations.Add(viewModel);
        }

        private void LoadFriends() {
            _model.GetFriendList(true).ForEach(info =>
            {
                FriendConversationViewModel viewModel = _viewModelFactory.Create<FriendConversationViewModel>();
                viewModel.FullName = info.FirstName + " " + info.LastName;
                viewModel.UserId = info.ID;
                viewModel.Relationship = info.Relationship == 2 ? Relationship.Friend : Relationship.None;
                viewModel.SelectAction += SelectConversation;
                OriginFriendList.Add(viewModel);
            });
            SearchAction();
        }

        private void SelectConversation(ConversationViewModel viewModel) {
            this.SelectedConversation = viewModel;
            if (viewModel != null)
                this.SelectedConversation.StickerContainer = StickerContainer;
        }
        
        private void initSelfId() {
            DataAPI.getData<GetSelfID, GetSelfIDResult>(result => {
                authenticator.CurrentSession.SessionID = result.ID;
            });
        }
        
        #region Search
        public void SearchRecentConversation(string s) {
            List<UserShortInfo> searchlist = new List<UserShortInfo>();
            foreach (var con in Conversations) {
                if (s.Contains(con.ConversationName)) {
                    UserShortInfo result = new UserShortInfo();
                    result.ConversationID = con.ConversationId;
                    result.FirstName = con.ConversationName;
                    result.LastActive = con.LastActive;
                    searchlist.Add(result);
                }
            }
            //TODO show recent
        }
        public void SearchAction(string s = null)
        {
            if (!FastCodeUtils.NotEmptyStrings(s))
            {
                SearchingFriendList.Clear();
                foreach (var model in OriginFriendList)
                {
                    SearchingFriendList.Add(model);
                }

                return;
            }
            ShowSearchResult(_model.Search(s));
        }

        public void ShowSearchResult(List<UserShortInfo> list) {
            SearchingFriendList.Clear();
            foreach (var info in list) {
                FriendConversationViewModel viewModel = _viewModelFactory.Create<FriendConversationViewModel>();
                viewModel.FullName = info.FirstName + " " + info.LastName;
                viewModel.UserId = info.ID;
                viewModel.Relationship = info.Relationship == 2 ? Relationship.Friend : Relationship.None;
                viewModel.SelectAction += SelectConversation;
                SearchingFriendList.Add(viewModel);
            }
        }

        #endregion

    }
}