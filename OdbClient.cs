using OdbCommunicator.OdbCommon;
using OdbCommunicator.OdbEventArg;
using OdbCommunicator.OdbExceptions;
using OdbCommunicator.OdbCheck;
using OdbCommunicator.OdbQueries;
using OdbCommunicator.OdbSockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace OdbCommunicator
{
    public class OdbClient
    {
        public static int OBD_REFRESH_RATE = 5;
        public static bool OBD_REPORTER_ENABLED = true;

        #region EVENTS

        public event EventHandler<OdbEventArgs> DataReceive;

        #endregion

        #region PRIVATES

        private Boolean? isOdb = null;

        private OdbSocket socket = null;
        private OdbStatus status = null;
        private OdbCommands commands = null;
        private OdbEcu ecu = null;
        private OdbReporter reporter = new OdbReporter();

        private DispatcherTimer poller;
        private Dictionary<OdbPid, OdbQuery> queryResponses = new Dictionary<OdbPid, OdbQuery>();

        /// <summary>
        /// Priorities array
        /// </summary>
        private OdbPriority[] Priorities = new OdbPriority[] { 
            OdbPriority.VeryHigh, OdbPriority.High, OdbPriority.VeryHigh, OdbPriority.High, OdbPriority.VeryHigh, OdbPriority.Medium,
            OdbPriority.VeryHigh, OdbPriority.High, OdbPriority.VeryHigh, OdbPriority.High, OdbPriority.VeryHigh, OdbPriority.Medium,
            OdbPriority.VeryHigh, OdbPriority.Small 
        };
        private System.Collections.IEnumerator PriorityEnumerator;

        #endregion

        #region PROPERTY

        /// <summary>
        /// Check if is connected
        /// </summary>
        public bool IsConnected
        {
            get 
            {
                return this.socket.IsConnected;
            }
        }

        #endregion

        /// <summary>
        /// Create new odb client
        /// </summary>
        public OdbClient()
        {

            this.PriorityEnumerator = Priorities.GetEnumerator();
            this.socket = new OdbSocket(new StreamSocket());

            this.status = new OdbStatus(this.socket);
            this.commands = new OdbCommands(this.socket);
            this.ecu = new OdbEcu(this.socket, this.commands);
        }

        #region QUERIES

        /// <summary>
        /// Get data for pid
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public OdbQueryResponse RequestFor(OdbPid pid)
        {
            //add new query to databaze
            if (!queryResponses.ContainsKey(pid))
            {
                OdbQuery newQuery = new OdbQuery();
                newQuery.Pid = pid;
                newQuery.Status = this.checkSupported(newQuery);
                queryResponses.Add(pid, newQuery);

                reporter.ReportNewQuery(newQuery);
            }

            //read data from query
            OdbQuery query = queryResponses[pid];
            OdbQueryResponse response = new OdbQueryResponse();
            if (query.Status == QueryStatus.Complete)
            {
                response.Data = query.Data;
                response.Unit = query.Pid.Units;
                response.MinValue = query.Pid.MinValue;
                response.MaxValue = query.Pid.MaxValue;

                return response;
            }
            return null;
        }

        /// <summary>
        /// Get data for pid
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public void UnregisterQuery(OdbPid pid)
        {
            //remove query from databaze
            if (queryResponses.ContainsKey(pid))
            {
                queryResponses.Remove(pid);
                reporter.ReportDeleteQuery(pid);
            }
        }

        #endregion

        #region PUBLIC

        /// <summary>
        /// Check if is odb device
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsOdb()
        {
            if (!isOdb.HasValue)
            {
                isOdb = await status.IsOdb();
            }
            return isOdb.Value;
        }

        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public async Task Connect(HostName hostname, String service = "{00001101-0000-1000-8000-00805f9b34fb}")
        {
            await this.socket.Connect(hostname, service);

            if (await this.IsOdb())
            {
                //load supported pids from odb device
                await this.commands.LoadSupportedPids();

                //load ecu by supported pids
                this.ecu.LoadCurrentEcu();

                //create poll for reading data from ODB periodicaly
                this.createPoller();
            }
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {
            if (this.IsConnected)
            {
                if (this.poller != null)
                {
                    this.poller.Stop();
                    this.poller = null;
                }
                //disconnect socket
                this.socket.Disconnect();
            }
        }

        /// <summary>
        /// Start reading data from device
        /// </summary>
        public void Start()
        {
            if (!this.IsConnected)
            {
                throw new OdbException(OdbError.DeviceIsNotConnected);
            }

            //trigger data receive
            triggerOnDataReceive();

            this.poller.Start();
            reporter.ReportInfo("Start poller for obtaining data from device.");
        }

        /// <summary>
        /// Stop reading data from device
        /// </summary>
        public void Stop()
        {
            this.poller.Stop();
            this.poller = null;
            reporter.ReportInfo("Stop poller and reading data from device.");
        }

        #endregion

        #region PRIVATE

        /// <summary>
        /// Create pooler for data reading
        /// </summary>
        private void createPoller()
        {
            if (poller != null) 
            {
                return;
            }

            poller = new DispatcherTimer();
            poller.Interval = TimeSpan.FromMilliseconds(OBD_REFRESH_RATE);
            poller.Tick += onPollerRequest;

            //trigger data receive
            triggerOnDataReceive();
        }

        /// <summary>
        /// On poller request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void onPollerRequest(object sender, EventArgs e)
        {
            if (queryResponses.Count == 0)
            {
                return;
            }

            //pooler stop
            poller.Stop();

            //get priority
            var currentPriority = getCurrentPriority();

            //iterate
            foreach (var q in queryResponses)
            {
                OdbQuery query = q.Value;
                if (query.Status != QueryStatus.NotSupported && query.Pid.Priority == currentPriority)
                {
                    OdbResponse response;
                    try
                    {
                        response = await this.socket.SendAndCheck(query.Pid);
                    }
                    catch (OdbException)
                    {
                        response = null;
                    }
                    if (response != null && response.IsValid)
                    {
                        OdbData data = this.socket.ResolveData(response, query.Pid);
                        if (data != null)
                        {
                            query.Status = QueryStatus.Complete;
                            query.Data = this.parseDataForSpecifiedPid(query.Pid, data);
                        }
                        else
                        {
                            query.Status = QueryStatus.NotSupported;
                        }
                    }
                    else
                    {
                        query.Status = QueryStatus.NotSupported;
                    }
                }


                //trigger data receive
                triggerOnDataReceive();
            }

            if (this.IsConnected)
            {
                poller.Start();
            }
        }

        /// <summary>
        /// Get current priority
        /// </summary>
        /// <returns></returns>
        private OdbPriority getCurrentPriority()
        {
            if (!PriorityEnumerator.MoveNext())
            {
                PriorityEnumerator.Reset();
                PriorityEnumerator.MoveNext();
            }
            return (OdbPriority)PriorityEnumerator.Current;
        }

        /// <summary>
        /// Check if query is supported
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private QueryStatus checkSupported(OdbQuery query)
        {
            if (this.commands.IsPidSupportedForEcu(this.ecu.Current, query.Pid))
            {
                return QueryStatus.NoData;
            }
            return QueryStatus.NotSupported;
        }

        /// <summary>
        /// Parse data for specified pid
        /// </summary>
        /// <param name="odbPid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private Double parseDataForSpecifiedPid(OdbPid odbPid, OdbData data)
        {
            int A = -1, B = -1, C = -1, D = -1;
            int length = data.Data.Length;

            if (length > 4 || length == 0) 
            {
                throw new OdbException(OdbError.IncorrectDataLength);
            }

            if (length >= 4) 
            {
                D = Convert.ToInt32(data.Data[3], 16);
            }
            if (length >= 3) 
            {
                C = Convert.ToInt32(data.Data[2], 16);
            }
            if (length >= 2) 
            {
                B = Convert.ToInt32(data.Data[1], 16);
            }
            if (length >= 1) 
            {
                A = Convert.ToInt32(data.Data[0], 16);
            }

            return odbPid.Compute(A, B, C, D);
        }

        /// <summary>
        /// Trigger on data receive
        /// </summary>
        private void triggerOnDataReceive()
        {
            if (this.DataReceive != null)
            {
                OdbEventArgs args = new OdbEventArgs();
                args.Client = this;

                this.DataReceive.Invoke(this, args);
            }
        }

        #endregion

    }
}
