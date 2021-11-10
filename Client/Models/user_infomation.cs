using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class user_infomation
    {
        public string email { get; set; }
        public string ho { get; set; }
        public string ten { get; set; }
        public string password { get; set; }
        public DateTime birth_day { get; set; }
        public enum gender { male, female, other }
        public gender sex { get; set; }
        public Guid user_id { get; private set; } = new Guid();
    }
}
