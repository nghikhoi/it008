using System;
using System.Collections.Generic;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;
using UI.MVC;

namespace UI.MVC {
	
	public partial class ChatContainerController : IController {

		private ChatPage view;

		public ChatContainerController(ChatPage view) {
			this.view = view;
		}

		#region Message

		public void SetBubbleColor(Color color) {
			//TODO Đổi màu bubble chat
		}

		public void AddMediaToCurrentSelectedConversation(List<string> FileIDs, List<string> FileNames, List<int> Positions) {
			//TODO
			// Cập nhật các media vô trình media hiện tại
		}

		public void AddMessage(AbstractMessage message) {
			//TODO
			// Kiểm tra Id người gửi xem có giống với id của user hay không để xác định msg nằm trái hay phải
			// Hiển thị lên giao diện
		}
		
		#endregion

	}

}