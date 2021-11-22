using System;
using System.Collections.Generic;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;
using UI.Network.RestAPI;

namespace UI.Network.Packets.AfterLoginRequest
{
    public class GetFriendIDsResult : IPacket
    {
        public List<string> ids { get; set; } = new List<string>();

        public void Decode(IByteBuffer buffer)
        {
            string id;
            id = ByteBufUtils.ReadUTF8(buffer);

            while (id != "~")
            {
                ids.Add(id);
                id = ByteBufUtils.ReadUTF8(buffer);
            }    
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            ChatModel.Instance.FriendIDs.Clear();
            //TODO Application.Current.Dispatcher.Invoke(() => UserList.Instance.ClearListView());

            ChatModel model = ChatModel.Instance;
            model.FriendIDs.Clear();
            ConversationList conversationList = ModuleContainer.GetModule<ConversationList>();
            conversationList.view.clear_friend_list();

            foreach (string id in ids)
            {
	            model.FriendIDs.Add(id);

	            // Get short info from ID
	            GetShortInfo packet = new GetShortInfo();
	            packet.ID = id;
	            DataAPI.getData<GetShortInfoResult>(packet);
            }
        }
    }
}
