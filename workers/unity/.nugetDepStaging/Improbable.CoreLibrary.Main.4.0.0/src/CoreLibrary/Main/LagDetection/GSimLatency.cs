using Improbable.Corelib.Metrics;
using Improbable.Unity.Visualizer;
using Improbable.Util.Metrics;
using UnityEngine;

namespace Improbable.Corelib.LagDetection
{
    public class GSimLatency : MonoBehaviour
    {
        private float lastTimeOfSentPing;

        [Require] protected EngineLatencyWriter lagDetectionState;
        [Require] protected EngineLatencyReplyReader lagResponseState;

        private IMetricsUpdater metrics;

        protected void OnEnable()
        {
            metrics = MetricsUpdatersManager.GetUpdater("GSim Latency");
            lagResponseState.EnginePingReceived += OnPingReceived;
        }

        protected void Update()
        {
            float currentTime = Time.time;
            if (IsTimeToRefreshLag(currentTime))
            {
                lagDetectionState.Update
                    .TriggerEnginePingSent(ToMillis(currentTime))
                    .FinishAndSend();
                lastTimeOfSentPing = currentTime;
            }
        }

        private void OnPingReceived(EnginePingReceived pingReceived)
        {
            int currentTimeInMillis = ToMillis(Time.time);
            var latency = currentTimeInMillis - pingReceived.ReceivedPingTimestampMillis;
            lagDetectionState.Update
                 .RoundTripMillis(latency)
                 .FinishAndSend();

            if (metrics != null)
            {
                metrics.Update(latency);
            }
        }

        private bool IsTimeToRefreshLag(float currentTime)
        {
            return currentTime - lastTimeOfSentPing > (lagDetectionState.RefreshPeriodMillis / 1000f);
        }

        private static int ToMillis(float currentTime)
        {
            return (int)System.Math.Floor(currentTime * 1000);
        }

    }
}
