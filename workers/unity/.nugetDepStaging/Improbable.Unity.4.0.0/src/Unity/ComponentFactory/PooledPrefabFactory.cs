using System.Collections.Generic;
using Improbable.Unity.Entity;
using UnityEngine;

namespace Improbable.Unity.ComponentFactory
{
    public class PooledPrefabFactory : IPrefabFactory<GameObject>
    {
        private readonly Dictionary<EntityAssetId, List<GameObject>> OutOfDatePools = new Dictionary<EntityAssetId, List<GameObject>>();
        private readonly Dictionary<EntityAssetId, GameObject> Pools = new Dictionary<EntityAssetId, GameObject>();
        private static readonly Vector3 InstantiationPoint = new Vector3(-9999, -9999, -9999);

        public GameObject MakeComponent(GameObject loadedPrefab, EntityAssetId assetId)
        {
            return Spawn(loadedPrefab, assetId);
        }

        public void DespawnComponent(GameObject gameObject, EntityAssetId assetId)
        {
            Despawn(gameObject, assetId);
        }

        public void InvalidatePool(EntityAssetId assetId)
        {
            if (!Pools.ContainsKey(assetId))
            {
                return;
            }

            GameObject currentPool = Pools[assetId];
            Pools.Remove(assetId);

            MarkPoolAsInvalid(assetId, currentPool);
        }

        public void PreparePool(GameObject loadedPrefab, EntityAssetId assetId, int count)
        {
            var poolComponent = GetOrCreatePool(loadedPrefab, assetId);

            for (int i = 0; i < count; ++i)
            {
                poolComponent.CreateInactiveInPool();
            }
        }

        private GameObject Spawn(GameObject loadedPrefab, EntityAssetId assetId)
        {
            var pool = GetOrCreatePool(loadedPrefab, assetId);
            return pool.Spawn(InstantiationPoint, Quaternion.identity);
        }

        private void Despawn(GameObject pooledGameObject, EntityAssetId assetId)
        {
            GameObject pool;
            if (Pools.TryGetValue(assetId, out pool))
            {
                var container = pool.GetComponent<PooledPrefabContainer>();
                if (container.Contains(pooledGameObject))
                {
                    container.Despawn(pooledGameObject);
                    return;
                }
            }
            DespawnFromOldPools(pooledGameObject, assetId);
        }

        private void DespawnFromOldPools(GameObject pooledGameObject, EntityAssetId assetId)
        {
            List<GameObject> oldPools;
            if (!OutOfDatePools.TryGetValue(assetId, out oldPools))
            {
                return;
            }

            foreach (var pool in oldPools)
            {
                var container = pool.GetComponent<PooledPrefabContainer>();
                if (container.Contains(pooledGameObject))
                {
                    container.Despawn(pooledGameObject);
                    if (container.ActiveCount == 0)
                    {
                        oldPools.Remove(pool);
                        Object.Destroy(pool);
                    }
                    return;
                }
            }
        }

        private void MarkPoolAsInvalid(EntityAssetId assetId, GameObject pool)
        {
            List<GameObject> oldPools;
            if (!OutOfDatePools.TryGetValue(assetId, out oldPools))
            {
                oldPools = new List<GameObject>();
                OutOfDatePools.Add(assetId, oldPools);
            }

            oldPools.Add(pool);
        }


        private PooledPrefabContainer GetOrCreatePool(GameObject loadedPrefab, EntityAssetId assetId)
        {
            GameObject pool;
            if (Pools.TryGetValue(assetId, out pool))
            {
                return pool.GetComponent<PooledPrefabContainer>();
            }

            return CreatePool(loadedPrefab, assetId);
        }

        private PooledPrefabContainer CreatePool(GameObject loadedPrefab, EntityAssetId assetId)
        {
            var pool = new GameObject();
            Pools[assetId] = pool;

            var poolComponent = pool.AddComponent<PooledPrefabContainer>();
            poolComponent.Init(loadedPrefab, assetId);

            return poolComponent;
        }
    }
}