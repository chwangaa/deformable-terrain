using System;
using System.IO;
using System.Text.RegularExpressions;
using Assets.Improbable.CoreLibrary.WorkerSpecific;
using Improbable.Unity;
using Improbable.Unity.EditorTools.Util;
using Improbable.Unity.Entity;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    /// <summary>
    ///     Class responsible for working out compiled WorkerSpecific Prefab path from existing Prefab.
    /// </summary>
    internal static class WorkerSpecificPrefabPath
    {
        private const string PrefabSuffix = ".prefab";

        private static readonly Regex PrefabRegex = new Regex("([^@]+)(?:@(\\w+))?.prefab");

        public static string PrefabPath(EnginePlatform engine, string sourcePrefabPath)
        {
            if (!IsPrefab(sourcePrefabPath))
            {
                throw new ArgumentException(string.Format("{0} is not a valid prefab. Expected file to end with {1}", sourcePrefabPath, PrefabSuffix), "sourcePrefabPath");
            }
            var sourceFileName = Path.GetFileName(sourcePrefabPath);
            var targetFileName = WorkerSpecificPrefabFilename(engine, sourceFileName);
            var path = Path.Combine(EditorPaths.PREFAB_COMPILE_DIRECTORY, targetFileName);
            return path.Replace(@"\", "/");
        }

        private static string WorkerSpecificPrefabFilename(EnginePlatform engine, string prefabFilename)
        {
            var entityAssetId = PrefabFilenameToEntityAssetId(prefabFilename);
            return WorkerSpecificPrefabName.AssetIdToPrefabName(entityAssetId, engine) + PrefabSuffix;
        }

        private static bool IsPrefab(string prefabPath)
        {
            return prefabPath.EndsWith(PrefabSuffix, StringComparison.OrdinalIgnoreCase);
        }

        private static EntityAssetId PrefabFilenameToEntityAssetId(string prefabFilename)
        {
            var regexMatch = PrefabRegex.Match(prefabFilename);
            if (regexMatch.Success)
            {
                var prefabName = regexMatch.Groups[1].Captures[0].Value;
                var contextCaptures = regexMatch.Groups[2].Captures;
                string context = contextCaptures.Count == 1 ? contextCaptures[0].Value : EntityAssetId.DEFAULT_CONTEXT;
                return new EntityAssetId(prefabName, context);
            }
            else
            {
                throw new ArgumentException("Invalid prefab filename", "prefabFilename");
            }
        }
    }
}