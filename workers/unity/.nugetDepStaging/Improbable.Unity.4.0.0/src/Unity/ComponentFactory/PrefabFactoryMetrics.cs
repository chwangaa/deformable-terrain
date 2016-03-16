using Improbable.Unity.Entity;
using Improbable.Util.Metrics;
using UnityEngine;

namespace Improbable.Unity.ComponentFactory
{
    /// <summary>
    /// A Proxy to wrap a IPrefabFactroy such that we report metrics about
    /// the number of entities in an engine by prefab
    /// 
    /// metrics are named "prefab.{prefabName}.count"
    /// </summary>
    public class PrefabFactoryMetrics : IPrefabFactory<GameObject>
    {
        private readonly IMetricsFactory metricsFactory;
        private readonly IPrefabFactory<GameObject> prefabFactory;

        public PrefabFactoryMetrics(IMetricsFactory metricsFactory, IPrefabFactory<GameObject> prefabFactory)
        {
            this.metricsFactory = metricsFactory;
            this.prefabFactory = prefabFactory;
        }

        /// <inheritdoc />
        public GameObject MakeComponent(GameObject prefabGameObject, EntityAssetId assetId)
        {
            GetPrefabsGauge(assetId.PrefabName).Increment();
            return prefabFactory.MakeComponent(prefabGameObject, assetId);
        }

        /// <inheritdoc />
        public void DespawnComponent(GameObject gameObject, EntityAssetId assetId)
        {
            GetPrefabsGauge(assetId.PrefabName).Decrement();
            prefabFactory.DespawnComponent(gameObject, assetId);
        }

        private IGauge GetPrefabsGauge(string prefabName)
        {
            return metricsFactory.Collector.Gauge("prefab." + prefabName + ".count");
        }
    }
}
