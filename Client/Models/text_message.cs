using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    class text_message:message 
    {
        public string content { get; set; }
        public text_message(string content="") : base()
        {
            this.content = content;
        }
    }
}
