using System.Collections.Generic;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard
{
    internal class EntityExportUtil
    {
        public static List<GameObject> DisableExportedEntities()
        {
            List<GameObject> allPrefabsInFolder = FindAllPrefabsFromPrefabFolder();
            DisableAllPrefabs(allPrefabsInFolder);
            return allPrefabsInFolder;
        }

        public static List<GameObject> FindAllPrefabsFromPrefabFolder()
        {
            var prefabsInSceneFromPrefabFolder = new List<GameObject>();
            var gameObjects = Object.FindObjectsOfType<GameObject>();
            for (int index = 0; index < gameObjects.Length; ++index)
            {
                GameObject gameObject = gameObjects[index];
                string assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(PrefabUtility.FindPrefabRoot(gameObject)));

                if (assetPath.Contains(EditorPaths.PREFAB_RESOURCES_DIRECTORY) || assetPath.Contains(EditorPaths.PREFAB_SOURCE_DIRECTORY))
                {
                    prefabsInSceneFromPrefabFolder.Add(gameObject);
                }
            }
            return prefabsInSceneFromPrefabFolder;
        }

        public static void ReenableExportedEntities(IEnumerable<GameObject> exportedEntities)
        {
            EnableAllPrefabs(exportedEntities);
        }

        private static void DisableAllPrefabs(IEnumerable<GameObject> prefabs)
        {
            foreach (GameObject prefab in prefabs)
            {
                prefab.SetActive(false);
            }
        }

        private static void EnableAllPrefabs(IEnumerable<GameObject> prefabs)
        {
            foreach (GameObject prefab in prefabs)
            {
                prefab.SetActive(true);
            }
        }
    }
}