using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Metrics.Visualizers
{
    [EngineType(EnginePlatform.FSim)]
    public class PhysicsLagPingReceiver : MonoBehaviour
    {
        [Require] protected ClientPhysicsLatencyReplyWriter lagResponseState;
        [Require] protected ClientPhysicsLatencyReader lagDetectionState;

        protected void OnEnable()
        {
            lagDetectionState.ClientPhysicsPingSent += OnPingEventReceived;
        }

        private void OnPingEventReceived(ClientPhysicsPingSent pingSentEvent)
        {
            lagResponseState.Update
                 .TriggerClientPhysicsPingReceived(pingSentEvent.TimestampMillis)
                 .FinishAndSend();
        }
    }
}