using System;
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
using UI.ViewModels.Messages;

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

		private ObservableCollection<MessageViewModel> _Messages;
		public ObservableCollection<MessageViewModel> Messages { get => _Messages; set => _Messages = value; }

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

		#endregion

		#region Command

		public event Action<ConversationViewModel> SelectAction;
		public ICommand SelectCommand { get; private set; }
		public ICommand SendTextMessageCommand { get; private set; }
		public ICommand LoadMoreCommand { get; private set; }
		public InitializeCommand FirstSelectCommand { get; private set; }

		#endregion

		private readonly IViewModelFactory _factory;
		private readonly ChatConnection _connection;
		private readonly IAppSession _appSession;
		
		public ConversationViewModel(ChatConnection chatConnection, IAppSession appSession, IViewModelFactory factory) {
			Messages = new ObservableCollection<MessageViewModel>();
			_factory = factory;
			_connection = chatConnection;
			_appSession = appSession;
			
			SelectCommand = new RelayCommand<object>(null, o => SelectAction?.Invoke(this));
			InitializeCommand.Execute(null);
			FirstSelectCommand = new InitializeCommand(FirstSelectLoad);
			SendTextMessageCommand = new RelayCommand<object>(null, o => SendTextMessage());
			LoadMoreCommand = new RelayCommand<object>(null, o => LoadMessages());
			SelectAction += (vm) => FirstSelectCommand.Execute(null);
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
			});
		}
		
		public void LoadMessages(bool loadConversation = false) {
			if (LastMessId < 0)
				return;
			int quantity = 10;
				
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

		protected void AddMessage(AbstractMessage message, bool toTop = false) {
			MessageViewModel messageViewModel = null;
			switch ((BubbleType) message.GetPreviewCode()) {
				case BubbleType.Attachment: {
					
					break;
				}
				case BubbleType.Image: {
					messageViewModel = _factory.Create<ImageMessageViewModel>();
					(messageViewModel as ImageMessageViewModel).ConversationId = ConversationId;
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
					(messageViewModel as VideoMessageViewModel).ConversationId = ConversationId;
					break;
				}
			}

			if (messageViewModel != null) {
				messageViewModel.Message = message;
				//Prevent from async add when receive message
				Application.Current.Dispatcher.Invoke(() => {
					if (toTop)
						Messages.Insert(0, messageViewModel);
					else
						Messages.Add(messageViewModel);
				});
			}
		}

		protected override void Initialize(object parameter = null) {
			
		}

	}
}