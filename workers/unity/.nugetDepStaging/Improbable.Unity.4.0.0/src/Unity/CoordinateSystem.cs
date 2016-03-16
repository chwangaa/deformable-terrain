using System;
using Improbable.Math;
using Improbable.Unity.Common.Core.Math;
using log4net;

namespace Improbable.Unity
{
    public static class CoordinateSystem
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CoordinateSystem));

        private static Coordinates localOrigin;

        public static Coordinates LocalOrigin
        {
            get { return localOrigin; }
            set
            {
                var oldOrigin = localOrigin;
                localOrigin = value;
                OnLocalOriginMoved(oldOrigin);
            }
        }

        private static void OnLocalOriginMoved(Coordinates oldOrigin)
        {
            logger.InfoFormat("Local origin moved to: {0} from: {1}", localOrigin, oldOrigin);
            var evt = LocalOriginMoved;
            if (evt != null)
            {
                evt(oldOrigin, localOrigin);
            }
        }

        public static event Action<Coordinates, Coordinates> LocalOriginMoved;

        public static UnityEngine.Vector3 ToUnityVector(this Coordinates globalPosition)
        {
            return (globalPosition - LocalOrigin).ToUnityVector();
        }

        public static Coordinates ToCoordinates(this UnityEngine.Vector3 localPosition)
        {
            return localPosition.ToNativeVector() + LocalOrigin; 
        }
    }


}
