using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using UI.Models.Message;

namespace UI.Models.Message
{

    public delegate void MessageAddHandler(AbstractMessage message, int index);
    public delegate void MediaAddHandler(MediaAbstractMessage message, int index);

    public class AbstractConversation : IConversation
    {
        public MessageAddHandler MessageAddEvent;
        public MediaAddHandler MediaAddEvent;
        public Guid ID { get; set; }
        public string Name { get; set; }
        public long LastActive { get; set; }
        public HashSet<Guid> Members { get; set; } = new HashSet<Guid>();
        public List<AbstractMessage> Messages { get; set; } = new List<AbstractMessage>();
        public List<MediaAbstractMessage> Medias { get; set; } = new List<MediaAbstractMessage>();
        public Dictionary<Guid, string> Nicknames { get; set; } = new Dictionary<Guid, string>();
        public int LastMessID { get; set; }
        public int LastMediaID { get; set; }
        public int LastMediaIDBackup { get; set; }
        public int LastAttachmentID { get; set; }
        public Color Color { get; set; }
        public bool Loaded = false;

        public void AddMessage(AbstractMessage message, bool LoadFromServer = false)
        {
            AddToCollection(Messages, message, LoadFromServer, false);
            MessageAddEvent?.Invoke(message, Messages.Count);
        }

        public void AddMedia(MediaAbstractMessage media, bool LoadFromServer = false) {
            AddToCollection(Medias, media, LoadFromServer, false);
            MediaAddEvent?.Invoke(media, Medias.Count);
        }

        private static void AddToCollection(IList collection, object obj, bool LoadFromServer, bool reverse)
        {
            bool indexBool = LoadFromServer && !reverse;
            if (indexBool)
                collection.Insert(0, obj);
            else collection.Add(obj);
        }
    }
}
