﻿namespace Improbable.Corelib.Physical
{
    public static class DefaultPhysicalParameters
    {
        public const float NetworkUpdatePeriodThreshold = 0.05f;
        public const float SlottedNetworkUpdatePeriodThreshold = 1f;
        public const float PositionNetworkUpdatePeriodThreshold = NetworkUpdatePeriodThreshold;
        public const float SlottedPositionNetworkUpdatePeriodThreshold = SlottedNetworkUpdatePeriodThreshold;
        public const float PositionNetworkUpdateSquareDistanceThreshold = 0.01f * 0.01f;
        public const float RotationNetworkUpdatePeriodThreshold = NetworkUpdatePeriodThreshold;
        public const float SlottedRotationNetworkUpdatePeriodThreshold = SlottedNetworkUpdatePeriodThreshold;
        public const float RotationNetworkUpdateSquareDistanceThreshold = 0.01f * 0.01f;
        public const float RotationNetworkUpdateAngleThreshold = 0.01f;
        public const float MinAngleToInterpolateBetween = float.MinValue;
        public const float MinDistanceToInterpolateBetween = float.MinValue;
        public const float MaxSecondsToInterpolateAfterLastUpdate = 0f;
        public const bool CanBeClientAuthoritative = false;
        public const bool CanBeFSimAuthoritative = true;
    }
}