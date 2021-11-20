﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using UI.Models;
using UI.Models.Message;
using UI.MVC;
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
				foreach (string key in recentResult.Conversations.Keys) {
					GetConversationShortInfo packet = new GetConversationShortInfo();
					packet.ConversationID = key;
					DataAPI.getData<GetConversationShortInfoResult>(packet, infoResult => {
						AddShortInfoConversation(infoResult.ConversationID, infoResult.ConversationName, infoResult.LastActive);
					});
				}
			});
		}

		#region Message

		public void IncomingMessage(string ConversationId, AbstractMessage message, bool seeing) {
			//TODO:
			// 1. Hiển thị tin nhắn mới nhất lên danh sách chat
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
		}

		#endregion

		#region Search

		public void ShowSearchResult(List<SearchResult> list) {
			//TODO hiển thị danh sách lên giao diện
		}

		#endregion

	}

}