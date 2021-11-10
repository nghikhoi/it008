using System;
using System.Windows.Media;
using UI.Models;
using UI.MVC;

namespace UI.MVC {
	
	public partial class ChatContainerController : IController {

		private ChatPage view;
		private ChatContainerModel model;

		public ChatContainerController(ChatPage view, ChatContainerModel model) {
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
		
		#endregion

		#region Profile

		#endregion

		#region Search

		#endregion

		#region Sticker
		
		#endregion
		
	}

}