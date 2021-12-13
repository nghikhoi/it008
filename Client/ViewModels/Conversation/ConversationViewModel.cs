using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UI.Command;
using UI.Models;
using UI.Models.Message;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.RestAPI;
using UI.Services;
using UI.Utils;

namespace UI.ViewModels {
	public class ConversationViewModel : InitializableViewModel {

		#region Properties

		private string _conversationName;
		public string ConversationName {
			get => _conversationName;
			set {
				_conversationName = value;
				OnPropertyChanged(nameof(ConversationName));
			}
		}

		private string _conversationAvatar;
		public string ConversationAvatar {
			get => _conversationAvatar;
			set {
				_conversationAvatar = value;
				OnPropertyChanged("ConversationAvatar");
			}
		}

		private string _conversationId = "~";
		public string ConversationId {
			get => _conversationId;
			set {
				_conversationId = value;
				OnPropertyChanged(nameof(ConversationId));
			}
		}

		private int _lastMessId;
		public int LastMessId {
			get => _lastMessId;
			set {
				_lastMessId = value;
				OnPropertyChanged(nameof(LastMessId));
			}
		}

		private int _lastMediaId;
		public int LastMediaId {
			get => _lastMediaId;
			set {
				_lastMediaId = value;
				OnPropertyChanged(nameof(LastMediaId));
			}
		}

		private int _lastMediaIdBackup;
		public int LastMediaIdBackup {
			get => _lastMediaIdBackup;
			set {
				_lastMediaIdBackup = value;
				OnPropertyChanged(nameof(LastMediaIdBackup));
			}
		}

		private int _lastAttachmentId;
		public int LastAttachmentId {
			get => _lastAttachmentId;
			set {
				_lastAttachmentId = value;
				OnPropertyChanged(nameof(LastAttachmentId));
			}
		}

		private long _lastActive;
		public long LastActive {
			get => _lastActive;
			set {
				_lastActive = value;
				OnPropertyChanged("LastActive");
			}
		}

		private bool _isLoadMoreEnable;
		public bool IsLoadMoreEnable {
			get => _isLoadMoreEnable;
			set {
				_isLoadMoreEnable = value;
				OnPropertyChanged(nameof(IsLoadMoreEnable));
			}
		}

		private bool _isOnline=true;
		public bool IsOnline
        {
			get => _isOnline;
            set
            {
				_isOnline = value;
				OnPropertyChanged(nameof(IsOnline));
            }
        }

	
		public bool IsOffline
        {
			get => !_isOnline;
        }

		private string _conversationBackground = @"../../Resources/Images/avt.jpg";
		public string ConversationBackground
		{
			get => _conversationBackground;
			set
			{
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

		private static readonly int MAX_SHOW = 6;
		private ObservableCollection<MessageViewModel> _limitShowMedias;
		public ObservableCollection<MessageViewModel> LimitShowMedias {
			get => _limitShowMedias;
			set => _limitShowMedias = value;
		}

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

		#endregion

		#region Command

		public event Action<ConversationViewModel> SelectAction;
		public ICommand SelectCommand { get; private set; }
		public ICommand SelectMediaCommand { get; private set; }
		public ICommand SendTextMessageCommand { get; private set; }
		public ICommand LoadMoreCommand { get; private set; }
		public ICommand LoadMoreMediaCommand { get; private set; }
		public ICommand ChatpageSendImageCommand { get; private set; }
		public ICommand ChatpageSendFileCommand { get; private set; }
		public ICommand ChatpageSelectEmojiCommand { get; private set; }
		public InitializeCommand FirstSelectCommand { get; private set; }

		#endregion

		private readonly IViewModelFactory _factory;
		private readonly ChatConnection _connection;
		private readonly IAppSession _appSession;
		
		public ConversationViewModel(ChatConnection chatConnection, IAppSession appSession, IViewModelFactory factory) {
			Messages = new ObservableCollection<MessageViewModel>();
			Medias = new ObservableCollection<MessageViewModel>();
			LimitShowMedias = new ObservableCollection<MessageViewModel>();
			Medias.CollectionChanged += LimitShowUtils.CreateHandler(Medias, LimitShowMedias, 0, MAX_SHOW);
			
			_factory = factory;
			_connection = chatConnection;
			_appSession = appSession;
			
			SelectCommand = new RelayCommand<object>(null, o => SelectAction?.Invoke(this));
			SelectMediaCommand = new RelayCommand<MediaViewModel>(null, SelectMedia);
			InitializeCommand.Execute(null);
			FirstSelectCommand = new InitializeCommand(FirstSelectLoad);
			SendTextMessageCommand = new RelayCommand<object>(null, o => SendTextMessage());
			LoadMoreCommand = new RelayCommand<object>(null, o => LoadMessages());
			LoadMoreMediaCommand = new RelayCommand<object>(null, o => LoadMedias());
			SelectAction += (vm) => FirstSelectCommand.Execute(null);
			ChatpageSendImageCommand = new RelayCommand<object>(null, SelectImage);
			ChatpageSendFileCommand = new RelayCommand<object>(null, SelectVideo);
			ChatpageSelectEmojiCommand = new RelayCommand<object>(null, SelectEmoji);
		}

	
		private void SelectEmoji(object para)
        {
			Texting += para.ToString();
			
        }
		private void SelectImage(object para=null) {
			List<string> paths = new List<string>();
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				// Open document 
				string Selectedfilename = dlg.FileName;
				paths.Add(Selectedfilename);
				FileAPI.UploadMedia(this.ConversationId, paths, map => {
					foreach(string name in map.Keys)
                    {
						string id = map[name];
						ImageMessage img = new ImageMessage();
						img.FileName = name;
						img.FileID = id;
						App.Current.Dispatcher.Invoke(()=> {
							SendMessage(img);
						});
                    }
				}
				, error => { });
			}
		}
		private void SelectVideo(object para = null)
		{
			List<string> paths = new List<string>();
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.Filter = "Video files (*.mp4)|*.mp4";


			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				// Open document 
				string Selectedfilename = dlg.FileName;
				paths.Add(Selectedfilename);
				FileAPI.UploadMedia(this.ConversationId, paths, map => {
					foreach (string name in map.Keys)
					{
						string id = map[name];
						VideoMessage vid = new VideoMessage();
						vid.FileName = name;
						vid.FileID = id;
						App.Current.Dispatcher.Invoke(() => {
							SendMessage(vid);
						});
					}

				}
				, error => { });
			}
		}

		private void SendMessage(AbstractMessage msg)
        {
			msg.SenderID = _appSession.SessionID;
			AddMessage(msg);
			SendMessage packet = new SendMessage();
			packet.Message = msg;
			packet.ConversationID = ConversationId;
			_connection.Send(packet);

		}

		private void SelectMedia(MediaViewModel Parameter) {
			if (Parameter == null || !(Parameter is MediaViewModel))
				return;
			ShowingMedia = Parameter as MediaViewModel;
        }

		private void SendTextMessage() {
			TextMessage textMessage = new TextMessage();
			textMessage.Message = Texting;
			Texting = "";
			textMessage.SenderID = _appSession.SessionID;
			AddMessage(textMessage);

			SendMessage packet = new SendMessage();
			packet.Message = textMessage;
			packet.ConversationID = ConversationId;
			_connection.Send(packet);
		}
		
		public void ReceiveMessage(AbstractMessage message) {
			AddMessage(message);
			if (message is MediaAbstractMessage) {
				AddMedia(message);
			}
		}

		protected virtual void FirstSelectLoad(object parameter = null) {
			ConversationFromID request = new ConversationFromID();
			request.ConversationID = ConversationId;
			DataAPI.getData<ConversationFromIDResult>(request, result => {
				LastMessId = result.LastMessID;

				if (!FirstSelectCommand.IsExecuted()) {
					LastMediaId = result.LastMediaID;
					LastMediaIdBackup = result.LastMediaID;
				}

				LastAttachmentId = result.LastAttachmentID;
				//ConversationName = result.ConversationName;
				//conversation.Members = result.Members.ToList();

				//conversation.Color = ColorUtils.IntToColor(result.BubbleColor);
				//TODO update chat container color
				//
				// view.cleanChatPage();
				LoadMessages(true);
				LoadMedias(true, MAX_SHOW);
			});
		}

		#region Media

		public void LoadMedias(bool loadConversation = false, int quantity = 5) {
			if (LastMessId < 0)
				return;
			GetMediaFromConversation msgPacket = new GetMediaFromConversation();
			msgPacket.ConversationID = ConversationId;
			msgPacket.MediaPosition = LastMediaId;
			msgPacket.Quantity = quantity;
			LastMediaId -= quantity;
			DataAPI.getData<GetMediaFromConversationResult>(msgPacket, result => {
				for (var i = 0; i < result.Positions.Count; i++) {
					int position = result.Positions[i];
					string fileName = result.FileNames[i];
					string fileId = result.FileIDs[i];
					MediaAbstractMessage message = null;
					if (MediaInfo.IsVideoFileName(fileName)) {
						message = new VideoMessage();
					}
					else {
						message = new ImageMessage();
					}
					message.FileName = fileName;
					message.FileID = fileId;
					AddMedia(message, true);
				}
			});
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
			if (LastMessId < 0)
				return;
				
			GetMessageFromConversation msgPacket = new GetMessageFromConversation();
			msgPacket.ConversationID = ConversationId;
			msgPacket.MessagePosition = LastMessId;
			msgPacket.Quantity = quantity;
			msgPacket.LoadConversation = loadConversation;
			LastMessId -= quantity;
			DataAPI.getData<GetMessageFromConversationResult>(msgPacket, result => {
				for (int i = 0; i < result.SenderID.Count; ++i) {
					result.Content[i].SenderID = result.SenderID[i];
					AddMessage(result.Content[i], true);
				}
			});
		}

		protected void AddMessage(AbstractMessage message, bool loadFromServer = false) {
			MessageViewModel messageViewModel = null;
			switch ((BubbleType) message.GetPreviewCode()) {
				case BubbleType.Attachment: {
					
					break;
				}
				case BubbleType.Image: {
					messageViewModel = _factory.Create<ImageMessageViewModel>();
					break;
				}
				case BubbleType.Sticker: {
					
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
			}

			if (messageViewModel != null) {
				if (message is MediaAbstractMessage) {
					Console.WriteLine("Message: " + ((MediaAbstractMessage) message).FileID);
                }
				messageViewModel.ConversationId = ConversationId;
				messageViewModel.Message = message;
				//Prevent from async add when receive message
				Application.Current.Dispatcher.Invoke(() => {
					if (loadFromServer) {
						Messages.Insert(0, messageViewModel);
					} else {
						Messages.Add(messageViewModel);
					}
					if (messageViewModel is MediaViewModel) {
						Console.WriteLine("MessageVM: " + ((MediaViewModel) messageViewModel).MediaInfo.ThumbURL);
					}
				});
			}
		}

		#endregion

		protected override void Initialize(object parameter = null) {
			
		}

	}
}