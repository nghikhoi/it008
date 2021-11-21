using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class video_message
    {
        public string id { get; private set; }
        public video_message(string id = "0") : base()
        {
            this.id = id;
        }
    }
}
