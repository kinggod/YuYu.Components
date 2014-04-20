using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YuYu.Components
{
    /// <summary>
    /// JPushHandler
    /// </summary>
    public class JPushHandler
    {
        /// <summary>
        /// 推送追踪周期
        /// </summary>
        public const int PUSHTRACKINGLIFETIME = 10 * 24 * 60; //10 days. Defined by JPush official.

        /// <summary>
        /// JPushClient
        /// </summary>
        public JPushClient JPushClient { get; protected set; }

        /// <summary>
        /// 推送追踪列表
        /// </summary>
        public IList<PushTracking> PushTrackings { get; protected set; }

        /// <summary>
        /// 数据锁
        /// </summary>
        public object DataLocker { get; protected set; }

        /// <summary>
        /// 线程间隔时间（秒）
        /// </summary>
        public int ThreadInterval { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public UpdatePushResult PushResultUpdate { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public InitializePushTracking PushTrackingInitialize { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public ReportException ExceptionReport { get; protected set; }

        /// <summary>
        /// 监控器
        /// </summary>
        public Thread Monitor { get; protected set; }

        /// <summary>
        /// 监控功能
        /// </summary>
        protected void MonitorFunction()
        {
            while (true)
            {
                try
                {
                    List<string> ids = new List<string>();
                    lock (DataLocker)
                    {
                        DateTime nowUtcTime = DateTime.UtcNow;
                        for (int i = 0; i < this.PushTrackings.Count; )
                        {
                            PushTracking pushTracking = this.PushTrackings[i];
                            if ((nowUtcTime - pushTracking.UtcTimestamp).TotalMinutes > PUSHTRACKINGLIFETIME)
                                this.PushTrackings.RemoveAt(i);
                            else
                            {
                                ids.Add(pushTracking.MessageID);
                                i++;
                            }
                        }
                    }
                    List<PushResult> results = new List<PushResult>();
                    while (ids.Count > 0)
                    {
                        List<string> listToOperate = new List<string>();
                        listToOperate.AddRange(ids.Take(ids.Count > 100 ? 100 : ids.Count));
                        IList<PushResult> queryResult = JPushClient.QueryPushResult(listToOperate);
                        if (queryResult != null)
                            results.AddRange(queryResult);
                        ids.RemoveRange(0, listToOperate.Count);
                    }
                    if (this.PushResultUpdate != null)
                        this.PushResultUpdate.Invoke(results);
                }
                catch (Exception e)
                {
                    this.HandleException(new Exception("Failed to run monitor work.", e));
                }
                Thread.Sleep(this.ThreadInterval * 1000);
            }
        }

        /// <summary>
        /// Initializes the background thread.
        /// </summary>
        protected void InitializeMonitorThreadInBackground()
        {
            this.Monitor = new Thread(new ThreadStart(this.MonitorFunction));
            this.Monitor.IsBackground = true;
            DateTime fromTime = DateTime.UtcNow.AddDays(-10);
            try
            {
                if (this.PushTrackingInitialize != null)
                    this.PushTrackingInitialize(fromTime);
            }
            catch (Exception e)
            {
                HandleException(new InvalidOperationException("Failed to initialize message tracking list from UTC time: " + fromTime.ToString("yyyy/MM/dd+HH:mm:ss.fff"), e));
            }
            this.Monitor.Start();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JPushClient" /> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="masterSecret">The master secret.</param>
        /// <param name="pushResultUpdate">The update push message status delegate.</param>
        /// <param name="pushTrackingInitialize">The initialize delegate.</param>
        /// <param name="exceptionReport">The exception delegate.</param>
        /// <param name="threadInterval">The interval in second.</param>
        public JPushHandler(string appKey, string masterSecret, int threadInterval = 60, UpdatePushResult pushResultUpdate = null, InitializePushTracking pushTrackingInitialize = null, ReportException exceptionReport = null)
        {
            JPushClient = new JPushClient(appKey, masterSecret, true);
            if (threadInterval < 1)
                threadInterval = 60;
            this.ThreadInterval = threadInterval;
            this.PushTrackings = new List<PushTracking>();
            this.DataLocker = new object();
            this.PushResultUpdate = pushResultUpdate;
            this.PushTrackingInitialize = pushTrackingInitialize;
            this.ExceptionReport = exceptionReport;
            this.InitializeMonitorThreadInBackground();
        }

        /// <summary>
        /// Send the message push.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PushResponse SendPush(PushRequest request)
        {
            try
            {
                PushResponse response = JPushClient.SendPush(request);
                if (response != null && !string.IsNullOrWhiteSpace(response.MessageID))
                    this.AddMessageTrackingID(new PushTracking
                    {
                        MessageID = response.MessageID,
                        UtcTimestamp = DateTime.UtcNow
                    });
                return response;
            }
            catch (Exception e)
            {
                this.HandleException(new Exception("Failed to send push message", e));
            }
            return null;
        }

        /// <summary>
        /// Adds the message tracking identifier.
        /// </summary>
        /// <param name="messageTracking">The message tracking.</param>
        public void AddMessageTrackingID(PushTracking messageTracking)
        {
            lock (DataLocker)
            {
                this.AddPushTrackingByWithoutLocker(messageTracking);
            }
        }

        /// <summary>
        /// Removes the message tracking.
        /// </summary>
        /// <param name="messageID"></param>
        public void RemoveMessageTracking(string messageID)
        {
            lock (DataLocker)
            {
                this.RemovePushTrackingWithoutLocker(messageID);
            }
        }

        /// <summary>
        /// Gets the message tracking identifier.
        /// </summary>
        public IList<string> MessageTrackingID
        {
            get
            {
                IList<string> result = new List<string>();
                lock (DataLocker)
                {
                    foreach (PushTracking pushTracking in this.PushTrackings)
                        result.Add(pushTracking.MessageID);
                }
                return result;
            }
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="exception"></param>
        protected void HandleException(Exception exception)
        {
            if (exception != null && this.ExceptionReport != null)
                this.ExceptionReport(exception);
        }

        /// <summary>
        /// Finds the message tracking by identifier.
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected PushTracking FindPushTrackingByMessageIDWithoutLocker(string messageID, out int index)
        {
            index = -1;
            if (!string.IsNullOrWhiteSpace(messageID))
                foreach (PushTracking temp in this.PushTrackings)
                {
                    index++;
                    if (temp.MessageID == messageID)
                        return temp;
                }
            return null;
        }

        /// <summary>
        /// Adds the message tracking identifier without locker.
        /// </summary>
        /// <param name="pushTracking"></param>
        protected void AddPushTrackingByWithoutLocker(PushTracking pushTracking)
        {
            if (pushTracking != null && !string.IsNullOrWhiteSpace(pushTracking.MessageID))
            {
                int index = 0;
                if (FindPushTrackingByMessageIDWithoutLocker(pushTracking.MessageID, out index) == null)
                    this.PushTrackings.Add(pushTracking);
            }
        }

        /// <summary>
        /// Removes the message tracking without locker.
        /// </summary>
        /// <param name="messageID"></param>
        protected void RemovePushTrackingWithoutLocker(string messageID)
        {
            if (!string.IsNullOrWhiteSpace(messageID))
            {
                int index = 0;
                PushTracking pushTracking = FindPushTrackingByMessageIDWithoutLocker(messageID, out index);
                if (pushTracking != null && index >= 0)
                    this.PushTrackings.RemoveAt(index);
            }
        }
    }
}
