using System;
using System.Collections.Generic;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.MVC;

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

            foreach (var id in ids)
            {
                Console.WriteLine(id);
                ChatModel.Instance.FriendIDs.Add(id);

                // Get short info from ID
                GetShortInfo packet = new GetShortInfo();
                packet.ID = id;
                _ = ChatConnection.Instance.Send(packet);
            }
        }
    }
}
