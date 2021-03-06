using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Network.Packets;

namespace ChatServer.Network.Protocol
{
    public class AfterLoginProtocol : ChatProtocol
    {
        public AfterLoginProtocol() : base("After Login")
        {
            Inbound(0x20, new GetSelfIDRequest());
            Outbound(0x20, new GetSelfIDResponse());

            Inbound(0x00, new FriendsListRequest());
            Outbound(0x00, new FriendsListResponse());

            Inbound(0x01, new ShortProfileRequest());
            Outbound(0x01, new ShortProfileResponse());

            Inbound(0x03, new UserSearchRequest());
            Outbound(0x03, new UserSearchResponse());

            Inbound(0x05, new SendMessageRequest());

            Outbound(0x04, new SendMessageResponse());

            // Conversations
            Inbound(0x06, new ConversationFrIDRequest());
            Outbound(0x06, new ConversationFrIDResponse());

            Inbound(0x02, new SingleConversationFrUserIDRequest());
            Outbound(0x02, new SingleConversationFrUserIDResponse());

            Inbound(0x25, new GroupConversationAddRequest());
            Outbound(0x25, new GroupConversationAddedResponse());

            Inbound(0x16, new MessageFromConversationRequest());
            Outbound(0x16, new MessageFromConversationResponse());

            Inbound(0x17, new GetConversationShortInfoRequest());
            Outbound(0x17, new GetConversationShortInfoResponse());

            Inbound(0x18, new RecentConversationsRequest());
            Outbound(0x18, new RecentConversationsResponse());

            Inbound(0x19, new BuzzRequest());
            Outbound(0x19, new BuzzResponse());

            Outbound(0x49, new GetConversationShortInfoNotify());

            Inbound(0x50, new ChangeBubbleChatColor());
            Outbound(0x50, new ChangeBubbleChatColor());

            Inbound(0x51, new SetAvatarRequest());
            Outbound(0x51, new SetAvatarResponse());

            Inbound(0x52, new SetNicknamesRequest());
            Outbound(0x52, new SetNicknamesResponse());

            // Notifications
            Outbound(0x07, new UserOnline());
            Outbound(0x08, new UserOffline());

            Inbound(0x11, new AddFriendRequest());
            Outbound(0x10, new ForwardedFriendRequest());
            Inbound(0x13, new ResponseAddFriendRequest());
            Outbound(0x12, new AcceptedFriendResponse());
            Outbound(0x14, new FinalizeAcceptedFriendRequest());

            Inbound(0x15, new GetNotificationsRequest());
            Outbound(0x15, new GetNotificationsResponse());

            // Profile
            Inbound(0x09, new DisplayedProfileRequest());
            Outbound(0x09, new DisplayedProfileResponse());

            Inbound(0x30, new GetSelfProfileRequest());
            Outbound(0x30, new GetSelfProfileResponse());

            Inbound(0x31, new UpdateSelfProfileRequest());

            Inbound(0x32, new ModifyPasswordRequest());
            Outbound(0x32, new ModifyPasswordResponse());

            Inbound(0x33, new ChatThemeGetRequest());

            Inbound(0x34, new ChatThemeSetRequest());
            Outbound(0x34, new ChatThemeSetRequest());

            // Sticker
            Inbound(0x21, new GetBoughtStickerPacksRequest());
            Outbound(0x21, new GetBoughtStickerPacksResponse());
            Inbound(0x22, new GetNearestSickerRequest());
            Outbound(0x22, new GetNearestSickerResponse());

            Inbound(0x23, new BuyStickerCategoryMixed());
            Outbound(0x23, new BuyStickerCategoryMixed());




            // Media
            Inbound(0x40, new MediaFromConversationRequest());
            Outbound(0x40, new MediaFromConversationResponse());
        }
    }
}
