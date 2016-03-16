namespace Improbable.Unity.EditorTools.Build
{
    public class SimplePackager : IPackager
    {
        public void Prepare(Package package, string packagePath)
        {
            package.AddDirectory(packagePath, ".");
        }
    }
}