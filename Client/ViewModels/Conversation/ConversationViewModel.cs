using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using UI.Command;
using UI.Models;
using UI.Models.Message;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.RestAPI;
using UI.Services;
using UI.Utils;
using Application = System.Windows.Application;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace UI.ViewModels {
	public class ConversationViewModel : InitializableViewModel {

		#region Properties

        private AbstractConversation _conversation;
        public AbstractConversation Conversation
        {
            get => _conversation;
            set
            {
                _conversation = value;
				Messages.Clear();
                IsOnline = _conversation.IsOnline;
				_conversation.Messages.ForEach(msg => AddMessage(msg));
				Medias.Clear();
				_conversation.Medias.ForEach(msg => AddMedia(msg));
                _conversation.MediaAddEvent += (msg, loadFromServer) => App.Current.Dispatcher.Invoke(() => AddMedia(msg, loadFromServer));
                _conversation.MessageAddEvent += (msg, loadFromServer) => App.Current.Dispatcher.Invoke(() => AddMessage(msg, loadFromServer));
                messagePackage = new MessagePackage(_model, Conversation.ID, Guid.Parse(_appSession.SessionID));
                _conversation.PropertyChanged += (sender, args) =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        if (args.PropertyName == nameof(Conversation.Color))
                        {
                            UpdateColor(Color);
                        }

                        if (args.PropertyName == nameof(Conversation.IsOnline))
                        {
                            IsOnline = _conversation.IsOnline;
                        }

                        if (args.PropertyName == nameof(Conversation.Nicknames))
                        {
                            UpdateNicknames();
                        }
                    });
                };

                OnPropertyChanged(nameof(Conversation), nameof(ConversationName), nameof(ConversationAvatar), nameof(ConversationId));
            }
        }

        public Color Color => Conversation.Color;
		public Color SelectingColor { get; set; }

		private string _mainEmoji = "❤️";
		public string MainEmoji
        {
			get => _mainEmoji;
            set
            {
				_mainEmoji = value;
				OnPropertyChanged(nameof(MainEmoji));
            }
        }

        public bool IsGroup { get; set; } = false;

        public string ConversationId => Conversation.ID.ToString();
		
		public string ConversationName {
			get
            {
                if (Conversation == null) return null;
                if (Conversation.Members.Count > 2 || FastCodeUtils.NotEmptyStrings(Conversation.Name))
                    return Conversation.Name;
                return Conversation.Nicknames.First(pair => string.CompareOrdinal(pair.Key.ToString(), _appSession.SessionID) != 0).Value;
            }
		}
		
		public object ConversationAvatar {
            get
            {
                if (FastCodeUtils.NotEmptyStrings(Conversation.Avatar))
                    return new SingleAvatarViewModel()
                    {
                        AvatarId = Conversation.Avatar
                    };
                if (Conversation.Members.Count > 2)
                {
                    GroupAvatarViewModel avatar = new GroupAvatarViewModel();
                    Guid[] ids = Conversation.Members.Where(id => string.CompareOrdinal(id.ToString(), _appSession.SessionID) != 0)
                        .Take(2).ToArray();
                    avatar.UserOne = ids[0].ToString();
                    avatar.UserTwo = ids[1].ToString();
                    return avatar;
                }

                return new SingleAvatarViewModel()
                {
                    AvatarId = Conversation.Members
                        .First(id => string.CompareOrdinal(id.ToString(), _appSession.SessionID) != 0).ToString()
                };
            }
		}

        public long LastActive => Conversation.LastActive;

		private bool _isLoadMoreEnable;
		public bool IsLoadMoreEnable {
			get => _isLoadMoreEnable;
			set {
				_isLoadMoreEnable = value;
				OnPropertyChanged(nameof(IsLoadMoreEnable));
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

		private bool _isOnline = false;
		public bool IsOnline {
			get => _isOnline;
			set {
				_isOnline = value;
				OnPropertyChanged(nameof(IsOnline));
				OnPropertyChanged(nameof(IsOffline));
			}
		}

		public bool IsOffline {
			get => !_isOnline;
		}

		private string _conversationBackground = @"../../Resources/Images/avt.jpg";
		public string ConversationBackground {
			get => _conversationBackground;
			set {
				_conversationBackground = value;
			}
		}

		private ObservableCollection<MessageViewModel> _messages;
		public ObservableCollection<MessageViewModel> Messages { get => _messages; set => _messages = value; }
		private ObservableCollection<MessageViewModel> _medias;
		public ObservableCollection<MessageViewModel> Medias {
			get => _medias;
			set => _medias = value;
		}
		private ObservableCollection<GroupBubbleViewModel> _groupbubbles;
		public ObservableCollection<GroupBubbleViewModel> GroupBubbles { get => _groupbubbles; set => _groupbubbles = value; }

		private static readonly int MAX_SHOW = 6;
		private ObservableCollection<MessageViewModel> _limitShowMedias;
		public ObservableCollection<MessageViewModel> LimitShowMedias {
			get => _limitShowMedias;
			set => _limitShowMedias = value;
		}
		private ObservableCollection<MessageViewModel> _attachments;
		public ObservableCollection<MessageViewModel> Attachments {
			get => _attachments;
			set => _attachments = value;
		}
		private ObservableCollection<MessageViewModel> _limitAttachments;
		public ObservableCollection<MessageViewModel> LimitAttachments {
			get => _limitAttachments;
			set => _limitAttachments = value;
		}

        public ObservableCollection<NicknameViewModel> Nicknames { get; } =
            new ObservableCollection<NicknameViewModel>();

        private string _lastMessage;

        public string Subtitle
        {
            get
            {
                if (Messages.Count == 0) return "";
                AbstractMessage lastMessage = Messages[Messages.Count - 1].Message;
                if (lastMessage is TextMessage text)
                    return text.Message;
                if (lastMessage is MediaAbstractMessage)
                    return "Đã gửi một tệp media.";
                if (lastMessage is AttachmentMessage)
                    return "Đã gửi một tệp tin.";
                if (lastMessage is Sticker)
                    return "Đã gửi một sticker.";
                return "";
            }
        }

		public string LastMessage {
			get => _lastMessage;
			set {
				_lastMessage = value;
				OnPropertyChanged("LastMessage");
			}
		}

		private string _texting;
		public string Texting {
			get => _texting;
			set {
				_texting = value;
				OnPropertyChanged(nameof(Texting));
			}
		}

		private MediaViewModel _showingMedia;
		public MediaViewModel ShowingMedia {
			get => _showingMedia;
			set {
				_showingMedia = value;
				OnPropertyChanged(nameof(ShowingMedia));
			}
		}

        private MessagePackage messagePackage;

		#endregion

		#region Command

		public event Action<ConversationViewModel> SelectAction;
        public event Action<ConversationViewModel> SendMessageAction;
		public ICommand SelectCommand { get; private set; }
		public ICommand SelectMediaCommand { get; private set; }
		public ICommand SendTextMessageCommand { get; private set; }
		public ICommand EndLineTextCommand { get; private set; }
		public ICommand LoadMoreCommand { get; private set; }
		public ICommand LoadMoreMediaCommand { get; private set; }
		public ICommand ChatpageSendImageCommand { get; private set; }
		public ICommand ChatpageSendFileCommand { get; private set; }
		public ICommand SendFileCommand { get; private set; }
		public ICommand ChatpageSelectEmojiCommand { get; private set; }
        public ICommand UpdateColorCommand { get; private set; }
        public ICommand StartChangeNameCommand { get; private set; }
		public ICommand SendEmojiCommand { get; private set; }
		public ICommand DownloadCurrentItem { get; private set; }
		public InitializeCommand FirstSelectCommand { get; private set; }
		public ICommand BuzzCommand { get; private set; }

		#endregion

		protected readonly IViewModelFactory _factory;
		protected readonly ChatConnection _connection;
		protected readonly IAppSession _appSession;
        protected readonly IModelContext _model;

		public ConversationViewModel(ChatConnection chatConnection, IAppSession appSession, IViewModelFactory factory, IModelContext model) {
			Messages = new ObservableCollection<MessageViewModel>();
			Medias = new ObservableCollection<MessageViewModel>();
			LimitShowMedias = new ObservableCollection<MessageViewModel>();
			Medias.CollectionChanged += ObserveUtils.CreateLimitObserve(Medias, LimitShowMedias, 0, MAX_SHOW);
			Attachments = new ObservableCollection<MessageViewModel>();
			LimitAttachments = new ObservableCollection<MessageViewModel>();
			Attachments.CollectionChanged += ObserveUtils.CreateLimitObserve(Attachments, LimitAttachments, 0, MAX_SHOW);
			GroupBubbles = new ObservableCollection<GroupBubbleViewModel>();
			_factory = factory;
			_connection = chatConnection;
			_appSession = appSession;
            _model = model;

			SelectCommand = new RelayCommand<object>(null, o => SelectAction?.Invoke(this));
			SelectMediaCommand = new RelayCommand<MediaViewModel>(null, SelectMedia);
            FirstSelectCommand = new InitializeCommand(FirstSelectLoad);
			SendTextMessageCommand = new RelayCommand<object>(null, o => SendTextMessage());
			EndLineTextCommand = new RelayCommand<object>(null, o => Texting += (Environment.NewLine));
			LoadMoreCommand = new RelayCommand<object>(null, o => LoadMessages());
			LoadMoreMediaCommand = new RelayCommand<object>(null, o => LoadMedias());
			SelectAction += (vm) => FirstSelectCommand.Execute(null);
			ChatpageSendImageCommand = new RelayCommand<object>(null, SelectImage);
			ChatpageSendFileCommand = new RelayCommand<object>(null, SelectVideo);
			SendFileCommand = new RelayCommand<object>(null, SelectFile);
            DownloadCurrentItem = new RelayCommand<object>(null, o => DownloadShowingMedia());
			ChatpageSelectEmojiCommand = new RelayCommand<object>(null, SelectEmoji);
            UpdateColorCommand = new RelayCommand<object>(null, o => _model.UpdateColor(Conversation.ID, SelectingColor));
            StartChangeNameCommand = new RelayCommand<object>(null, o => StartChangeName());
			SendEmojiCommand = new RelayCommand<object>(null, SendEmoji);
            BuzzCommand = new RelayCommand<object>(null, o => SendBuzzPacket());
        }

        private void SendBuzzPacket()
        {
            BuzzSend request = new BuzzSend()
            {
                ConversationID = ConversationId
            };
			ChatConnection.Instance.Session.Send(request);
        }

        private void UpdateNicknames()
        {
            foreach (var bubble in GroupBubbles)
            {
                bubble.SenderName = Conversation.Nicknames[Guid.Parse(bubble.SenderID)];
			}
        }

        private void DownloadShowingMedia()
        {
            if (ShowingMedia == null) return;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "All Files (*.*)|*.*";
            dialog.CheckPathExists = true;
            dialog.FileName = ShowingMedia.MediaInfo.FileName;
            bool? result = dialog.ShowDialog();
            if (result == true) {
                string path = dialog.FileName;
                DownloadItemViewModel item = new DownloadItemViewModel() {
                    SavePath = path,
                    FileName = ShowingMedia.MediaInfo.FileName,
                    FileId = ShowingMedia.MediaInfo.FileID,
                    ConversationId = ConversationId
                };
                DownloadWindowCommand.StartDownload(item);
                /*FileAPI.DownloadAttachment(ConversationId, Message.FileID, path, (sender, args) => { },
                    (sender, args) => { }, error => { }, "message");*/
            }
        }

        private void StartChangeName()
        {
            Nicknames.Clear();
            foreach (var pair in Conversation.Nicknames)
            {
                NicknameViewModel vm = new NicknameViewModel()
                {
                    UserId = pair.Key.ToString(),
                    OriginNickname = pair.Value,
                    Nickname = pair.Value,
                    ConversationViewModel = this
                };
				Nicknames.Add(vm);
            }
        }


		private void UpdateColor(Color color)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var msg in Messages)
                {
					msg.BubbleColor = new SolidColorBrush(Color);
                }
            });
        }


		private void SelectEmoji(object para) {
			Texting += para.ToString();

		}

        private void PackageMessage()
        {
            messagePackage = messagePackage.Send();
            SendMessageAction?.Invoke(this);
        }

		private void SelectImage(object para = null) {
			List<string> paths = new List<string>();
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true) {
				// Open document 
				string Selectedfilename = dlg.FileName;
                messagePackage.AddImage(Selectedfilename);
				PackageMessage();
            }
		}
		private void SelectVideo(object para = null) {
			List<string> paths = new List<string>();
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.Filter = "Video files (*.mp4)|*.mp4|Sounds files|*.mp3";


			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true) {
				// Open document 
				string Selectedfilename = dlg.FileName;
                messagePackage.AddVideo(Selectedfilename);
                PackageMessage();
			}
		}

		private void SelectFile(object para = null) {
			List<string> paths = new List<string>();
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.Filter = "All Files (*.*)|*.*";


			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true) {
				// Open document 
				string Selectedfilename = dlg.FileName;
                messagePackage.AddAttachment(Selectedfilename);
                PackageMessage();
			}
		}

		protected void AddAttachment(AttachmentMessage message, bool loadFromServer = false) {
			MessageViewModel messageViewModel = _factory.Create<AttachmentMessageViewModel>();

			if (messageViewModel != null) {
				messageViewModel.ConversationId = ConversationId;
				messageViewModel.Message = message;
				//Prevent from async add when receive message
				Application.Current.Dispatcher.Invoke(() => {
					if (loadFromServer) {
						Attachments.Add(messageViewModel);
					} else {
						Attachments.Insert(0, messageViewModel);
					}
					if (messageViewModel is MediaViewModel) {
						Console.WriteLine("MediaVM: " + ((MediaViewModel) messageViewModel).MediaInfo.ThumbURL);
					}
				});
			}
		}

		private void SelectMedia(MediaViewModel Parameter) {
			if (Parameter == null || !(Parameter is MediaViewModel))
				return;
			ShowingMedia = Parameter as MediaViewModel;
		}

		public void SendStickerMessage(Sticker sticker) {
            messagePackage.AddSticker(sticker.ID);
            PackageMessage();
		}

		private void SendTextMessage()
        {
            if (!FastCodeUtils.NotEmptyStrings(Texting))
                return;
            messagePackage.AddTextMessage(Texting);
            PackageMessage();
            Texting = "";
        }

		private void SendEmoji(object o = null)
		{
			if (!FastCodeUtils.NotEmptyStrings(MainEmoji))
				return;
			messagePackage.AddTextMessage(MainEmoji);
			PackageMessage();
		}

		protected virtual void FirstSelectLoad(object parameter = null) {
			_model.LoadConversation(Conversation.ID);
		}

		#region Media

		public void LoadMedias(int quantity = 5) {
			_model.LoadMedias(Conversation.ID, quantity);
		}

		protected void AddMedia(AbstractMessage message, bool loadFromServer = false) {
			MessageViewModel messageViewModel = null;
			switch ((BubbleType) message.GetPreviewCode()) {
				case BubbleType.Image: {
						messageViewModel = _factory.Create<ImageMessageViewModel>();
						break;
					}
				case BubbleType.Video: {
						messageViewModel = _factory.Create<VideoMessageViewModel>();
						break;
					}
			}

			if (messageViewModel != null) {
				if (message is MediaAbstractMessage) {
					Console.WriteLine("Media: " + ((MediaAbstractMessage) message).FileID);
				}
				messageViewModel.ConversationId = ConversationId;
				messageViewModel.Message = message;
				//Prevent from async add when receive message
				Application.Current.Dispatcher.Invoke(() => {
					if (loadFromServer) {
						Medias.Add(messageViewModel);
					} else {
						Medias.Insert(0, messageViewModel);
					}
					if (messageViewModel is MediaViewModel) {
						Console.WriteLine("MediaVM: " + ((MediaViewModel) messageViewModel).MediaInfo.ThumbURL);
					}
				});
			}
		}

		#endregion

		#region Message

		public void LoadMessages(bool loadConversation = false, int quantity = 10) {
			_model.LoadMessages(Conversation.ID, quantity);
		}

		protected void AddMessage(AbstractMessage message, bool loadFromServer = false) {
			MessageViewModel messageViewModel = null;
			switch ((BubbleType) message.GetPreviewCode()) {
                case BubbleType.Attachment: {
                    messageViewModel = _factory.Create<AttachmentMessageViewModel>();
                    break;
                }
                case BubbleType.Image: {
                    messageViewModel = _factory.Create<ImageMessageViewModel>();
                    break;
                }
                case BubbleType.Sticker: {
                    messageViewModel = _factory.Create<StickerMessageViewModel>();
                    StickerMessage tmp = message as StickerMessage;
                    tmp.Sticker = Sticker.LoadedStickers[tmp.Sticker.ID];
                    break;
                }
                case BubbleType.Text: {
                    messageViewModel = _factory.Create<TextMessageViewModel>();
                    break;
                }
                case BubbleType.Video: {
                    messageViewModel = _factory.Create<VideoMessageViewModel>();
                    break;
                }
                case BubbleType.Announcement: {
                    messageViewModel = _factory.Create<AnnouncementViewModel>();
                    break;
                }
			}

			if (messageViewModel != null) {
				if (message is MediaAbstractMessage) {
					Console.WriteLine("Message: " + ((MediaAbstractMessage) message).FileID);
				}
				if (message is AttachmentMessage attachment) {
					AddAttachment(attachment, loadFromServer);
				}
				messageViewModel.ConversationId = ConversationId;
				messageViewModel.Message = message;
                messageViewModel.BubbleColor = new SolidColorBrush(Color);
                if (message is AnnouncementMessage)
                {
                    ((AnnouncementViewModel) messageViewModel).InitText(this);
				}
				//Prevent from async add when receive message
				Application.Current.Dispatcher.Invoke(() => {
					GroupBubbleViewModel groupBubbleView = null;
					if (loadFromServer) {
						Messages.Insert(0, messageViewModel);
						if (Messages.Count == 1)
							OnPropertyChanged(nameof(Subtitle));
						groupBubbleView = GroupBubbles.ElementAtOrDefault(0);
					} else {
						Messages.Add(messageViewModel);
						OnPropertyChanged(nameof(Subtitle));
						groupBubbleView = GroupBubbles.ElementAtOrDefault(GroupBubbles.Count - 1);
                    }

					bool createNew = false;
					if (groupBubbleView == null || message is AnnouncementMessage || string.CompareOrdinal(groupBubbleView.SenderID, message.SenderID) != 0) {
						groupBubbleView = new GroupBubbleViewModel();
                        groupBubbleView.IsReceive = messageViewModel.IsReceivedMessage;
						groupBubbleView.SenderID = messageViewModel.Message.SenderID;
                        groupBubbleView.SenderName = Conversation.Nicknames[Guid.Parse(groupBubbleView.SenderID)];
                        if (message is AnnouncementMessage)
                        {
                            groupBubbleView.IsAnnouce = true;
                        }

                        createNew = true;
					}

                    if (loadFromServer)
                    {
                        groupBubbleView.Messages.Insert(0, messageViewModel);
					}
                    else
                    {
                        groupBubbleView.Messages.Add(messageViewModel);
					}
					if (createNew) {
						if (loadFromServer)
							GroupBubbles.Insert(0, groupBubbleView);
						else
							GroupBubbles.Add(groupBubbleView);
					}
				});
			}
		}

		#endregion

		protected override void Initialize(object parameter = null) {

		}

	}
}