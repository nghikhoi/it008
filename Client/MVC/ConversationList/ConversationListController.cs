using System;
using System.Collections.Generic;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;
using UI.MVC;
using UI.Network.Packets.AfterLoginRequest;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Search;
using UI.Network.RestAPI;

namespace UI.MVC {
	
	public partial class ConversationListController : IController {

		private ucListRecentMessage view;

		public ConversationListController(ucListRecentMessage view) {
			this.view = view;
		}

		public void loadRecentConversation() {
			DataAPI.getData<RecentConversations, RecentConversationsResult>(recentResult => {
                view.clear_recent_conversation();
                foreach (string key in recentResult.Conversations.Keys) {
					GetConversationShortInfo packet = new GetConversationShortInfo();
					packet.ConversationID = key;
					DataAPI.getData<GetConversationShortInfoResult>(packet, infoResult => {
						AddShortInfoConversation(infoResult.ConversationID, infoResult.ConversationName, infoResult.LastActive);
					});
				}
			});
		}

		public void addShortInfo(UserShortInfo info) {
			ChatListItemControl item = new ChatListItemControl();
			item.ConversationID = info.ConversationID;
			item.NameBlock.Text = info.FirstName+" "+info.LastName;
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(info.LastActive).ToLocalTime();
			item.LastActive = dtDateTime;
			string s = "";
			s += info.FirstName[0];
			s += " " + info.LastName[0];
			item.ConverstionIcon.Text = s;
			view.update_friend_list(item);
		}

		public void loadFriends() {
			DataAPI.getData<GetFriendIDs, GetFriendIDsResult>(listResult => {
				ChatModel model = ChatModel.Instance;
				model.FriendIDs.Clear();
				view.clear_friend_list();

				foreach (string id in listResult.ids)
				{
					model.FriendIDs.Add(id);

					// Get short info from ID
					GetShortInfo packet = new GetShortInfo();
					packet.ID = id;
					DataAPI.getData<GetShortInfoResult>(packet, infoResult => {
						addShortInfo(infoResult.info);
					});
				}
			});
		}

		#region Message

		public void IncomingMessage(string ConversationId, AbstractMessage message, bool seeing) {
			//TODO:
			// 1. Hiển thị tin nhắn mới nhất lên danh sách 
			foreach(ChatListItemControl item in view.listFriend.Children)
            {
				if(item != null)
                {
					if(message is TextMessage)
                    {
						item.LatestMsg.Text = (message as TextMessage).Message;
                    }
					else if(message is VideoMessage)
                    {
						item.LatestMsg.Text = "Sent a video.";
                    }
                    else if(message is ImageMessage)
                    {
						item.LatestMsg.Text = "Sent a picture.";
                    }
                }
            }
		}
		
		public void AddConversation(AbstractConversation conversation) {
			//TODO
			// 1. Cập nhật data vào ChatModel
			// 2. Hiển thị lên danh sách
		}
		
		public void AddShortInfoConversation(string ConversationId, string ConversationName, long lastActive) {
			//TODO
			// 1. Cập nhật data vào ChatModel
			// 2. Hiển thị lên danh sách
			ChatListItemControl item = new ChatListItemControl();
			item.ConversationID = ConversationId;
			item.NameBlock.Text = ConversationName;
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(lastActive).ToLocalTime();
			item.LastActive = dtDateTime;
			string s = "";
			s += ConversationName[0];
			for(int i = 0; i < ConversationName.Length; i++)
			{
				if(ConversationName[i]==' ' && i<ConversationName.Length &&ConversationName[i + 1] != '\0')
				{
					s += ConversationName[i + 1];
				}
			}
			item.ConverstionIcon.Text = s;
			view.update_recent_conversation(item);
		}

		public void AddShortInfoConversation_active()
        {
			Dictionary<string, ConversationCache> conversations = ChatModel.Instance.Conversations;
			if (conversations.Count == 0) return;
			foreach (var con in conversations)
			{
				AddShortInfoConversation(con.Key, con.Value.ConversationName, con.Value.LastActiveTime);
			}
		}

		#endregion

		#region Search
		public void SearchRecentConversation(string s)
		{
			List<UserShortInfo> searchlist = new List<UserShortInfo>();
			Dictionary<string, ConversationCache> conversations = ChatModel.Instance.Conversations;
			foreach (var con in conversations)
			{
				if (s.Contains(con.Value.ConversationName))
				{
					UserShortInfo result = new UserShortInfo();
					result.ConversationID = con.Key;
					result.FirstName = con.Value.ConversationName;
					result.LastActive = con.Value.LastActiveTime;
					searchlist.Add(result);
				}
			}
			//TODO show recent
		}
		public void SearchAction(string s)
		{
			SearchUser packet = new SearchUser();
			packet.Email = s;
			DataAPI.getData<SearchUserResult>(packet, result => {
				ShowSearchResult(result.Results);
			});
		}

		public void ShowSearchResult(List<UserShortInfo> list) {
			view.clear_friend_list();
			foreach (var item in list)
            {
				addShortInfo(item);
            }
		}

		#endregion

	}

}