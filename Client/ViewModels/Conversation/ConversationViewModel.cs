using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
using UI.ViewModels.Conversation;

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
                    if (args.PropertyName == nameof(Conversation.Color))
                    {
						UpdateColor(Color);
                    }

                    if (args.PropertyName == nameof(Conversation.IsOnline))
                    {
                        IsOnline = _conversation.IsOnline;
                    }
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
		public InitializeCommand FirstSelectCommand { get; private set; }

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
			ChatpageSelectEmojiCommand = new RelayCommand<object>(null, SelectEmoji);
            UpdateColorCommand = new RelayCommand<object>(null, o => _model.UpdateColor(Conversation.ID, SelectingColor));
            StartChangeNameCommand = new RelayCommand<object>(null, o => StartChangeName());
			SendEmojiCommand = new RelayCommand<object>(null, SendEmoji);
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
					if (msg is TextMessageViewModel)
					{
						if ((msg as TextMessageViewModel).Message.isEmoji)
						{
							msg.BubbleColor = null;
							continue;
						}
					}
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

			dlg.Filter = "Video files (*.mp4)|*.mp4";


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
			messagePackage.AddEmoji(MainEmoji);
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
				case BubbleType.Emoji:
					{
						messageViewModel = _factory.Create<TextMessageViewModel>();
						(messageViewModel as TextMessageViewModel).Message.isEmoji = true;
						break;
					}
				case BubbleType.Video: {
						messageViewModel = _factory.Create<VideoMessageViewModel>();
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
				//Prevent from async add when receive message
				Application.Current.Dispatcher.Invoke(() => {
					GroupBubbleViewModel groupBubbleView = null;
					if (loadFromServer) {
						Messages.Insert(0, messageViewModel);
						groupBubbleView = GroupBubbles.ElementAtOrDefault(0);
					} else {
						Messages.Add(messageViewModel);
						groupBubbleView = GroupBubbles.ElementAtOrDefault(GroupBubbles.Count - 1);
                    }

					bool createNew = false;
					if (groupBubbleView == null || string.CompareOrdinal(groupBubbleView.SenderID, message.SenderID) != 0) {
						groupBubbleView = new GroupBubbleViewModel();
						groupBubbleView.IsReceive = messageViewModel.IsReceivedMessage;
						groupBubbleView.SenderID = messageViewModel.Message.SenderID;
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