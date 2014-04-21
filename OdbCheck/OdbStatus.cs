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
                    await this.Initialize();
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
        /// Initialize
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
            try
            {
                await this.SoftReset();
                await this.Echo(false);
                await this.LineFeed(false);
                await this.Headers(true);
                await this.LoadSupportedProtocols();
                await this.ShowProcotolName();
            }
            catch
            {
                throw new OdbException(OdbError.DeviceIsNotOdbCompatible);
            }
        }

        /// <summary>
        /// Show protocol name
        /// </summary>
        /// <returns></returns>
        public async Task ShowProcotolName()
        {
            await this.socket.SendAndCheck(OdbPids.ATDP);
        }

        /// <summary>
        /// Reset
        /// </summary>
        /// <returns></returns>
        public async Task Reset()
        {
            await this.socket.SendAndCheck(OdbPids.ATZ);
        }

        /// <summary>
        /// Soft reset
        /// </summary>
        /// <returns></returns>
        public async Task SoftReset()
        {
            await this.socket.SendAndCheck(OdbPids.ATWS);
        }

        /// <summary>
        /// Echo
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task Echo(Boolean enable)
        {
            var pid = enable ? OdbPids.ATE1 : OdbPids.ATE0;
            await this.socket.SendAndCheck(pid);
        }

        /// <summary>
        /// Line feed
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task LineFeed(Boolean enable)
        {
            var pid = enable ? OdbPids.ATL1 : OdbPids.ATL0;
            await this.socket.SendAndCheck(pid);
        }

        /// <summary>
        /// Header
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public async Task Headers(Boolean enable)
        {
            var pid = enable ? OdbPids.ATH1 : OdbPids.ATH0;
            await this.socket.SendAndCheck(pid);
        }



        /// <summary>
        /// Load supported PIds
        /// </summary>
        /// <returns></returns>
        private async Task LoadSupportedProtocols(OdbProtocol protocolType = OdbProtocol.Unknown, int protocolNumber = 0)
        {
            var selectedProtocol = -1;

            if (protocolType == OdbProtocol.Unknown)
            {
                OdbPid protocol = null;
                for (selectedProtocol = 0; selectedProtocol <= 9; selectedProtocol++)
                {
                    protocol = OdbPids.GetPidForProtocolNumber(selectedProtocol);
                    try
                    {
                        OdbResponse response = await this.socket.SendAndCheck(protocol);
                        if (response.IsValid) break;
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

                this.socket.SelectedProtocol = protocol;
            }
            else if (protocolType == OdbProtocol.Specified && protocolNumber > 0 && protocolNumber < 10)
            {
                OdbPid protocol = OdbPids.GetPidForProtocolNumber(selectedProtocol);
                await this.socket.SendAndCheck(protocol);
                this.socket.SelectedProtocol = protocol;
            }
            else
            {
                throw new OdbException(OdbError.WrongProtocolNumber);
            }
        }
    }
}
