using OdbCommunicator.OdbCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator
{
    public static class OdbPids
    {
        /// <summary>
        /// Get pid for Protocol number
        /// </summary>
        /// <param name="protocolNumber"></param>
        /// <returns></returns>
        public static OdbPid GetPidForProtocolNumber(int protocolNumber)
        {
            switch (protocolNumber)
            {
                case 1:
                    return OdbPids.ATSP1;
                case 2:
                    return OdbPids.ATSP2;
                case 3:
                    return OdbPids.ATSP3;
                case 4:
                    return OdbPids.ATSP4;
                case 5:
                    return OdbPids.ATSP5;
                case 6:
                    return OdbPids.ATSP6;
                case 7:
                    return OdbPids.ATSP7;
                case 8:
                    return OdbPids.ATSP8;
                case 9:
                    return OdbPids.ATSP9;
                default:
                    return OdbPids.ATSP0;
            }
        }

        /// <summary>
        /// get response formater
        /// </summary>
        /// <param name="protocolNumber"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public static OdbData GetResponseFormatForProtocolNumber(int protocolNumber, int dataLength)
        {
            OdbData data = new OdbData();
            switch (protocolNumber)
            {
                case 1:
                    data.Header = new string[3];
                    data.Info = new string[2];
                    data.Data = new string[dataLength];
                    data.Ender = new string[1];
                    data.DataLength = dataLength + 6;
                    break;
                default:
                    data.Header = new string[0];
                    data.Info = new string[0];
                    data.Data = new string[0];
                    data.Ender = new string[0];
                    data.DataLength = 0;
                    break;
            }
            return data;
        }




        #region ELM COMMANDS

        //ELM command
        public static readonly OdbPid ATZ = new OdbPid()
        {
            Pid = "ATZ",
            ExpectedResponse = "ELM327",
            Description = "Reset ELM327 device",
            IsElmCommand = true
        };
        public static readonly OdbPid ATE0 = new OdbPid()
        {
            Pid = "ATE0",
            ExpectedResponse = "OK",
            Description = "Disable echo on device",
            IsElmCommand = true
        };
        public static readonly OdbPid ATL0 = new OdbPid()
        {
            Pid = "ATL0",
            ExpectedResponse = "OK",
            Description = "Disable lines feed",
            IsElmCommand = true
        };
        public static readonly OdbPid ATH1 = new OdbPid()
        {
            Pid = "ATH1",
            ExpectedResponse = "OK",
            Description = "Headers enable",
            IsElmCommand = true
        };

        #endregion

        #region MODES

        //protocols
        public static readonly OdbPid ATSP0 = new OdbPid()
        {
            Pid = "ATSP0",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP0",
            IsElmCommand = true
        };
        public static readonly OdbPid ATSP1 = new OdbPid()
        {
            Pid = "ATSP1",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP1",
            IsElmCommand = true,
            PidPrefix = "01"
        };
        public static readonly OdbPid ATSP2 = new OdbPid()
        {
            Pid = "ATSP2",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP2",
            IsElmCommand = true,
            PidPrefix = "02"
        };
        public static readonly OdbPid ATSP3 = new OdbPid()
        {
            Pid = "ATSP3",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP3",
            IsElmCommand = true,
            PidPrefix = "03"
        };
        public static readonly OdbPid ATSP4 = new OdbPid()
        {
            Pid = "ATSP4",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP4",
            IsElmCommand = true,
            PidPrefix = "04"
        };
        public static readonly OdbPid ATSP5 = new OdbPid()
        {
            Pid = "ATSP5",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP5",
            IsElmCommand = true,
            PidPrefix = "05"
        };
        public static readonly OdbPid ATSP6 = new OdbPid()
        {
            Pid = "ATSP6",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP6",
            IsElmCommand = true,
            PidPrefix = "06"
        };
        public static readonly OdbPid ATSP7 = new OdbPid()
        {
            Pid = "ATSP7",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP7",
            IsElmCommand = true,
            PidPrefix = "07"
        };
        public static readonly OdbPid ATSP8 = new OdbPid()
        {
            Pid = "ATSP8",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP8",
            IsElmCommand = true,
            PidPrefix = "08"
        };
        public static readonly OdbPid ATSP9 = new OdbPid()
        {
            Pid = "ATSP9",
            ExpectedResponse = "OK",
            Description = "Protocol ATSP9",
            IsElmCommand = true,
            PidPrefix = "09"
        };

        #endregion

        #region MODE 1 CHECKERS

        //ODB PIDS supproted commands
        public static readonly OdbPid Mode1_PidsSupported20 = new OdbPid()
        {
            Pid = "00",
            ExpectedResponse = "",
            Description = "PIDs supported [01 - 20]",
            ByteCount = 4,
            Mode = OdbPids.ATSP1
        };
        public static readonly OdbPid Mode1_PidsSupported40 = new OdbPid()
        {
            Pid = "20",
            ExpectedResponse = "",
            Description = "PIDs supported [21 - 40]",
            ByteCount = 4,
            Mode = OdbPids.ATSP1
        };
        public static readonly OdbPid Mode1_PidsSupported60 = new OdbPid()
        {
            Pid = "40",
            ExpectedResponse = "",
            Description = "PIDs supported [41 - 60]",
            ByteCount = 4,
            Mode = OdbPids.ATSP1
        };
        public static readonly OdbPid Mode1_PidsSupported80 = new OdbPid()
        {
            Pid = "60",
            ExpectedResponse = "",
            Description = "PIDs supported [61 - 80]",
            ByteCount = 4,
            Mode = OdbPids.ATSP1
        };

        #endregion

        #region MODE 1 PIDS

        public static readonly OdbPid Mode1_EngineCoolantTemperature = new OdbPid()
        {
            Pid = "05",
            ExpectedResponse = "",
            Description = "Engine coolant temperature",
            ByteCount = 1,
            MinValue = -40,
            MaxValue = 215,
            Units = "°C",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.Small,
            Compute = (A, B, C, D) =>
            {
                return A - 40;
            }
        };

        public static readonly OdbPid Mode1_EngineRpm = new OdbPid()
        {
            Pid = "0C",
            ExpectedResponse = "",
            Description = "Engine RPM",
            ByteCount = 2,
            MinValue = 0,
            MaxValue = 16383.75,
            Units = "RPM",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.VeryHigh,
            Compute = (A, B, C, D) => {
                return ((A * 256) + B) / 4;
            }
        };

        public static readonly OdbPid Mode1_VehicleSpeed = new OdbPid()
        {
            Pid = "0D",
            ExpectedResponse = "",
            Description = "Vehicle speed",
            ByteCount = 1,
            MinValue = 0,
            MaxValue = 255,
            Units = "km/h",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.VeryHigh,
            Compute = (A, B, C, D) =>
            {
                return A;
            }
        };

        public static OdbPid Mode1_EngineCalculatedLoad = new OdbPid()
        {
            Pid = "04",
            ExpectedResponse = "",
            Description = "Calculated engine load value",
            ByteCount = 1,
            MinValue = 0,
            MaxValue = 100,
            Units = "%",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.High,
            Compute = (A, B, C, D) =>
            {
                return (A * 100) / 255;
            }
        };

        public static OdbPid Mode1_MafAirFlowRate = new OdbPid()
        {
            Pid = "10",
            ExpectedResponse = "",
            Description = "MAF air flow rate",
            ByteCount = 2,
            MinValue = 0,
            MaxValue = 655.35,
            Units = "grams/sec",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.Medium,
            Compute = (A, B, C, D) =>
            {
                return ((A * 256) + B) / 100;
            }
        };

        public static OdbPid Mode1_ThrottlePosition = new OdbPid()
        {
            Pid = "11",
            ExpectedResponse = "",
            Description = "Throttle position",
            ByteCount = 1,
            MinValue = 0,
            MaxValue = 100,
            Units = "%",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.High,
            Compute = (A, B, C, D) =>
            {
                return (A * 100) / 255;
            }
        };

        public static OdbPid Mode1_RunTimeSinceEngineStart = new OdbPid()
        {
            Pid = "1F",
            ExpectedResponse = "",
            Description = "Run time since engine start",
            ByteCount = 2,
            MinValue = 0,
            MaxValue = 65535,
            Units = "seconds",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.Small,
            Compute = (A, B, C, D) =>
            {
                return (A * 256) + B;
            }
        };

        public static OdbPid Mode1_RelativeAcceleratorPedalPosition = new OdbPid()
        {
            Pid = "5A",
            ExpectedResponse = "",
            Description = "Relative accelerator pedal position",
            ByteCount = 1,
            MinValue = 0,
            MaxValue = 100,
            Units = "%",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.High,
            Compute = (A, B, C, D) =>
            {
                return (A * 100) / 255;
            }
        };

        public static OdbPid Mode1_EngineOilTemperature = new OdbPid()
        {
            Pid = "5C",
            ExpectedResponse = "",
            Description = "Engine oil temperature",
            ByteCount = 2,
            MinValue = -40,
            MaxValue = 210,
            Units = "°C",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.Small,
            Compute = (A, B, C, D) =>
            {
                return A - 40;
            }
        };

        public static OdbPid Mode1_FuelInjectionTiming = new OdbPid()
        {
            Pid = "5D",
            ExpectedResponse = "",
            Description = "Fuel injection timing",
            ByteCount = 2,
            MinValue = -210,
            MaxValue = 301.992,
            Units = "°",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.Medium,
            Compute = (A, B, C, D) =>
            {
                return (((A*256)+B)-26.880)/128;
            }
        };

        public static OdbPid Mode1_EngineReferenceTorque = new OdbPid()
        {
            Pid = "63",
            ExpectedResponse = "",
            Description = "Engine reference torque",
            ByteCount = 2,
            MinValue = 0,
            MaxValue = 65535,
            Units = "Nm",
            Mode = OdbPids.ATSP1,
            Priority = OdbPriority.High,
            Compute = (A, B, C, D) =>
            {
                return A * 256 + B;
            }
        };

        #endregion
    }
}
