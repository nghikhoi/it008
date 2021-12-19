using UI.Models.Message;
using UI.Services;

namespace UI.ViewModels
{
    public class AnnouncementViewModel : MessageViewModel
    {

		public new AnnouncementMessage Message
		{
			get => (AnnouncementMessage) base.Message;
			set => base.Message = value;
		}

		public string Text { get; set; }

		public AnnouncementViewModel(IAppSession appSession) : base(appSession)
		{

		}

        public void InitText(ConversationViewModel conversation)
        {
            switch (Message.Type)
            {
                case AnnouncementType.CHANGE_NICKNAME:
                {
                    Text = Message.Value;
                    return;
                }
            }
        }
	}
}
