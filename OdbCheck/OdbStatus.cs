using OdbCommunicator.OdbCommon;
using OdbCommunicator.OdbExceptions;
using OdbCommunicator.OdbSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbCheck
{
    public class OdbStatus
    {
        private OdbSocket socket;
        private OdbReporter reporter = new OdbReporter();

        /// <summary>
        /// Odb Status
        /// </summary>
        /// <param name="socket"></param>
        public OdbStatus(OdbSocket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// Check if device is ODB
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsOdb()
        {
            if (this.socket.IsConnected)
            {
                try
                {
                    await this.resetAndMakeInitialization();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }






        /// <summary>
        /// Reset and make initialization
        /// </summary>
        /// <returns></returns>
        private async Task resetAndMakeInitialization()
        {
            try
            {
                await this.socket.SendAndCheck(OdbPids.ATZ);
                await this.socket.SendAndCheck(OdbPids.ATE0);
                await this.socket.SendAndCheck(OdbPids.ATL0);

                await this.checkSupportedProtocols();
                await this.socket.Send(OdbPids.ATH1);
            }
            catch
            {
                throw new OdbException(OdbError.DeviceIsNotOdbCompatible);
            }
        }

        /// <summary>
        /// Check supported PIds
        /// </summary>
        /// <returns></returns>
        private async Task checkSupportedProtocols(OdbProtocol protocolType = OdbProtocol.Unknown, int protocolNumber = 0)
        {
            var selectedProtocol = -1;

            if (protocolType == OdbProtocol.Unknown)
            {
                for (selectedProtocol = 1; selectedProtocol <= 9; selectedProtocol++)
                {
                    OdbPid protocol = OdbPids.GetPidForProtocolNumber(selectedProtocol);
                    try
                    {
                        await this.socket.SendAndCheck(protocol);
                        await this.socket.SendAndCheck(OdbPids.Mode1_PidsSupported20);
                        break;
                    }
                    catch
                    {
                        reporter.ReportInfo("Protocol ATSP" + protocol.Description + " is not supported.");
                    }
                }

                if (selectedProtocol == 10)
                {
                    throw new OdbException(OdbError.CouldNotFindCompatibleProtocol);
                }

                this.socket.SelectedProtocol = selectedProtocol;
            }
            else if (protocolType == OdbProtocol.Specified && protocolNumber > 0 && protocolNumber < 10)
            {
                await this.socket.SendAndCheck(OdbPids.GetPidForProtocolNumber(protocolNumber));
                await this.socket.SendAndCheck(OdbPids.Mode1_PidsSupported20);

                this.socket.SelectedProtocol = protocolNumber;
            }
            else
            {
                throw new OdbException(OdbError.WrongProtocolNumber);
            }
        }
    }
}
