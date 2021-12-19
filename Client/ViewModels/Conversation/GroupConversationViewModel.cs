using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UI.Command;
using UI.Models;
using UI.Models.Message;
using UI.Network;
using UI.Network.Packets.AfterLoginRequest.Message;
using UI.Network.Packets.AfterLoginRequest.Notification;
using UI.Network.RestAPI;
using UI.Services;
using UI.Services.Common;
using UI.Utils;
using UI.ViewModels.Search;

namespace UI.ViewModels {
	public class GroupConversationViewModel : ConversationViewModel {

        #region Properties

        private HomeDialogState dialogState;
        public ViewModelBase DialogViewModel {
            get => dialogState.CurrentViewModel;
            set {
                dialogState.CurrentViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }

        #endregion

        #region Command

        public ICommand OpenAddGroupDialog { get; private set; }

		#endregion

        public GroupConversationViewModel(ChatConnection chatConnection, PacketRespondeListener listener, IAppSession appSession, IViewModelFactory factory, IModelContext model, HomeDialogState dialogState) : base(chatConnection, appSession, factory, model)
        {
            IsGroup = true;
            this.dialogState = dialogState; 
            OpenAddGroupDialog = new RelayCommand<object>(null, o => {
                dialogState.CurrentViewModel = factory.Create<SearchViewModel>();
                (dialogState.CurrentViewModel as SearchViewModel).AcceptEvent += AddToGroup;
            });
		}

        private void AddToGroup(List<UserShortInfo> infos) {
            GroupConversationCreateRequest request = new GroupConversationCreateRequest() {
                Members = infos.Select(info => Guid.Parse(info.ID)).ToList(),
                ConversationId = Conversation.ID.ToString()
            };

            ChatConnection.Instance.Session.Send(request);
        }

    }
}