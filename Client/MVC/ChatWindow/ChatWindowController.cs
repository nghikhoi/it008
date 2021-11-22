using System;
using System.Windows;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;
using UI.MVC;
using UI.Network.Packets.AfterLoginRequest;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.Packets.AfterLoginRequest.Profile;
using UI.Network.Packets.AfterLoginRequest.Sticker;
using UI.Network.RestAPI;
using UI.Utils;

namespace UI.MVC {
	
	public partial class ChatWindowController : IController {

		private HomeWindow view;
		private ChatContainer chatContainer;
		private ConversationList conversationList;
		private Notification notification;
		private SettingPage setting;

		public ChatWindowController(HomeWindow view) {
			this.view = view;
			initChatContainer();
			initConversationList();
			initSettingPage();
			initNotification();
		}

		#region Initialize

		private void initChatContainer() {
			ChatPage view = this.view.ChatPage;
			ChatContainerController controller = new ChatContainerController(view);
			ChatContainer module = new ChatContainer();
			module.InitializeMVC(ChatModel.Instance, view, controller);
			chatContainer = module;
		}

		private void initConversationList() {
			ucListRecentMessage view = this.view.MessageList;
			ConversationListController controller = new ConversationListController(view);
			ConversationList module = new ConversationList();
			module.InitializeMVC(ChatModel.Instance, view, controller);
			conversationList = module;
		}
		
		private void initSettingPage() {
			SettingWindow view = new SettingWindow();
			SettingPageController controller = new SettingPageController(view);
			SettingPage module = new SettingPage();
			module.InitializeMVC(ChatModel.Instance, view, controller);
			setting = module;
		}

		private void initNotification() {
			NotificationPage view = this.view.NotificaionPage;
			NotificationController controller = new NotificationController(view);
			Notification module = new Notification();
			module.InitializeMVC(ChatModel.Instance, view, controller);
			notification = module;
		}

		private void initData() {
			if (App.IS_LOCAL_DEBUG) return;
			initSelfId();
			while (ChatModel.Instance.SelfID == null) { }
			DataAPI.getData<GetSelfProfile, GetSelfProfileResult>();
			notification.controller.initNotifications();
			conversationList.controller.loadRecentConversation();
			conversationList.controller.loadFriends();
			DataAPI.getData<GetBoughtStickerPacksRequest, GetBoughtStickerPacksResponse>();
		}

		private void initSelfId() {
			DataAPI.getData<GetSelfID, GetSelfIDResult>(result => {
				ChatModel.Instance.SelfID = result.ID;
			});
		}

		#endregion

		private bool isFirstView = true;

		public void LogOut() {
			view.Hide();
			Authentication authentication = ModuleContainer.GetModule<Authentication>();
			authentication.controller.showLogin();
		}
		
		public void showView() {
			if (isFirstView) {
				initData();
				isFirstView = false;
			}
			view.Show();
			Application.Current.MainWindow = view;
		}

		#region Message

		public void Buzz() {
			//TODO Như tên gọi
		}

		public void AddMessage(string ConversationId, string SenderId, AbstractMessage message) {
			// TODO:
			// 1. Kiểm tra tin nhắn xem có phải tin nhắn mới nhất không để update vô ConversationList
			// 2. Cập nhật message vào chat model
			// 3. Kiểm tra xem message có thuộc về conversation đang mở hay ko để add vào panel
			
			ChatModel model = ChatModel.Instance;
			ConversationCache cache = model.computeIfAbsent(ConversationId, new ConversationCache());
			cache.Members.Add(SenderId);
			
			cache.Bubbles.Add(new BubbleInfo(message, true));

			model.PrivateConversations[SenderId] = ConversationId;

			ConversationListController conversationListController = ModuleContainer.GetModule<ConversationList>().controller; 

			if (model.currentSelectedConversation.CompareTo(ConversationId) == 0)
			{
				ChatContainerController chatContainerController = ModuleContainer.GetModule<ChatContainer>().controller;
				chatContainerController.AddMessage(message);

				if (!model.SelfID.Equals(SenderId)) {
					conversationListController.IncomingMessage(ConversationId, message, true);
				}
			}
			else if (!model.SelfID.Equals(SenderId))
				conversationListController.IncomingMessage(ConversationId, message, false);
		}

		public void LoadConversation(String userId, String conversationId) {
			// TODO:
			// Thêm data vô Chat Model
		}

		#endregion

		#region Notification

		public void UpdateOnlineStatus(String userId, bool status) {
			//TODO:
			// Cập nhật data vào ChatModel
			// Cập nhật trạng thái hiển thị trên ConversationList
			//update_online_status() de cap nhat trng thai onl
			//update_offline_status() de cap nhat trang thai offline
			//tu xu tiep nha ck 
		}

		public void AddFriendAcceptedNoti(String userId, String userName) {
			
		}

		public void AddFriendRequestNoti(String userId, String userName) {
			
		}

		public void NotifyModifyPasswordResult(bool result) {
			//TODO
			// Hiển thị thông báo cập nhật mật khẩu thành công
		}

		#endregion

	}

}