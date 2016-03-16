namespace Improbable.Unity.EditorTools.Build
{
    public interface IPackager
    {
        /// <param name="package">the package that this packager should populate</param>
        /// <param name="packagePath">the working directory that unity has built into.</param>
        /// <remarks>
        /// An IPackager takes a built unity client and populates a Package ready for it to be deployed.  Typically you would package the 
        /// results of the unity build that are stored in packagePath, but if you need extra files in here (perhaps assets), this is where you would add them.
        /// To configure the IPackager in use, set the UnityPlayerBuilder.GetPackager function to return your custom packager.
        /// </remarks>
        void Prepare(Package package, string packagePath);
    }
}