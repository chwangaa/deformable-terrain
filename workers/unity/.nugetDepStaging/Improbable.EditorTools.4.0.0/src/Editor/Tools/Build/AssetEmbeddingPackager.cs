namespace Improbable.Unity.EditorTools.Build
{
    public class AssetEmbeddingPackager : IPackager
    {
        private readonly string dataDirectory;

        public AssetEmbeddingPackager(string dataDirectory)
        {
            this.dataDirectory = dataDirectory;
        }

        public void Prepare(Package package, string packagePath)
        {
            package.AddDirectory(packagePath, ".");
            package.AddDirectory(dataDirectory, "AssetDatabase/EntityPrefab");
        }
    }
}