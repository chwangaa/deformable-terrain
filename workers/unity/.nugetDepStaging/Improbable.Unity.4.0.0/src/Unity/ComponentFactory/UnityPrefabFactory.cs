using Improbable.Unity.Entity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Unity.ComponentFactory
{
    public class UnityPrefabFactory : IPrefabFactory<GameObject>
    {
        private static readonly Vector3 InstantiationPoint = new Vector3(-9999, -9999, -9999);

        public GameObject MakeComponent(GameObject loadedPrefab, EntityAssetId assetId)
        {
            return Object.Instantiate(loadedPrefab, InstantiationPoint, Quaternion.identity) as GameObject;
        }

        public void DespawnComponent(GameObject gameObject, EntityAssetId assetId)
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                Object.DestroyImmediate(gameObject);
            }
            else
            {
                Object.Destroy(gameObject);
            }

        }
    }
}