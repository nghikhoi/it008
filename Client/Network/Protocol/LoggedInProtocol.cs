using UI.Network.Packets.AfterLoginRequest;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.Packets.AfterLoginRequest.Profile;
using UI.Network.Packets.AfterLoginRequest.Search;
using UI.Network.Packets.AfterLoginRequest.Sticker;

namespace UI.Network.Protocol
{
    public class LoggedInProtocol : ChatProtocol
    {
        public LoggedInProtocol() : base("LoggedIn Protocol")
        {
            Inbound(0x20, new GetSelfIDResult());
            Outbound(0x20, new GetSelfID());

            Inbound(0x00, new GetFriendIDsResult());
            Outbound(0x00, new GetFriendIDs());

            Inbound(0x01, new GetShortInfoResult());
            Outbound(0x01, new GetShortInfo());

            Inbound(0x04, new ReceiveMessage());

            Outbound(0x05, new SendMessage());

            Inbound(0x02, new SingleConversationFrUserIDResult());
            Outbound(0x02, new SingleConversationFrUserID());

            Inbound(0x25, new GroupConversationAddedResponse());
            Outbound(0x25, new GroupConversationCreateRequest());

            Inbound(0x16, new GetMessageFromConversationResult());
            Outbound(0x16, new GetMessageFromConversation());

            Inbound(0x06, new ConversationFromIDResult());
            Outbound(0x06, new ConversationFromID());

            Inbound(0x17, new GetConversationShortInfoResult());
            Outbound(0x17, new GetConversationShortInfo());

            Inbound(0x18, new RecentConversationsResult());
            Outbound(0x18, new RecentConversations());

            Inbound(0x19, new BuzzReceive());
            Outbound(0x19, new BuzzSend());

            Inbound(0x49, new GetConversationShortInfoNotify());

            Inbound(0x50, new BubbleChatColorSetRequest());
            Outbound(0x50, new BubbleChatColorSetRequest());

            Outbound(0x51, new SetAvatarRequest());
            Inbound(0x51, new SetAvatarResponse());

            Outbound(0x52, new SetNicknamesRequest());
            Inbound(0x52, new SetNicknamesResponse());

            // Notifications
            Inbound(0x07, new UserOnlineReceive());
            Inbound(0x08, new UserOfflineReceive());

            Inbound(0x10, new FriendRequestReceive());
            Outbound(0x11, new FriendRequest());
            Inbound(0x12, new FriendRequestResult());
            Outbound(0x13, new ResponseFriendRequest());
            Inbound(0x14, new FinalizeAcceptedFriendRequestReceive());

            Inbound(0x15, new GetNotificationsResult());
            Outbound(0x15, new GetNotifications());

            // Search
            Inbound(0x03, new SearchUserResult());
            Outbound(0x03, new SearchUser());

            // Profile
            Inbound(0x09, new GetDisplayedProfileResult());
            Outbound(0x09, new GetDisplayedProfile());

            Inbound(0x30, new GetSelfProfileResult());
            Outbound(0x30, new GetSelfProfile());

            Outbound(0x31, new UpdateSelfProfile());

            Inbound(0x32, new ModifyPasswordResult());
            Outbound(0x32, new ModifyPassword());

            Outbound(0x33, new ChatThemeGetRequest());

            Inbound(0x34, new ChatThemeSetRequest());
            Outbound(0x34, new ChatThemeSetRequest());

            // Sticker
            Inbound(0x21, new GetBoughtStickerPacksResponse());
            Outbound(0x21, new GetBoughtStickerPacksRequest());
            Inbound(0x22, new GetNearestSickerResponse());
            Outbound(0x22, new GetNearestSickerRequest());

            Inbound(0x23, new BuyStickerCategoryResponse());
            Outbound(0x23, new BuyStickerCategoryRequest());






            // Media
            Inbound(0x40, new GetMediaFromConversationResult());
            Outbound(0x40, new GetMediaFromConversation());
        }
    }
}
