using System.IO;
using Improbable.Assets;
using Improbable.Unity;
using Improbable.Unity.EditorTools.Build;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    internal class WorkerSpecificPackager
    {
        public static IPackager GetPackager(EnginePlatform enginePlatform, BuildTarget target, Config config)
        {
            var shouldEmbedAssets = config.Assets == AssetDatabaseStrategy.Local;
            var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), EditorPaths.dataDirectory);
            return shouldEmbedAssets
                ? new WorkerSpecificAssetEmbeddingPackager(enginePlatform, dataDirectory) as IPackager
                : new SimplePackager();
        }
    }
}