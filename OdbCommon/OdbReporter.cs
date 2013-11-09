using OdbCommunicator.OdbQueries;
using OdbCommunicator.OdbSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbCommon
{
    public class OdbReporter
    {
        private const String ErrorPrefix = "ObdCommunicator | ERROR >>> ";
        private const String InfoPrefix = "ObdCommunicator | INFO: ";
        private const String ResponsePrefix = "ObdCommunicator | ";

        /// <summary>
        /// report error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public void ReportError(string message, string code)
        {
            if (!OdbClient.OBD_REPORTER_ENABLED)
            {
                return;
            }
            Debug.WriteLine(ErrorPrefix + "{0} occured with error code \"{1}\"", message, code);
        }

        /// <summary>
        /// Report info
        /// </summary>
        /// <param name="message"></param>
        public void ReportInfo(string message)
        {
            if (!OdbClient.OBD_REPORTER_ENABLED)
            {
                return;
            }
            Debug.WriteLine(InfoPrefix + message);
        }

        /// <summary>
        /// report response
        /// </summary>
        /// <param name="odbResponse"></param>
        public void ReportResponse(OdbResponse odbResponse)
        {
            if (!OdbClient.OBD_REPORTER_ENABLED)
            {
                return;
            }

            if (odbResponse.IsValid)
            {
                Debug.WriteLine(ResponsePrefix + odbResponse.Pid.Pid + " (" + odbResponse.Pid.Description + ") > " + odbResponse.Response);
            }
            else
            {
                Debug.WriteLine(ErrorPrefix + odbResponse.Pid.Pid + " (" + odbResponse.Pid.Description + ") > Invalid response from OBD device.");
            }
        }

        /// <summary>
        /// Report selected ecu
        /// </summary>
        /// <param name="selectedEcu"></param>
        public void ReportEcu(Ecu selectedEcu)
        {
            if (!OdbClient.OBD_REPORTER_ENABLED)
            {
                return;
            }

            if (selectedEcu != null)
            {
                Debug.WriteLine(InfoPrefix + "Using car ECU with id '{0}' and {1} supporteds pids.", selectedEcu.EcuId, selectedEcu.CountOfPidsSupported);
            }
            else
            {
                Debug.WriteLine(ErrorPrefix + "There is not compatible ECU for your car.");
            }
        }

        /// <summary>
        /// Report new query
        /// </summary>
        /// <param name="newQuery"></param>
        public void ReportNewQuery(OdbQuery newQuery)
        {
            if (!OdbClient.OBD_REPORTER_ENABLED)
            {
                return;
            }

            if (newQuery.Status == QueryStatus.NotSupported)
            {
                Debug.WriteLine(InfoPrefix + "Query for '{0} ({1})' can not be registered because this PID is not supported.", newQuery.Pid.Description, newQuery.Pid.Pid);
            }
            else
            {
                Debug.WriteLine(InfoPrefix + "Registering new query for '{0} ({1})'.", newQuery.Pid.Description, newQuery.Pid.Pid);
            }
        }
    }
}
