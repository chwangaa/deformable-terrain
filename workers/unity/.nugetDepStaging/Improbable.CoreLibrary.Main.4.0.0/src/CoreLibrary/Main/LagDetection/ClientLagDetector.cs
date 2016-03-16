using Improbable.Corelib.Metrics;
using Improbable.Unity.Visualizer;
using Improbable.Util.Metrics;
using UnityEngine;
using IoC;

namespace Improbable.Corelib.LagDetection
{
    public class ClientLagDetector : MonoBehaviour
    {
        [Inject] public IMetricsCollector Collector { get; set; }
        private float lastTimeOfSentPing;
        private IMetricsUpdater metrics;
        private IGauge gaugeMetrics;

        [Require] protected ClientPhysicsLatencyReplyReader physicsLagReplierState;
        [Require] protected ClientPhysicsLatencyWriter lagDetectionState;

        protected void OnEnable()
        {
            metrics  = MetricsUpdatersManager.GetUpdater("RTT FSim Latency (ms)");
            gaugeMetrics = Collector.Gauge("Client to FSim latency");
            physicsLagReplierState.ClientPhysicsPingReceived += OnPhysicsLagPingReceived;
        }

        protected void Update()
        {
            float currentTime = Time.time;
            if (IsTimeToRefreshLag(currentTime))
            {
                lagDetectionState.Update
                     .TriggerClientPhysicsPingSent(ToMillis(currentTime))
                     .FinishAndSend();
                lastTimeOfSentPing = currentTime;
            }
        }

        private void OnPhysicsLagPingReceived(ClientPhysicsPingReceived pingReceived)
        {
            int currentTimeInMillis = ToMillis(Time.time);
            var latency = currentTimeInMillis - pingReceived.ReceivedPingTimestampMillis;
            lagDetectionState.Update
                 .RoundTripMillis(latency)
                 .FinishAndSend();

            metrics.Update(latency);
            gaugeMetrics.Set(latency);
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
