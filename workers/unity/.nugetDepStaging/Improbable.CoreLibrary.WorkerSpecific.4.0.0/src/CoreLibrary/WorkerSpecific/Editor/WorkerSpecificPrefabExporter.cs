using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Improbable.Gel.Util;
using Improbable.Corelibrary.PreProcessor.Global;
using Improbable.Unity.EditorTools.PrefabExport;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    /// <summary>
    ///     Prefab exporter responsible for exporting Worker-Specific prefabs
    ///     Exported prefabs are compiled at export time and have global-preprocessors applied to them.
    /// </summary>
    internal static class WorkerSpecificPrefabExporter
    {
        public static void ExportPrefabs()
        {
            EnsureDirectoriesExist();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.SaveAssets();
            if (EditorApplication.isCompiling)
            {
                //TODO(seb): Potentially spawn operation in separate thread and retry until compilation finished.
                throw new InvalidOperationException("Editor is compiling scripts");
            }
            var assetGuids = EntityPrefabExporter.FindAssetGuids();
            ExportAssets(assetGuids);
        }

        private static void ExportAssets(IEnumerable<string> assetGuids)
        {
            var compiledPrefabs = CompileEngineSpecificPrefabs(assetGuids);
            EditorApplication.SaveAssets();
            ExportPrefabs(compiledPrefabs);
        }

        private static void EnsureDirectoriesExist()
        {
            PathUtil.EnsureDirectoryExists(EditorPaths.PREFAB_SOURCE_DIRECTORY);
            PathUtil.EnsureDirectoryExists(EditorPaths.PREFAB_RESOURCES_DIRECTORY);
            PathUtil.EnsureDirectoryExists(EditorPaths.prefabExportDirectory);
            PathUtil.EnsureDirectoryExists(EditorPaths.PREFAB_COMPILE_DIRECTORY);
        }

        private static IEnumerable<string> CompileEngineSpecificPrefabs(IEnumerable<string> guids)
        {
            var globalPreprocessorAdder = new GameObjectComponentAdder(GlobalPreProcessors.Preprocessors);
            return WorkerSpecificPrefabGenerator.Generate(guids, (platform, prefab) => { globalPreprocessorAdder.AddComponentsTo(prefab); });
        }

        private static IEnumerable<AssetBundleBuild> AssetBundleBuilds(IEnumerable<string> compiledPrefabPaths)
        {
            foreach (var path in compiledPrefabPaths)
            {
                AssetBundleBuild assetBundleBuild = new AssetBundleBuild
                {
                    assetBundleName = Path.GetFileNameWithoutExtension(path).ToLower(),
                    assetNames = new[] { path },
                };
                yield return assetBundleBuild;
            }
        }


        private static void ExportPrefabs(IEnumerable<string> assetPaths)
        {
            BuildPipeline.BuildAssetBundles(EditorPaths.prefabExportDirectory,
                                            AssetBundleBuilds(assetPaths).ToArray(),
                                            BuildAssetBundleOptions.UncompressedAssetBundle);
        }
    }
}
