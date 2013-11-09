using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbQueries
{
    public class OdbQueryResponse
    {
        public Double Data { get; set; }
        public String Unit { get; set; }
        public Double MinValue { get; set; }
        public Double MaxValue { get; set; }
    }
}
