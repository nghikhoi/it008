using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    class voice_message:message
    {
        public string id { get; private set; }
        public voice_message(string id = "0") : base()
        {
            this.id = id;
        }
    }
}
