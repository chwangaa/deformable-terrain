using System;
using Improbable.Util.Metrics;
using IoC;
using UnityEngine;

namespace Improbable.Unity.Util
{
    internal class EngineMetricsMemoryUsage : MonoBehaviour
    {
        private IGauge gcTotalMemory;
        [Inject] public IMetricsCollector MetricsCollector { get; set; }

        public void SetupDependencies()
        {
            gcTotalMemory = MetricsCollector.Gauge("Unity used heap size");
        }

        private void Update()
        {
            gcTotalMemory.Set(GC.GetTotalMemory(/* forceFullCollection */ false));
        }
    }
}
