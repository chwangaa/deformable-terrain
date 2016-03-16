using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Improbable.Gel.Util;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;

namespace Improbable.Unity.EditorTools.PrefabExport
{
    public class EntityPrefabExporter
    {
        public static void ExportEntityPrefabs()
        {
            PathUtil.EnsureDirectoryExists(EditorPaths.PREFAB_SOURCE_DIRECTORY);
            PathUtil.EnsureDirectoryExists(EditorPaths.PREFAB_RESOURCES_DIRECTORY);
            PathUtil.EnsureDirectoryExists(EditorPaths.prefabExportDirectory);

            EnsurePrefabsHaveAssetNames(FindAssetGuids());
            EditorApplication.SaveAssets();
            ExportAllNamedPrefabs();
        }

        public static IEnumerable<string> FindAssetGuids()
        {
            return AssetDatabase.FindAssets("t:prefab", new[]
            {
                EditorPaths.PREFAB_SOURCE_DIRECTORY,
                EditorPaths.PREFAB_RESOURCES_DIRECTORY
            }).Where(IsPrefab);
        }

        private static bool IsPrefab(string guid)
        {
            return Path.GetExtension(AssetDatabase.GUIDToAssetPath(guid)) == ".prefab";
        }

        private static void ExportAllNamedPrefabs()
        {
            BuildPipeline.BuildAssetBundles(EditorPaths.prefabExportDirectory, BuildAssetBundleOptions.UncompressedAssetBundle);
        }

        private static void EnsurePrefabsHaveAssetNames(IEnumerable<string> guids)
        {
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var importer = AssetImporter.GetAtPath(path);
                var name = Path.GetFileNameWithoutExtension(path);
                importer.assetBundleName = name;
            }
        }
    }
}