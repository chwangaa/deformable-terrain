using System;

namespace Improbable.Unity.EditorTools.Build
{
    internal interface IZipPackage : IDisposable
    {
        /// <summary>
        /// Adds a directory to this Zip package.
        /// </summary>
        /// <param name="basePath">the base directory that contains the sub-folder</param>
        /// <param name="subFolder">the subfolder whose contents to put into the zip.</param>
        /// <remarks>
        ///   If called with base path like "/foo/bar" and sub-folder "moo/zar" then the zip
        ///   will contain all files within "/foo/bar/moo/zar" prefixed with "moo/zar".
        /// </remarks>
        void AddDirectory(string basePath, string subFolder);

        /// <summary>
        /// Adds a directory to this Zip package using a file pattern.
        /// </summary>
        /// <param name="basePath">the base directory that contains the sub-folder</param>
        /// <param name="subFolder">the subfolder whose contents to put into the zip.</param>
        /// <param name="filePattern">A file wildcard pattern to restrict which files to include</param>
        void AddDirectory(string basePath, string subFolder, string filePattern);

        /// <summary>
        /// Sets a Zip Comment
        /// </summary>
        string Comment { set; }
    }
}