using System;
using Improbable.Fapi.Receptionist;
using Newtonsoft.Json;

namespace Improbable.Unity.Core
{
    public class QueueStatus : IQueueStatus
    {
        [JsonProperty("retry_in")]
        public float RetryInS { get; private set; }

        public TimeSpan RetryAfter
        {
            get { return TimeSpan.FromSeconds(RetryInS); }
        }

        [JsonProperty("queue_token", Required = Required.Always)]
        public string QueueToken { get; private set; }

        [JsonProperty("status", Required = Required.Always)]
        public string Status { get; private set; }

        [JsonProperty("place_in_queue")]
        public int PlaceInQueue { get; private set; }
    }
}