using System;
using Improbable.Corelib.Util;
using UnityEngine;

namespace Improbable.Corelib.Metrics
{
    public class PhysicsLagValueSource : IValueSource<float>, IDisposable
    {
        private readonly ClientPhysicsLatencyReader lagDetection;
        private float lastLagSeconds;
        private float lastReadLagSeconds;
        private float lastReadLagSecondsTimestamp;

        public PhysicsLagValueSource(ClientPhysicsLatencyReader lagDetection)
        {
            this.lagDetection = lagDetection;
            lagDetection.RoundTripMillisUpdated += OnRoundTripMillisUpdated;
        }

        public void Dispose()
        {
            lagDetection.RoundTripMillisUpdated -= OnRoundTripMillisUpdated;
        }

        public float GetValue()
        {
            UpdateCurrentLagValue();
            return lastReadLagSeconds;
        }

        private void OnRoundTripMillisUpdated(int i)
        {
            lastLagSeconds = (float) i / 1000;
        }

        private void UpdateCurrentLagValue()
        {
            var now = Time.time;
            var maxAllowedChangeInLag = (now - lastReadLagSecondsTimestamp) / 4;
            if (maxAllowedChangeInLag > 0)
            {
                lastReadLagSecondsTimestamp = now;
                var differenceInReadLag = lastLagSeconds - lastReadLagSeconds;
                var lagUpdateAmount = System.Math.Min(maxAllowedChangeInLag, System.Math.Max(differenceInReadLag, -maxAllowedChangeInLag));
                lastReadLagSeconds = lastReadLagSeconds + lagUpdateAmount;
            }
        }
    }
}
