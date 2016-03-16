using System;
using Improbable.Entity.Physical;
using Improbable.Logging;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using Improbable.Util;
using log4net;
using UnityEngine;

namespace Improbable.Corelib.Physical
{
    public class PhysicalPosition : PhysicalBase<Coordinates>
    {
        [Tooltip("The minimum square distance from the last sent position before sending a new state update.")]
        public float PositionNetworkUpdateSquareDistanceThreshold;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(PhysicalPosition));
        private static readonly TimeSpan QuietPeriod = new TimeSpan(0, 0, 5);
        private const int MaxLogMessagesBeforeQuietPeriod = 5;
        private readonly LogLimiter invalidPositionLogLimiter = new LogLimiter(new SystemTimeSource(), QuietPeriod, MaxLogMessagesBeforeQuietPeriod);

        [Require] protected PositionWriter Position;

        protected override void OnEnable()
        {
            base.OnEnable();

            CachedTransform.position = Position.Value.ToUnityVector();
        }

        protected override Coordinates GetLatestValue()
        {
            return CachedTransform.position.ToCoordinates();
        }

        protected override bool IsPastThreshold(Coordinates lastPosition, Coordinates newPosition)
        {
            return !lastPosition.IsWithinSquareDistance(newPosition, PositionNetworkUpdateSquareDistanceThreshold);
        }

        protected override void OnShouldUpdate(float timeDelta, Coordinates newPosition)
        {
            if (newPosition.IsFinite)
            {
                Position.Update
                     .Timestamp(Position.Timestamp + timeDelta)
                     .Value(newPosition)
                     .FinishAndSend();
            }
            else
            {
                if (invalidPositionLogLimiter.CanLogNow())
                {
                    Logger.ErrorFormat("Entity '{0}' (gameObject: '{1}') is trying to set position to '{2}'. Preventing this. Setting the position to last valid one: {3}",
                                       gameObject.GetEntityObject().EntityId,
                                       gameObject.name,
                                       newPosition,
                                       Position.Value);
                    invalidPositionLogLimiter.Logged();
                }
                CachedTransform.position = Position.Value.ToUnityVector();
            }
        }
    }
}
