using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UI.Annotations;
using UI.Models.Message;

namespace UI.Models.Message
{

    public delegate void MessageAddHandler(AbstractMessage message, bool LoadFromServer);
    public delegate void MediaAddHandler(MediaAbstractMessage message, bool LoadFromServer);

    public class AbstractConversation : IConversation, INotifyPropertyChanged
    {
        public MessageAddHandler MessageAddEvent;
        public MediaAddHandler MediaAddEvent;
        public Guid ID { get; set; }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _avatar;
        public string Avatar
        {
            get => _avatar;
            set
            {
                _avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }

        public long LastActive { get; set; }
        public HashSet<Guid> Members { get; set; } = new HashSet<Guid>();
        public List<AbstractMessage> Messages { get; set; } = new List<AbstractMessage>();
        public List<MediaAbstractMessage> Medias { get; set; } = new List<MediaAbstractMessage>();
        public Dictionary<Guid, string> Nicknames { get; set; } = new Dictionary<Guid, string>();
        public int LastMessID { get; set; }
        public int LastMediaID { get; set; }
        public int LastMediaIDBackup { get; set; }
        public int LastAttachmentID { get; set; }
        private HashSet<Guid> OnlineList = new HashSet<Guid>();
        public void UpdateStatus(Guid id, bool Online)
        {
            if (Online)
                OnlineList.Add(id);
            else OnlineList.Remove(id);
            OnPropertyChanged(nameof(IsOnline));
        }
        public bool IsOnline => OnlineList.Count > 0;
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool Loaded = false;

        public void UpdateNickname(Guid id, string nickname)
        {
            Nicknames[id] = nickname;
            OnPropertyChanged(nameof(Nicknames));
        }

        public void AddMessage(AbstractMessage message, bool LoadFromServer = false)
        {
            AddToCollection(Messages, message, LoadFromServer, false);
            MessageAddEvent?.Invoke(message, LoadFromServer);
        }

        public void AddMedia(MediaAbstractMessage media, bool LoadFromServer = false) {
            AddToCollection(Medias, media, LoadFromServer, false);
            MediaAddEvent?.Invoke(media, LoadFromServer);
        }

        private static void AddToCollection(IList collection, object obj, bool LoadFromServer, bool reverse)
        {
            bool indexBool = LoadFromServer && !reverse;
            if (indexBool)
            {
                collection.Insert(0, obj);
            }
            else
            {
                collection.Add(obj);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
