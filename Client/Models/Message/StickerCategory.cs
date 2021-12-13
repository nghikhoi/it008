using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models.Message
{
    public class StickerCategory
    {
        public List<Sticker> Stickers { get; set; } = new List<Sticker>();
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalImage { get; set; }
        public string ThumbURL { get; set; }
        public string IconURL { get; set; }
        public string IconPreview { get; set; }
        public int Group { get; set; }
        public string ThumbImg { get; set; }
        public string Source { get; set; }
        public int Type { get; set; }
        public string SourceURL { get; set; }
        public int IsHidden { get; set; }
        public int Order { get; set; }
    }
}
