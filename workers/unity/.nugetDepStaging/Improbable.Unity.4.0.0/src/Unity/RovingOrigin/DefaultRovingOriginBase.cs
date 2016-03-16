using Improbable.Math;
using log4net;
using UnityEngine;

namespace Improbable.Unity.RovingOrigin
{
    public abstract class DefaultRovingOriginBase : MonoBehaviour
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DefaultRovingOriginBase));

        public float Extents = 4000;
        public float Interval = 10;
        private float timeLastCheckedForRemap;

        public virtual void Update()
        {
            if (Interval + timeLastCheckedForRemap < Time.time)
            {
                timeLastCheckedForRemap = Time.time;
                var newOrigin = NewOrigin;
                Logger.DebugFormat("New candidate local origin: {0}", newOrigin);

                var offset = newOrigin - CoordinateSystem.LocalOrigin;

                if (System.Math.Abs(offset.X) > Extents ||
                    System.Math.Abs(offset.Y) > Extents ||
                    System.Math.Abs(offset.Z) > Extents)
                {
                    Logger.InfoFormat("New local origin: {0}", newOrigin);
                    CoordinateSystem.LocalOrigin = newOrigin;
                }
            }
        }

        protected abstract Coordinates NewOrigin { get; }
    }
}
