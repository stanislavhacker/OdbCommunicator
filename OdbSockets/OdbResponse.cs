using OdbCommunicator.OdbCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbSockets
{
    public class OdbResponse
    {
        public OdbPid Pid { get; set; }
        public String Response { get; set; }
        public TimeSpan Time { get; set; }
        public Boolean IsValid { get; set; }
    }
}
