using OdbCommunicator.OdbCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdbCommunicator.OdbExceptions
{
    public class OdbException : Exception
    {
        private String message;
        private OdbError type;
        private DateTime time;
        private Int32 code;

        /// <summary>
        /// Type
        /// </summary>
        public OdbError Type 
        {
            get 
            {
                return type;
            }
        }

        private OdbReporter reporter = new OdbReporter();

        /// <summary>
        /// Odb Exception
        /// </summary>
        /// <param name="code"></param>
        /// <param name="time"></param>
        public OdbException(OdbError type) : base(type.ToString())
        {
            this.type = type;
            this.time = DateTime.Now;
            this.message = this.getMessageByErrorType(type);
            this.code = this.getCodeByErrorType(type);

            reporter.ReportError(this.message, this.code.ToString("X"));
        }

        /// <summary>
        /// Get number by code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int getCodeByErrorType(OdbError type)
        {
            switch (type)
            {
                case OdbError.AlreadyConnectedToDevice:
                    return 0x001;
                case OdbError.CouldNotFindCompatibleProtocol:
                    return 0x002;
                case OdbError.DeviceIsNotConnected:
                    return 0x003;
                case OdbError.DeviceIsNotOdbCompatible:
                    return 0x004;
                case OdbError.WrongProtocolNumber:
                    return 0x005;
                case OdbError.WrongResponseFromDevice:
                    return 0x006;
                case OdbError.IncorrectDataLength:
                    return 0x007;
                default:
                    return 0x000;
            }
        }

        /// <summary>
        /// Get message by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string getMessageByErrorType(OdbError type)
        {
            switch (type)
            {
                case OdbError.AlreadyConnectedToDevice:
                    return "Device is already connected.";
                case OdbError.CouldNotFindCompatibleProtocol:
                    return "Could not find compatible protocol. Ensure the cable is securely connected to the OBD port on the vehicle.";
                case OdbError.DeviceIsNotConnected:
                    return "Device is not connected.";
                case OdbError.DeviceIsNotOdbCompatible:
                    return "Device is not ODB compatible.";
                case OdbError.WrongProtocolNumber:
                    return "You must specified protocolo number from 1 to 9 if use Specified protocol type.";
                case OdbError.WrongResponseFromDevice:
                    return "Another response than expected from device.";
                case OdbError.IncorrectDataLength:
                    return "Data provided has wrong length.";
                default:
                    return "Unexpected error occured.";
            }
        }
    }
}
