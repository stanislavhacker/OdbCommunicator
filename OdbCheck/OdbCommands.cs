using OdbCommunicator.OdbCommon;
using OdbCommunicator.OdbSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbCheck
{
    public class OdbCommands
    {
        private OdbSocket socket;
        private Dictionary<int, Dictionary<OdbPid, List<int>>> supportedPids = new Dictionary<int, Dictionary<OdbPid, List<int>>>();

        /// <summary>
        /// Odb Commands
        /// </summary>
        /// <param name="socket"></param>
        public OdbCommands(OdbSocket socket)
        {
            this.socket = socket;
        }


        #region PIDS

        /// <summary>
        /// Get supported pids
        /// </summary>
        /// <returns></returns>
        public async Task LoadSupportedPids()
        {
            await this.registerSupportedPid(OdbPids.Mode1_PidsSupported20);
            await this.registerSupportedPid(OdbPids.Mode1_PidsSupported40);
            await this.registerSupportedPid(OdbPids.Mode1_PidsSupported60);
            await this.registerSupportedPid(OdbPids.Mode1_PidsSupported80);
        }

        /// <summary>
        /// Get supp
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        private async Task registerSupportedPid(OdbPid what)
        {
            OdbResponse response = await this.socket.SendAndCheck(what);
            OdbData data = this.socket.ResolveData(response, what);
            if (data != null)
            {
                this.decodeSupportedPidsAndRegisterIt(what, data);
            }
        }

        /// <summary>
        /// Decode supported pids and  register it
        /// </summary>
        /// <param name="what"></param>
        /// <param name="data"></param>
        private void decodeSupportedPidsAndRegisterIt(OdbPid what, OdbData data)
        {
            List<int> pids = new List<int>();

            int pid = Convert.ToInt32(what.Pid.Split(' ')[1], 16);
            for (int i = 0; i < data.Data.Length; i++)
            {
                char[] binary = Convert.ToString(Convert.ToInt32(data.Data[i], 16), 2).ToCharArray();
                for (int j = 0; j < binary.Length; j++)
                {
                    pid++;
                    if (binary[j] == '1')
                    {
                        pids.Add(pid);
                    }
                }
            }

            this.RegisterSupportedPids(data.EcuIdentifier(), data.Protocol, pids);
        }

        #endregion

        #region ECUS

        /// <summary>
        /// Get all supported ecus
        /// </summary>
        /// <returns></returns>
        public List<Ecu> GetEcus()
        {
            List<Ecu> ecus = new List<Ecu>();
            foreach (KeyValuePair<int, Dictionary<OdbPid, List<int>>> ecuData in supportedPids)
            {
                Ecu ecu = new Ecu();
                ecu.EcuId = ecuData.Key;
                ecu.Modes = new List<EcuModes>();
                ecu.CountOfPidsSupported = 0;

                foreach (KeyValuePair<OdbPid, List<int>> pidsData in ecuData.Value)
                {
                    EcuModes modes = new EcuModes();
                    modes.Mode = pidsData.Key;
                    modes.SupportedPids = pidsData.Value;

                    ecu.CountOfPidsSupported += pidsData.Value.Count;
                    ecu.Modes.Add(modes);
                }
                ecus.Add(ecu);
            }
            return ecus;
        }

        /// <summary>
        /// Check if pid is supported for ecu
        /// </summary>
        /// <param name="ecu"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Boolean IsPidSupportedForEcu(Ecu ecu, OdbPid pid)
        {
            if (supportedPids.ContainsKey(ecu.EcuId))
            {
                var modes = supportedPids[ecu.EcuId];
                if (modes.ContainsKey(OdbPids.ATSP0))
                {
                    var pids = modes[OdbPids.ATSP0];
                    return pids.Contains(pid.GetPidIdInDecimal());
                }
                if (modes.ContainsKey(pid.Mode))
                {
                    var pids = modes[pid.Mode];
                    return pids.Contains(pid.GetPidIdInDecimal());
                }
            }
            return false;
        }

        #endregion




        /// <summary>
        /// Register supported pids
        /// </summary>
        /// <param name="ecuIdentifier"></param>
        /// <param name="odbPid"></param>
        /// <param name="pids"></param>
        private void RegisterSupportedPids(int ecuIdentifier, OdbPid odbPid, List<int> pids)
        {
            if (!supportedPids.ContainsKey(ecuIdentifier))
            {
                supportedPids.Add(ecuIdentifier, new Dictionary<OdbPid, List<int>>());
            }

            Dictionary<OdbPid, List<int>> supportedPidsForEcu = supportedPids[ecuIdentifier];
            if (supportedPidsForEcu.ContainsKey(odbPid))
            {
                var pidsSupported = supportedPidsForEcu[odbPid];
                supportedPidsForEcu[odbPid] = pidsSupported.Concat(pids).ToList();
            }
            else
            {
                supportedPidsForEcu.Add(odbPid, pids);
            }
        }
    }
}
