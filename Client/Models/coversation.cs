using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    class conversation
    {
        public List<message> list_message { get; set; } = new List<message>();
        public List<user_infomation> list_user { get; set; } = new List<user_infomation>();
        public Guid conversation_id = new Guid();
        public conversation(List<message> lsmsg, List<user_infomation > lsusrs)
        {
            list_message = lsmsg;
            list_user = lsusrs;
        }
    }
}
