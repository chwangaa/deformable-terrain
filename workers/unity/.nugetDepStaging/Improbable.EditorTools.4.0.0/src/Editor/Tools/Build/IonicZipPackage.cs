using System;
using System.IO;
using Ionic.Zip;

namespace Improbable.Unity.EditorTools.Build
{
    internal class IonicZipPackage : IZipPackage
    {
        private readonly ZipFile zip;

        public IonicZipPackage(string absoluteZipPath)
        {
            // see https://dotnetzip.codeplex.com/workitem/14087
            zip = new ZipFile(absoluteZipPath) { ParallelDeflateThreshold = -1 };
        }

        public void AddDirectory(string basePath, string subFolder)
        {
            zip.AddDirectory(Path.Combine(basePath, subFolder), subFolder);
        }

        public void AddDirectory(string basePath, string subFolder, string filePattern)
        {
            throw new NotImplementedException(
                string.Format("{0} does not support file patterns when adding a directory", typeof(IonicZipPackage).Name));
        }

        public string Comment
        {
            get { return zip.Comment; }
            set { zip.Comment = value; }
        }

        public void Dispose()
        {
            try
            {
                zip.Save();
            }
            finally
            {
                zip.Dispose();
            }
        }

        public void UpdateFile(string assemblyPath, string assemblyPathWithinPackage)
        {
            zip.UpdateFile(assemblyPath, assemblyPathWithinPackage);
        }
    }
}