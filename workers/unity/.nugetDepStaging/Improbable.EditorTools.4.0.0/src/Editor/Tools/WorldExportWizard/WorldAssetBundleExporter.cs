using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.WorldExportWizard
{
    public static class WorldAssetBundleExporter
    {
        public static void Export(string worldName)
        {
            var fsimBundleExportPath = EditorPaths.FSimBundleExportPath(worldName);
            var clientBundleExportPath = EditorPaths.ClientBundleExportPath(worldName);
            var prefabExportPath = EditorPaths.WorldPrefabPath(worldName);

            var temporaryRootObject = new GameObject("RootObject");
            var gameObjectsInScene = RootGameObjectsInScene();
            AddChildrenToParent(gameObjectsInScene, temporaryRootObject.transform);
            try
            {
                ExportAssetBundle(PrefabUtility.CreatePrefab(prefabExportPath, temporaryRootObject), fsimBundleExportPath, clientBundleExportPath);
            }
            finally
            {
                MoveGameObjectsToRoot(gameObjectsInScene);
                Object.DestroyImmediate(temporaryRootObject);
            }
        }

        private static void ExportAssetBundle(Object mainAsset, string fsimBundleExportPath, string clientBundleExportPath)
        {
// Disable obsolete warning, tracked by https://improbableio.atlassian.net/browse/SDK-816
#pragma warning disable 618
            BuildPipeline.BuildAssetBundle(mainAsset, new[] { mainAsset },
                                           fsimBundleExportPath,
                                           BuildAssetBundleOptions.UncompressedAssetBundle,
                                           BuildTarget.StandaloneWindows);
#pragma warning restore 618
            FileUtil.CopyFileOrDirectory(fsimBundleExportPath, clientBundleExportPath);
        }

        private static List<Transform> RootGameObjectsInScene()
        {
            return Object.FindObjectsOfType<Transform>().Where(transform => transform.parent == null).ToList();
        }

        private static void MoveGameObjectsToRoot(IEnumerable<Transform> transforms)
        {
            foreach (var transform in transforms)
            {
                transform.parent = null;
            }
        }

        private static void AddChildrenToParent(IEnumerable<Transform> childTransforms, Transform parentTransform)
        {
            foreach (var rootGameObject in childTransforms)
            {
                rootGameObject.parent = parentTransform.transform;
            }
        }
    }
}