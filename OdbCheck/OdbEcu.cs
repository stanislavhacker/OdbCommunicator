using OdbCommunicator.OdbCommon;
using OdbCommunicator.OdbSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbCheck
{
    public class OdbEcu
    {        
        private OdbSocket socket;
        private OdbCommands comamnds;
        private OdbReporter reporter = new OdbReporter();

        /// <summary>
        /// Ecu ccurrent
        /// </summary>
        private Ecu current = null;
        public Ecu Current
        {
            get
            {
                return current;
            }
        }
        

        /// <summary>
        /// Odb Ecu
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="comamnds"></param>
        public OdbEcu(OdbSocket socket, OdbCommands comamnds)
        {
            this.socket = socket;
            this.comamnds = comamnds;
        }

        /// <summary>
        /// Load ecu for reading
        /// </summary>
        public void LoadCurrentEcu()
        {
            int countPids = 0;
            List<Ecu> ecus = this.comamnds.GetEcus();
            Ecu selectedEcu = null;

            foreach (Ecu ecu in ecus)
            {
                if (countPids < ecu.CountOfPidsSupported)
                {
                    countPids = ecu.CountOfPidsSupported;
                    selectedEcu = ecu;
                }
            }

            reporter.ReportEcu(selectedEcu);
            this.current = selectedEcu;
        }
    }
}
