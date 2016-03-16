using Assets.Improbable.CoreLibrary.WorkerSpecific;
using Improbable.Unity;
using Improbable.Unity.EditorTools.Build;

namespace Improbable.CoreLibrary.WorkerSpecific.Editor
{
    /// <summary>
    ///     IPackager which filters assets to package based on an engine platform string
    /// </summary>
    internal class WorkerSpecificAssetEmbeddingPackager : IPackager
    {
        private const string RelativeAssetDatabasePath = "AssetDatabase/EntityPrefab";
        private readonly string dataDirectory;
        private readonly EnginePlatform enginePlatform;

        public WorkerSpecificAssetEmbeddingPackager(EnginePlatform enginePlatform, string dataDirectory)
        {
            this.enginePlatform = enginePlatform;
            this.dataDirectory = dataDirectory;
        }

        public void Prepare(Package package, string packagePath)
        {
            var assetFilterPattern = string.Format("*{0}{1}*", WorkerSpecificPrefabName.WorkerNameSeparator, enginePlatform.ToString().ToLower());
            package.AddDirectory(packagePath, ".");
            package.AddDirectory(dataDirectory, RelativeAssetDatabasePath, assetFilterPattern);
        }
    }
}