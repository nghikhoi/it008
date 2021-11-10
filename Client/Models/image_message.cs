using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UI.Models
{
    public class image_message:message
    {
        public string id { get; private set; }
        public image_message(string id = "0") : base()
        {
            this.id = id;
        }
        
    }
}
