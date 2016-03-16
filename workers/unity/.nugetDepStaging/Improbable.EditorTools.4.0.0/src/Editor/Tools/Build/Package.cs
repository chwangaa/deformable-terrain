using System;
using Assets.Editor.Improbable.EditorTools.Build;

namespace Improbable.Unity.EditorTools.Build
{
    /// <summary>
    ///     A Package is a place for you to put your built out player along with everything it needs when it runs.
    ///     Our default flow will upload a package to the cloud and use the FSim package on the physics server, and the
    ///     UnityClient package for the clients.
    /// </summary>
    public class Package : IDisposable
    {
        private readonly IZipPackage zip;

        public Package(string zipPath, string targetBuildVersionInfo)
        {
            zip = Windows7ZipPackage.IsSupported ? new Windows7ZipPackage(zipPath) : new ZlibZipPackage(zipPath) as IZipPackage;
            zip.Comment = targetBuildVersionInfo;
        }

        public void Dispose()
        {
            zip.Dispose();
        }

        public void AddDirectory(string basePath, string subFolder)
        {
            zip.AddDirectory(basePath, subFolder);
        }

        public void AddDirectory(string basePath, string subFolder, string filePattern)
        {
            zip.AddDirectory(basePath, subFolder, filePattern);
        }
    }
}