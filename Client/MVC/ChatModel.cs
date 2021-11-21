using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UI.Models;
using UI.Models.Message;
using UI.Utils;

namespace UI.MVC {
	
	public class ChatModel : IModel {

		public static readonly ChatModel Instance = new ChatModel();
		
		private ChatModel() {}
		
		public List<string> FriendIDs { get; set; } = new List<string>();
        public Dictionary<string, UserUtils.ShortProfile> FriendShortProfiles = new Dictionary<string, UserUtils.ShortProfile>();

        #region for reconnect
        public string Hashed;
        #endregion

        public string SelfID { get; set; }

        #region OnChatPage
        public string previousSelectedConversation { get; set; } = "";
        public string currentSelectedConversation { get; set; } = "";
        public List<BubbleInfo> CurrentUserMessages { get; set; } = new List<BubbleInfo>();
        public Dictionary<string, ConversationCache> Conversations { get; set; } = new Dictionary<string, ConversationCache>();

        public ConversationCache computeIfAbsent(string id, ConversationCache cache) {
	        if (!Conversations.ContainsKey(id))
		        Conversations.Add(id, cache);
	        return Conversations[id];
        }

        public ConversationCache getConversationCacheOrDefault(string id) {
	        return computeIfAbsent(id, new ConversationCache());
        }
        public ConversationCache getConversationCache(string id) {
	        return Conversations[id];
        }
        public Dictionary<string, string> PrivateConversations { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, UserControl> UserControls { get; set; } = new Dictionary<string, UserControl>();

        #region OnlineUser

        public readonly HashSet<string> OnlineUser = new HashSet<string>();
        public void updateOnlineStatus(string user, bool status) {
	        if (status)
		        OnlineUser.Remove(user);
	        else
		        OnlineUser.Add(user);
        }

        #endregion

        public Dictionary<int, StickerCategory> PaidSticker = new Dictionary<int, StickerCategory>();

        // Key: conversation-id, Value: index of media message in Messages list in Conversations
        public Dictionary<string, List<MediaPack>> MediaMessages { get; set; } = new Dictionary<string, List<MediaPack>>();
        //public Dictionary<string, MediaPlayerWindow> MediaWindows { get; set; } = new Dictionary<string, MediaPlayerWindow>();
        #endregion

        #region OnDownloadManager
        //public List<DownloadProgressNoti> DownloadProgresses = new List<DownloadProgressNoti>();
        #endregion

        #region OnSettingPage: UserProfile
        public UserProfile Profile { get; set; }
        #endregion

        #region OnSettingPage: UserProfile

        #endregion

	}
	
}