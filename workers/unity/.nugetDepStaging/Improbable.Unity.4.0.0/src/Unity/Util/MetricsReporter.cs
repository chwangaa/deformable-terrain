using System.Collections.Generic;
using System.Diagnostics;
using Improbable.Core.Network;
using Improbable.Fapi.Protocol;
using Improbable.Util.Metrics;
using UnityEngine;

namespace Improbable.Unity.Util
{
    class MetricsReporter : MonoBehaviour
   { 
        private IBridgeCommunicator _bridgeCommunicator;
        private readonly IDictionary<string, Stat> _metricsStats = new Dictionary<string, Stat>();
        private EngineMetrics _metricsMessage;

        private readonly Stopwatch _stopwatch = new Stopwatch();
        private long _lastTime;

        public float ReportingPeriodSeconds = 30;

        private IMetricsPublisher metricsPublisher = null;

        public void SetupDependencies(IBridgeCommunicator bridgeCommunicator, IMetricsPublisher publisher)
        {
            _bridgeCommunicator = bridgeCommunicator;
            _metricsMessage = new EngineMetrics();
            _stopwatch.Start();

            metricsPublisher = publisher;
        }

        public void Update()
        {
            foreach (var keyValuePair in MetricsUpdatersManager.AllMetrics)
            {
                var metric = keyValuePair.Key;
                var value = keyValuePair.Value.Value;
                Stat stat;
                if (!_metricsStats.TryGetValue(metric, out stat))
                {
                    stat = new Stat(metric);
                    _metricsStats.Add(metric, stat);
                }
                stat.IncludeValue(value);
            }
            if (_stopwatch.ElapsedMilliseconds - _lastTime > ReportingPeriodSeconds * 1000)
            {
                _lastTime = _stopwatch.ElapsedMilliseconds;
                Report();
            }

            if (metricsPublisher != null)
            {
                metricsPublisher.PublishScheduledMetrics();
            }
        }

        private void Report()
        {
            foreach (var stat in _metricsStats.Values)
            {
                stat.Write(_metricsMessage);
                stat.Reset();
            }

            _bridgeCommunicator.Send(_metricsMessage);
        }

        private class Stat
        {
            private readonly string _name;
            private float _total;

            public Stat(string name)
            {
                Reset();
                _name = name;
            }

            public void IncludeValue(float value)
            {
                if (value < Min)
                {
                    Min = value;
                }
                if (value > Max)
                {
                    Max = value;
                }
                _total += value;
                Measures++;
            }

            public float Min { get; private set; }

            public float Max { get; private set; }

            public int Measures { get; private set; }

            public float Mean
            {
                get
                { 
                    return _total / Measures;
                }
            }

            public void Reset()
            {
                Measures = 0;
                Min = float.MaxValue;
                Max = float.MinValue;
                _total = 0;
            }

            public void Write(EngineMetrics metrics)
            {
                var fields = metrics.Metrics;
                lock (fields)
                {
                    if (Measures > 0)
                    {
                        fields[_name + ".Min"] = "" + Min;
                        fields[_name + ".Mean"] = "" + Mean;
                        fields[_name + ".Max"] = "" + Max;
                    }
                    else
                    {
                        fields.Remove(_name + ".Min");
                        fields.Remove(_name + ".Mean");
                        fields.Remove(_name + ".Max");
                    }
                }
            }
        }
    }
}