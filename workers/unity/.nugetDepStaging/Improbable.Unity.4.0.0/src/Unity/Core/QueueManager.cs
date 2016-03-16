using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Improbable.Core.Serialization;
using Improbable.Fapi.Receptionist;
using log4net;
using Newtonsoft.Json;
using UnityEngine;

namespace Improbable.Unity.Core
{
    internal class QueueManager : MonoBehaviour
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(QueueManager));

        private string loginToken;
        private Action<IQueueStatus> onQueueStatusUpdate;
        private Action<string> onQueueingComplete;
        private string queueingApiUrl;
        private string queueingToken;

        /// <summary>
        ///     The <code>QueueManager</code> handles the interaction with the infrastrucutures queuing REST api.
        ///     For a given login token this class will ensure the user is added to the queue, and notifed view
        ///     the <code>BootstrapHandler</code> callbacks of progress through the queue.
        /// </summary>
        /// <param name="loginToken">of the client to queue</param>
        /// <param name="apiUrl">of the infrastrucuture queuing API</param>
        /// <param name="onStart">called to indicate to the user that they are being qeued</param>
        /// <param name="onStatusUpdate">called when the clients queue status updates</param>
        /// <param name="onComplete">the queue is finished and the client can connect to the Fabric</param>
        public void StartQueueing(string loginToken,
                                  string apiUrl,
                                  Action onStart,
                                  Action<IQueueStatus> onStatusUpdate,
                                  Action<string> onComplete)
        {
            this.loginToken = loginToken;
            queueingApiUrl = apiUrl;
            onQueueStatusUpdate = onStatusUpdate;
            onQueueingComplete = onComplete;

            onStart();
            StartCoroutine(TryIn(TimeSpan.Zero, () => StartCoroutine(QueryQueueingStatus(Logger.Error))));
        }

        private static IEnumerator TryIn(TimeSpan timeToWait, Action onComplete)
        {
            yield return new WaitForSeconds(Convert.ToSingle(timeToWait.TotalSeconds));
            onComplete();
        }

        private IEnumerator QueryQueueingStatus(Action<Exception> onError)
        {
            var bytes = MakeRequestBody();
            var headers = MakeRequestHeaders(bytes);

            Logger.Debug("Making queuing request to " + queueingApiUrl + " with bytes=" + bytes.Length + " and headers=" + headers["Content-Length"]);

            var www = new WWW(queueingApiUrl, bytes, headers);
            yield return www;


            if (www.error != null)
            {
                Logger.Error(String.Format("Queueing request failed Error({0}) and Body({1})", www.error, www.text));
                onError(new ApplicationException(www.error));
            }
            else
            {
                IQueueStatus queueStatus;
                if (TryDeserializeResponse(www, out queueStatus))
                {
                    ProcessQueueResponse(onError, queueStatus);
                }
                else
                {
                    onError(new Exception("Failed to deserialize QueueStatus response"));
                }
            }
        }

        private Dictionary<string, string> MakeRequestHeaders(byte[] bytes)
        {
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + loginToken }
            };

            if (string.IsNullOrEmpty(queueingToken))
            {
                headers.Add("Content-Length", "0");
            }
            else
            {
                headers.Add("Content-Type", "application/json");
                headers.Add("Content-Length", bytes.Length.ToString());
            }

            return headers;
        }

        private void ProcessQueueResponse(Action<Exception> onError, IQueueStatus queueStatus)
        {
            switch (queueStatus.Status)
            {
                case "retry":
                    onQueueStatusUpdate(queueStatus);
                    // update current queuing token
                    queueingToken = queueStatus.QueueToken;
                    StartCoroutine(TryIn(queueStatus.RetryAfter, () => StartCoroutine(QueryQueueingStatus(onError))));
                    break;
                case "login":
                    onQueueingComplete(queueStatus.QueueToken);
                    break;
                default:
                    Logger.Error("Queue Status unknown: " + queueStatus.Status);
                    break;
            }
        }

        private byte[] MakeRequestBody()
        {
            byte[] bytes;
            if (!String.IsNullOrEmpty(queueingToken))
            {
                var body = new Dictionary<string, string>
                {
                    { "queue_token", queueingToken }
                };

                var json = JsonConvert.SerializeObject(body);
                bytes = Encoding.UTF8.GetBytes(json);
                Logger.Debug(String.Format("json={0} and bytes={1} with byteLength={3} and url={2}", json, bytes, queueingApiUrl, bytes.Length));
            }
            else
            {
                bytes = new byte[] { 0 }; // Unity does does not deal with empty byte arrays as the body of the request. So the 0 byte placeholder is needed.s
            }

            return bytes;
        }

        private static bool TryDeserializeResponse(WWW www, out IQueueStatus status)
        {
            try
            {
                status = JsonNetSerializer.GetInstance.DeserializeObject<QueueStatus>(www.text);
                return status != null;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                status = null;
                return false;
            }
        }
    }
}