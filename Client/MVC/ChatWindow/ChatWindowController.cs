using System;
using System.Windows.Media;
using UI.Models;
using UI.MVC;

namespace UI.MVC {
	
	public partial class ChatWindowController : IController {

		private HomeWindow view;
		private ChatWindowModel model;

		public ChatWindowController(HomeWindow view, ChatWindowModel model) {
			this.view = view;
			this.model = model;
		}

		#region Message

		public void Buzz() {
			//TODO
		}

		public void SetBubbleColor(Color color) {
			//TODO
		}

		public void AddConversation(Conversation conversation) {
			//TODO
		}
		
		public void AddShortInfoConversation(Conversation conversation) {
			//TODO
		}

		public void AddMessage(message message) {
			//TODO
		}

		public void LoadConversation(String userId, String conversationId) {
			
		}

		#endregion

		#region Notification

		public void UpdateOnlineStatus(String userId, bool status) {
			
		}

		public void AddFriendAcceptedNoti(String userId, String userName) {
			
		}

		public void AddFriendRequestNoti(String userId, String userName) {
			
		}

		#endregion

		#region Profile

		#endregion

		#region Search

		#endregion

		#region Sticker

		#endregion

	}

}