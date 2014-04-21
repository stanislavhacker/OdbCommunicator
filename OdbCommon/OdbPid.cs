using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbCommon
{
    public class OdbPid
    {
        /// <summary>
        /// Pid
        /// </summary>
        private String pid = "";
        public String Pid {
            get
            {
                if (Mode == null)
                {
                    return pid;
                }
                if (IsFastReading == false)
                {
                    return Mode.PidPrefix + " " + pid;
                }
                return Mode.PidPrefix + " " + pid + " 1";
            }
            set
            {
                pid = value;
            }
        }

        /// <summary>
        /// Pid prefix for mode type PID
        /// </summary>
        private String pidPrefix = "";
        public String PidPrefix
        {
            get
            {
                return pidPrefix;
            }
            set
            {
                pidPrefix = value;
            }
        }

        /// <summary>
        /// String that must by in response from OBD
        /// </summary>
        public String ExpectedResponse { get; set; }

        /// <summary>
        /// Is not obd but ELM327 command type
        /// </summary>
        public Boolean IsElmCommand { get; set; }

        /// <summary>
        /// Is fast reading
        /// </summary>
        public Boolean IsFastReading { get; set; }

        
        /// <summary>
        /// Is data command
        /// </summary>
        public Boolean IsDataCommand { get; set; }

        /// <summary>
        /// Bytes count that must be in response
        /// </summary>
        public int ByteCount { get; set; }

        /// <summary>
        /// Mode for current pid
        /// </summary>
        public OdbPid Mode { get; set; }

        /// <summary>
        /// Obd command priority
        /// </summary>
        public OdbPriority Priority { get; set; }

        public String Description { get; set; }
        public Double MinValue { get; set; }
        public Double MaxValue { get; set; }
        public String Units { get; set; }

        /// <summary>
        /// Compute action with four bytes
        /// </summary>
        public Func<int, int, int, int, Double> Compute { get; set; }

        /// <summary>
        /// Get pid id in decimal without mode
        /// </summary>
        /// <returns></returns>
        public int GetPidIdInDecimal()
        {
            try
            {
                return Convert.ToInt32(pid, 16);
            }
            catch
            {
                return -1;
            }
        }
    }
}
