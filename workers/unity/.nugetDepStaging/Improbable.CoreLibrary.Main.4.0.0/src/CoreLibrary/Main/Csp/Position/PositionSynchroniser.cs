using Improbable.Corelib.Metrics;
using Improbable.Corelib.Util;
using Improbable.Entity.Physical;
using Improbable.Math;
using UnityEngine;

namespace Improbable.Corelib.Csp.Position
{
    public class PositionSynchroniser
    {
        public const int DEFAULT_POSITION_CAPACITY = 120 * 3;
        private const double POSITION_RESYNC_EASE_FACTOR = 0.2;
        private readonly IValueSource<float> lagEstimationInSeconds;
        private readonly IReadWriteValue<Coordinates> localPosition;
        private readonly IValueHistory<Coordinates> localPositionHistory;
        private readonly IValueSource<Coordinates> remotePosition;
        private readonly ITimeSource timeSource;
        private readonly PositionHistoryCorrection positionHistoryCorrection = new PositionHistoryCorrection();

        public PositionSynchroniser(IValueSource<Coordinates> remotePosition,
                                    IValueHistory<Coordinates> localPositionHistory,
                                    ITimeSource timeSource,
                                    IValueSource<float> lagEstimationInSeconds,
                                    IReadWriteValue<Coordinates> localPosition)
        {
            this.remotePosition = remotePosition;
            this.localPositionHistory = localPositionHistory;
            this.timeSource = timeSource;
            this.lagEstimationInSeconds = lagEstimationInSeconds;
            this.localPosition = localPosition;
        }

        public void PerformResync()
        {
            positionHistoryCorrection.CorrectionToApply = CalculatePositionDelta() * POSITION_RESYNC_EASE_FACTOR;
            localPosition.SetValue(localPosition.GetValue() + positionHistoryCorrection.CorrectionToApply);
            localPositionHistory.ApplyCorrection(positionHistoryCorrection, lagEstimationInSeconds.GetValue() + 0.1f);
            localPositionHistory.RecordValue(localPosition.GetValue(), timeSource.CurrentTimeInSeconds);
        }

        public Vector3d CalculatePositionDelta()
        {
            float pastClientTimeInSeconds = timeSource.CurrentTimeInSeconds - lagEstimationInSeconds.GetValue();
            return remotePosition.GetValue() - localPositionHistory.ValueAtTime(pastClientTimeInSeconds);
        }

        public static PositionSynchroniser CreateRigidbodyPositionSynchroniser(IValueHistory<Coordinates> localPositionHistory,
                                                                               GameObject gameObject,
                                                                               PositionReader serverPositionState,
                                                                               ClientPhysicsLatencyReader physicsLagDetectionState)
        {
            var serverPosition = new PositionStateValueSource(serverPositionState);
            localPositionHistory.Reset(serverPosition.GetValue(), Time.time);
            return new PositionSynchroniser(serverPosition,
                                            localPositionHistory,
                                            new UnityTimeSource(),
                                            new PhysicsLagValueSource(physicsLagDetectionState),
                                            new RigidbodyPositionValue(gameObject));
        }

        private class PositionHistoryCorrection : IHistoryCorrection<Coordinates>
        {
            public Vector3d CorrectionToApply = Vector3d.ZERO;

            public void CorrectValue(ref Coordinates value, float timestamp)
            {
                value += CorrectionToApply;
            }
        }

        public static CircularBufferValueHistory<Coordinates> CreatePositionHistory(int historyCapacity)
        {
            return new CircularBufferValueHistory<Coordinates>(historyCapacity, new PositionInterpolation());
        }
    }
}
