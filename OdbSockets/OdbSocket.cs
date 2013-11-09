using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using OdbCommunicator.OdbCommon;
using System.IO;
using System.Diagnostics;
using OdbCommunicator.OdbExceptions;

namespace OdbCommunicator.OdbSockets
{
    public class OdbSocket
    {
        public const int BUFFER_STEP = 100;
        public const int RESPONSE_TRY_COUNT = 3;

        /// <summary>
        /// Check if is connected
        /// </summary>
        private bool isConnected = false;
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            private set
            {
                isConnected = value;
            }
        }


        /// <summary>
        /// Selected protocol
        /// </summary>
        private int selectedProtocol = -1;
        public int SelectedProtocol
        {
            get
            {
                return selectedProtocol;
            }
            set
            {
                selectedProtocol = value;
            }
        }

        private StreamSocket socket;
        private DataWriter writer;
        private DataReader reader;
        private Int32 tryCount = RESPONSE_TRY_COUNT;
        private OdbReporter reporter = new OdbReporter();

        /// <summary>
        /// Create new Odb Scoket
        /// </summary>
        /// <param name="socket"></param>
        public OdbSocket(StreamSocket socket)
        {
            this.socket = socket;
            this.socket.Control.KeepAlive = true;

            this.writer = new DataWriter(this.socket.OutputStream);
            this.reader = new DataReader(this.socket.InputStream);

            this.reader.InputStreamOptions = InputStreamOptions.Partial;
        }

        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public async Task Connect(HostName hostname, String service)
        {
            if (this.IsConnected)
            {
                throw new OdbException(OdbError.AlreadyConnectedToDevice);
            }

            try
            {
                await this.socket.ConnectAsync(hostname, service);
                this.IsConnected = true;
                reporter.ReportInfo(String.Format("Connected to device '{0}' on service port '{1}'", hostname.DisplayName, service));
            }
            catch
            {
                this.IsConnected = false;
            }
        }

        /// <summary>
        /// Disconnect socket
        /// </summary>
        public void Disconnect()
        {
            if (this.IsConnected)
            {
                reporter.ReportInfo(String.Format("Disconnect from device '{0}'", this.socket.Information.RemoteHostName.DisplayName));
                this.socket.Dispose();
                this.IsConnected = false;
            }
        }

        /// <summary>
        /// Send message, return bytes response
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        public async Task<OdbResponse> Send(OdbPid what)
        {
            if (!this.IsConnected)
            {
                throw new OdbException(OdbError.DeviceIsNotConnected);
            }

            //timer
            DateTime start = DateTime.Now;

            //send
            writer.WriteString(what.Pid + "\r");
            await writer.StoreAsync();
            await writer.FlushAsync();

            //receive data from device
            OdbResponse odbResponse = await receiveDataFromDevice(what, start);

            //try again on error reponse
            while (!odbResponse.IsValid && this.tryCount > 0)
            {
                reporter.ReportInfo("Incorrect response from device. Try another request. Current try step is " + this.tryCount + ".");
                odbResponse = await receiveDataFromDevice(what, start);
                this.tryCount--;
            }
            this.tryCount = RESPONSE_TRY_COUNT;

            //report response to console
            reporter.ReportResponse(odbResponse);

            return odbResponse;
        }

        /// <summary>
        /// Receive data from device
        /// </summary>
        /// <param name="what"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private async Task<OdbResponse> receiveDataFromDevice(OdbPid what, DateTime start)
        {            
            //await for response
            await Task.Delay(this.getReponseByPid(what));

            //create response
            OdbResponse odbResponse = new OdbResponse();
            odbResponse.Pid = what;

            try
            {
                //reader
                uint loaded = await reader.LoadAsync(BUFFER_STEP);

                String response = reader.ReadString(reader.UnconsumedBufferLength);
                while (loaded == BUFFER_STEP)
                {
                    loaded = await reader.LoadAsync(BUFFER_STEP);
                    response += reader.ReadString(reader.UnconsumedBufferLength);
                }
                odbResponse.Response = this.clearResponse(response, what);
                odbResponse.IsValid = response.Trim().Length > 0;
            }
            catch
            {
                odbResponse.Response = "";
                odbResponse.IsValid = false;
            }

            odbResponse.Time = DateTime.Now.Subtract(start);

            return odbResponse;
        }

        /// <summary>
        /// Send message and check response
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        public async Task<OdbResponse> SendAndCheck(OdbPid what)
        {
            OdbResponse response = await this.Send(what);
            if (!response.Response.Contains(what.ExpectedResponse))
            {
                throw new OdbException(OdbError.WrongResponseFromDevice);
            }
            return response;
        }

        /// <summary>
        /// Resolve incoming data and setup odb data
        /// </summary>
        /// <param name="response"></param>
        /// <param name="what"></param>
        /// <returns></returns>
        public OdbData ResolveData(OdbResponse response, OdbPid what)
        {
            int counter = 0;
            String[] bytes = response.Response.Split(' ');
            OdbData data = OdbPids.GetResponseFormatForProtocolNumber(this.SelectedProtocol, what.ByteCount);

            if (bytes.Length < what.ByteCount)
            {
                return null;
            }

            try
            {
                data.Protocol = OdbPids.GetPidForProtocolNumber(this.SelectedProtocol);
                for (int i = 0; i < data.Header.Length; i++)
                {
                    data.Header[i] = bytes[counter];
                    counter++;
                }
                for (int i = 0; i < data.Info.Length; i++)
                {
                    data.Info[i] = bytes[counter];
                    counter++;
                }
                for (int i = 0; i < data.Data.Length; i++)
                {
                    data.Data[i] = bytes[counter];
                    counter++;
                }
                for (int i = 0; i < data.Ender.Length; i++)
                {
                    data.Ender[i] = bytes[counter];
                    counter++;
                }
            }
            catch
            {
                return null;
            }
            return data;
        }



        /// <summary>
        /// Clear response message
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string clearResponse(string response, OdbPid what)
        {
            response = response.Replace("\r", " ");

            String[] lines = response.Split('\n');
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                String line = lines[i];
                line = line.Replace("\n", " ");
                line = line.Replace(">", "");
                line = line.Replace(what.Pid, "");
                line = line.Trim();

                if (line.Length > 0)
                {
                    return line;
                }
            }
            return lines.Last().Trim();
        }


        /// <summary>
        /// Get response time what for pid
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        private int getReponseByPid(OdbPid what)
        {
            if (what.IsElmCommand)
            {
                return 250;
            }
            return 150;
        }

    }
}
