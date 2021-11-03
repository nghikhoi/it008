using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public abstract class message
    {
        public string sender_code { get; private set; }
        public string conversation_code { get; private set; }
        public string message_code { get; private set; }
        public bool deleted { get; private set; }
        public message()
        {
            deleted = false;
        }

        public void delete_messgae(string owner)
        {
            if(owner==sender_code)
                deleted = true;
        }
    }
}
