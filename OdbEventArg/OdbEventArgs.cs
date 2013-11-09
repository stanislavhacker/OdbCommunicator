using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbEventArg
{
    public class OdbEventArgs : EventArgs
    {
        public OdbClient Client { get; set; }
    }
}
