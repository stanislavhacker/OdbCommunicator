using OdbCommunicator.OdbCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbQueries
{
    public class OdbQuery
    {
        public OdbPid Pid { get; set; }
        public DateTime Time { get; private set;  }
        public QueryStatus Status { get; set; }
        public Double Data { get; set; }

        public OdbQuery()
        {
            this.Time = DateTime.Now;
            this.Status = QueryStatus.NoData;
        }
    }
}
