using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbCommon
{
    public class Ecu
    {
        public Int32 EcuId { get; set; }
        public Int32 CountOfPidsSupported { get; set; }
        public List<EcuModes> Modes { get; set; }

    }

    public class EcuModes
    {
        public OdbPid Mode { get; set; }
        public List<int> SupportedPids { get; set; }

    }
}
