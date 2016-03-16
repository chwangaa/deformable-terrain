using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using Improbable.Core.Network;
using Improbable.Fapi.Protocol;
using UnityEngine;

namespace Improbable.Unity.Util
{
    public class EngineMetricsFPS : MonoBehaviour
    {
        private const long UPDATE_MIN_PERIOD_MILLIS = 2000; // 1 update per 2 seconds.
        private readonly Stopwatch stopwatch = new Stopwatch();
        private FPSMetric dynamicRate;
        private FPSMetric fixedRate;

        private const string prefixLoad = "Load";
        public EngineMetrics metricsMessage;
        private IBridgeCommunicator bridgeCommunicator;
        private TimeSpan lastFixedUpdateStart;
        private TimeSpan lastUpdatedLoadMetric;
        private TimeSpan cumulativeTimeTakenForFixedUpdate;

        public void SetupDependencies(IBridgeCommunicator bridgeCommunicator, int fixedUpdateRate)
        {
            this.bridgeCommunicator = bridgeCommunicator;
            metricsMessage = new EngineMetrics();
            dynamicRate = new FPSMetric("Dynamic", this);
            fixedRate = new FPSMetric("Fixed", this);
            stopwatch.Start();
        }

        public void FixedUpdate()
        {
            lastFixedUpdateStart = stopwatch.Elapsed;
            if (fixedRate != null && fixedRate.FrameRendered(stopwatch.ElapsedMilliseconds))
            {
                var sinceLoadLastUpdated = lastFixedUpdateStart - lastUpdatedLoadMetric;
                var load = 1.0 * cumulativeTimeTakenForFixedUpdate.Ticks / sinceLoadLastUpdated.Ticks;

                lock (metricsMessage.Metrics)
                {
                    metricsMessage.Metrics[prefixLoad] = load.ToString(CultureInfo.InvariantCulture);
                }
                bridgeCommunicator.Send(metricsMessage);

                cumulativeTimeTakenForFixedUpdate = TimeSpan.Zero;
                lastUpdatedLoadMetric = lastFixedUpdateStart;
            }
            StartCoroutine(EndOfFixedUpdate());
        }

        public IEnumerator EndOfFixedUpdate()
        {
            yield return new WaitForEndOfFrame();
            cumulativeTimeTakenForFixedUpdate += stopwatch.Elapsed - lastFixedUpdateStart;
        }

        public void Update()
        {
            if (dynamicRate != null)
            {
                dynamicRate.FrameRendered(stopwatch.ElapsedMilliseconds);
            }
        }

        private class FPSMetric
        {
            private readonly string prefixFPS;
            private readonly string prefixART;
            private readonly EngineMetricsFPS metrics;
            private long startedAt;
            private int frameCount;
            private double dt;
            public double Fps;

            internal FPSMetric(string prefix, EngineMetricsFPS metrics)
            {
                prefixFPS = prefix + ".FPS";
                prefixART = prefix + ".AverageRenderTime";
                this.metrics = metrics;
            }

            /* Time.deltaTime gives you the time to render the last frame.
             * dt is then the sum of frame rendering time since last Fps update.
             * frameCount is the number of frame renderings since last Fps update.
             */
            internal bool FrameRendered(long currentTime)
            {
                frameCount++;
                dt += (1000 * Time.deltaTime);
                var elapsed = currentTime - startedAt;
                if (elapsed > UPDATE_MIN_PERIOD_MILLIS)
                {
                    Fps = ((frameCount * 1000.0) / elapsed);
                    WriteMetrics();
                    frameCount = 0;
                    dt = 0;
                    startedAt = currentTime;
                    return true;
                }
                return false;
            }

            private void WriteMetrics()
            {
                lock (metrics.metricsMessage.Metrics)
                {
                    metrics.metricsMessage.Metrics[prefixFPS] = Fps.ToString(CultureInfo.InvariantCulture);
                    metrics.metricsMessage.Metrics[prefixART] = (dt / frameCount).ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }
}