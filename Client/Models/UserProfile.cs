using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class UserProfile
    {
        
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
        public Guid user_id { get; private set; } = new Guid();
        
    }
}
