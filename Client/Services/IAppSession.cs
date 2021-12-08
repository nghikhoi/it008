using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UI.Services {
    public interface IAppSession {

        string SessionKey { get; set; }
        event PropertyChangedEventHandler SessionKeyChanged;
        string SessionID { get; set; }
        event PropertyChangedEventHandler SessionIDChanged;

    }
}
