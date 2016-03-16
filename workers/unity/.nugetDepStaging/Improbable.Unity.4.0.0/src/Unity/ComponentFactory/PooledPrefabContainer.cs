using System.Collections.Generic;
using Improbable.Unity.Entity;
using log4net;
using UnityEngine;

namespace Improbable.Unity.ComponentFactory
{
    public class PooledPrefabContainer : MonoBehaviour
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PooledPrefabContainer));
        private const int PoolLayer = 31;

        private GameObject LoadedPrefab;
        private EntityAssetId assetId;
        
        private int InstanceNumber = 1;

        private readonly Dictionary<GameObject, PooledObject> SpawnedObjects = new Dictionary<GameObject, PooledObject>();
        private readonly List<PooledObject> DespawnedObjects = new List<PooledObject>();

        public void Init(GameObject prefab, EntityAssetId assetId)
        {
            AddSelfToPoolLayer();
            LoadedPrefab = prefab;
            this.assetId = assetId;
            name = string.Format("[Pool] {0} {1}", assetId.PrefabName, assetId.Context);
        }

        private void AddSelfToPoolLayer()
        {
            gameObject.layer = PoolLayer;
        }

        public static bool IsPool(GameObject obj)
        {
            return obj.layer == PoolLayer;
        }

        public void Despawn(GameObject spawnedObject)
        {
            PooledObject pooled;
            if (SpawnedObjects.TryGetValue(spawnedObject, out pooled))
            {
                pooled.DespawnedOnFrame = Time.frameCount;
                SpawnedObjects.Remove(spawnedObject);
                SetDespawned(spawnedObject);
                DespawnedObjects.Add(pooled);
            }
            else
            {
                Logger.WarnFormat("Could not despawn {0} (prefab {1} {2})", spawnedObject.name, assetId.PrefabName, assetId.Context);
            }
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            var freeObject = FindExistingObject() ?? CreateNewObject();
            InitObject(position, rotation, freeObject);
            return freeObject.GameObject;
        }

        public bool Contains(GameObject obj)
        {
            return SpawnedObjects.ContainsKey(obj);
        }

        public int ActiveCount
        {
            get { return SpawnedObjects.Count; }
        }

        private PooledObject CreateNewObject()
        {
            return new PooledObject(LoadedPrefab) { GameObject = { name = string.Format("{0} {1} {2:#000}", assetId.PrefabName, assetId.Context, InstanceNumber++) } };
        }

        private void SetDespawned(GameObject spawnedObject)
        {
            spawnedObject.transform.parent = transform;
            spawnedObject.SetActive(false);
        }

        public void CreateInactiveInPool()
        {
            var pooled = CreateNewObject();
            SetDespawned(pooled.GameObject);
            DespawnedObjects.Add(pooled);
        }

        private void InitObject(Vector3 position, Quaternion rotation, PooledObject pooledObject)
        {
            pooledObject.GameObject.transform.position = position;
            pooledObject.GameObject.transform.rotation = rotation;
            pooledObject.GameObject.transform.parent = transform;

            SpawnedObjects[pooledObject.GameObject] = pooledObject;
            
            pooledObject.GameObject.SetActive(true);            
        }

        private PooledObject FindExistingObject()
        {
            // Entities that were despawned within the last 2 frames are ignored to ensure that they aren't
            // re-used before all of their scheduled cleanup operations are completely finished
            for (var index = 0; index < DespawnedObjects.Count; index++)
            {
                var obj = DespawnedObjects[index];
                if (Time.frameCount - obj.DespawnedOnFrame >= 2)
                {
                    DespawnedObjects.RemoveAt(index);
                    return obj;
                }
            }

            return null;
        }

        private class PooledObject
        {
            public PooledObject(GameObject loadedPrefab)
            {
                GameObject = (GameObject) Instantiate(loadedPrefab);
            }

            public GameObject GameObject { get; private set; }
            public int DespawnedOnFrame { get; set; }
        }
    }
}