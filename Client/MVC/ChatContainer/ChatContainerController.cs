using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;
using UI.MVC;
using System.Windows.Media.Imaging;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.RestAPI;
using UI.Utils;

//todo: cac ham de add tin nhan trong view : text: update_message_container update_meaage_container_rcv (up load vao cuoi trang); update_msgcontainer_at_top update_msgcontainer_rcv_at_top (up vao dau trang)
//todo: hinh anh: update_image_message update_image_message_rcv update_image_message_at_top update_image_message_rcv_at_top
//todo: video: update_file_message update_file_message_rcv update_file_message_at_top update_file_message_rcv_at_top
//todo: xu not phan cap nhat tin nhan nha ck.
namespace UI.MVC {
	
	public partial class ChatContainerController : IController {

		private ChatPage view;

		public ChatContainerController(ChatPage view) {
			this.view = view;
		}
		
		public string ConversationID {
			get => ChatModel.Instance.currentSelectedConversation;
		}

		#region Message

		#region sendMessage

		public void sendTextMessage(string text) {
			TextMessage message = null;
			//TODO
			sendMessage(message);
		}

		public void sendImageMessage(List<string> imagePaths) {
			Action removeAction = view.addFakeLoading();
			FileAPI.UploadMedia(ConversationID, imagePaths, result => {
				foreach (var image in result)
				{
					string fileName = image.Key;
					string fileID = image.Value;

					ImageMessage message = new ImageMessage();
					message.FileID = fileID;
					message.FileName = fileName;

					sendMessage(message);
				}
				removeAction.Invoke();
			}, error => {
				
			});
		}

		public void sendVideoMessage(List<string> videoPaths) {
			Action removeAction = view.addFakeLoading();
			FileAPI.UploadMedia(ConversationID, videoPaths, result => {
				foreach (var video in result)
				{
					string fileName = video.Key;
					string fileID = video.Value;

					VideoMessage message = new VideoMessage();
					message.FileID = fileID;
					message.FileName = fileName;

					sendMessage(message);
				}
				removeAction.Invoke();
			}, error => {
				
			});
		}

		public void sendAttachmentMessage(List<string> attachmentPaths) {
			Action removeAction = view.addFakeLoading();
			FileAPI.UploadAttachment(ConversationID, attachmentPaths, result => {
				foreach (var attachment in result)
				{
					string fileName = attachment.Key;
					string fileID = attachment.Value;

					AttachmentMessage message = new AttachmentMessage();
					message.FileID = fileID;
					message.FileName = fileName;

					sendMessage(message);
				}
				removeAction.Invoke();
			}, error => {
				
			});
		}

		public void sendStickerMessage(Sticker sticker) {
			StickerMessage message = new StickerMessage();
			message.Sticker = sticker;
			sendMessage(message);
		}
		
		public void sendMessage(AbstractMessage message) {
			#region DisplayToView
			AddMessage(message);
			#endregion

			#region SendToServer
			SendMessage packet = new SendMessage();
			packet.Message = message;
			packet.ConversationID = ConversationID;
			_ = ChatConnection.Instance.Send(packet);
			#endregion
		}

		#endregion

		public void SetBubbleColor(Color color) {
			//TODO Đổi màu bubble chat
			var chatitem = new ucChatItem();
			chatitem.Background = new SolidColorBrush(color);
		}

		public void AddMediaToCurrentSelectedConversation(List<string> FileIDs, List<string> FileNames, List<int> Positions) {
			//TODO
			// Cập nhật các media vô trình media hiện tại
		}

		public void AddMessage(AbstractMessage message) {
			//TODO
			// Kiểm tra Id người gửi xem có giống với id của user hay không để xác định msg nằm trái hay phải
			// Hiển thị lên giao diện
			ChatModel model = ChatModel.Instance;
			if(message is TextMessage)
            {
				TextMessage txtmsg = (TextMessage)message;
                if (txtmsg.SenderID == model.SelfID)
                {
					view.update_message_container(txtmsg);
                }
                else
                {
					view.update_meaage_container_rcv(txtmsg);
                }
            }
			else if(message is VideoMessage)
            {
				string defaultpath = "C:\\";
				//todo: lay URI cho em no nha
				VideoMessage vimess = (VideoMessage)message;

                if (vimess.ID == model.SelfID)
				{

					Uri filepath = new Uri(defaultpath);
					view.update_file_message(filepath);
				}
                else
                {
					Uri filepath = new Uri(defaultpath);
					view.update_file_message_rcv(filepath);
				}
            }
			else if(message is ImageMessage)
            {
				string defaultpath = "C:\\";
				//todo: lay URI cho em no nha
				ImageMessage immesg = (ImageMessage)message;
				Uri filepath = new Uri(defaultpath);

				BitmapImage myimage = new BitmapImage();
				myimage.BeginInit();
				myimage.UriSource = filepath;
				myimage.EndInit();
				if (immesg.ID == model.SelfID)
				{
					view.update_image_message(myimage);
					
				}
				else
				{
					view.update_image_message_rcv(myimage);
				}
			} else if (message is StickerMessage) {
				
			}

		}

		public void LoadMessages(string id) {
			
		}
		
		#endregion

	}

}