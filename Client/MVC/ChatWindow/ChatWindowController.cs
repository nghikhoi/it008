using System;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;
using UI.MVC;

namespace UI.MVC {
	
	public partial class ChatWindowController : IController {

		private HomeWindow view;

		public ChatWindowController(HomeWindow view) {
			this.view = view;
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