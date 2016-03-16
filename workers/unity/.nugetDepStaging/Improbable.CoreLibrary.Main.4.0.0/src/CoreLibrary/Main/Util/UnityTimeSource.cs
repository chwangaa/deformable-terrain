using UnityEngine;

namespace Improbable.Corelib.Util
{
    public class UnityTimeSource : ITimeSource
    {
        public float CurrentTimeInSeconds
        {
            get { return Time.time; }
        }
    }
}