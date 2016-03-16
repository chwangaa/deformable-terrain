using System.Collections.Generic;
using Improbable.Math;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.ComponentFactory;
using UnityEngine;

namespace Improbable.Unity.RovingOrigin
{
    public class CoordinateRemapper : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject[] exceptions;
#pragma warning restore 649
        private static readonly HashSet<GameObject> ExceptionsSet = new HashSet<GameObject>();

        protected void Awake()
        {
            ExceptionsSet.UnionWith(exceptions);
        }

        public static void CoodinateSystemOnLocalOriginMoved(Coordinates oldOrigin, Coordinates newOrigin)
        {
            var offset = (oldOrigin - newOrigin).ToUnityVector();
            var allObjects = FindObjectsOfType<GameObject>();
            for (var i = 0; i < allObjects.Length; i++)
            {
                var obj = allObjects[i];
                if (IsInLocalCoordinates(obj))
                {
                    obj.transform.position += offset;
                }
            }
        }

        public static bool IsInLocalCoordinates(GameObject obj)
        {
            return obj.activeSelf && !PooledPrefabContainer.IsPool(obj) && !IsParented(obj) && !ExceptionsSet.Contains(obj);
        }

        private static bool IsParented(GameObject obj)
        {
            var parent = obj.transform.parent;
            return parent != null && !PooledPrefabContainer.IsPool(parent.gameObject);
        }
    }
}