using System.Diagnostics;

namespace Improbable.Unity.Logging
{
    public class UnityTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public override void WriteLine(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}