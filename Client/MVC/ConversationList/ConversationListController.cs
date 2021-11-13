using System;
using System.Collections.Generic;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;
using UI.MVC;
using UI.Network.Packets.AfterLoginRequest.Search;

namespace UI.MVC {
	
	public partial class ConversationListController : IController {

		private ChatListControl view;

		public ConversationListController(ChatListControl view) {
			this.view = view;
		}

		#region Message

		public void AddConversation(AbstractConversation conversation) {
			//TODO
			// 1. Cập nhật data vào ChatModel
			// 2. Hiển thị lên danh sách
		}
		
		public void AddShortInfoConversation(string ConversationId, string ConversationName, long lastActive) {
			//TODO
			// 1. Cập nhật data vào ChatModel
			// 2. Hiển thị lên danh sách
		}

		#endregion

		#region Search

		public void ShowSearchResult(List<SearchResult> list) {
			//TODO hiển thị danh sách lên giao diện
		}

		#endregion

	}

}