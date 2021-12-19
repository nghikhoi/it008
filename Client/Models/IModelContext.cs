using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UI.Models.Message;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.RestAPI;

namespace UI.Models {
    public interface IModelContext
    {

        event Action<Guid> NewConversationEvent;
        void UpdateColor(Guid ConversationId, Color color);
        void SendMessage(Guid ConversationId, AbstractMessage message);
        AbstractConversation GetConversation(Guid guid);

        void LoadMessages(Guid id, int quantity = 10);

        void LoadMedias(Guid id, int quantity = 5);

        void LoadAttachments(Guid id, int quantity = 5);

        void LoadConversation(Guid guid);

        AbstractConversation GetConversationFromUserId(Guid guid);

        List<UserShortInfo> GetFriendList(bool reload = false);

        List<UserShortInfo> Search(string searchString);

        HashSet<Guid> GetRecentConversations(bool reload = false);

    }
}
