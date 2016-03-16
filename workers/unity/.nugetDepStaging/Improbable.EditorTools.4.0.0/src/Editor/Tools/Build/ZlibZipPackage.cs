using System;
using System.IO;
using Assets.Improbable.Gel.Util;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Improbable.Unity.EditorTools.Build;
using Improbable.Unity.EditorTools.Util;

namespace Assets.Editor.Improbable.EditorTools.Build
{
    /// <summary>
    ///     Implementation of IZipPackage which uses the zlib compression library
    /// </summary>
    internal class ZlibZipPackage : IZipPackage
    {
        private const int ZipBufferSize = 4096;
        private const string AllFilesSearchPattern = "*";

        private readonly ZipOutputStream zipStream;

        public ZlibZipPackage(string path)
        {
            var stream = File.Create(new Uri(path).LocalPath);
            zipStream = new ZipOutputStream(stream) { IsStreamOwner = true };
        }

        public void Dispose()
        {
            zipStream.Close();
        }

        public void AddDirectory(string basePath, string subFolder)
        {
            AddDirectory(basePath, subFolder, "*");
        }

        public void AddDirectory(string basePath, string subFolder, string searchPattern)
        {
            if (string.IsNullOrEmpty(searchPattern))
            {
                searchPattern = AllFilesSearchPattern;
            }
            var subFolderPath = Path.Combine(basePath, subFolder);
            var basePathUri = new Uri(PathUtil.EnsureTrailingSlash(basePath));

            foreach (var file in Directory.GetFiles(subFolderPath, searchPattern, SearchOption.AllDirectories))
            {
                AddFileToZip(basePathUri, file);
            }
        }

        public string Comment
        {
            set { zipStream.SetComment(value); }
        }

        private void AddFileToZip(Uri basePathUri, string currentFile)
        {
            var currentFileUri = new Uri(currentFile);
            var relativeFilePath = basePathUri.MakeRelativeUri(currentFileUri);

            AddZipEntry(currentFile, relativeFilePath);
            CopyFileStreamToZip(currentFile);
            zipStream.CloseEntry();
        }

        private void AddZipEntry(string currentFile, Uri relativeFilePath)
        {
            var zipEntry = CreateZipEntry(currentFile, relativeFilePath);
            zipStream.PutNextEntry(zipEntry);
        }

        private static ZipEntry CreateZipEntry(string currentFile, Uri relativeFilePath)
        {
            var currentFileInfo = new FileInfo(currentFile);
            var zipEntryName = ZipEntry.CleanName(relativeFilePath.ToString());
            var newEntry = new ZipEntry(zipEntryName) { DateTime = currentFileInfo.LastWriteTime };
            return newEntry;
        }

        private void CopyFileStreamToZip(string currentFile)
        {
            var buffer = new byte[ZipBufferSize];
            using (var streamReader = File.OpenRead(currentFile))
            {
                StreamUtils.Copy(streamReader, zipStream, buffer);
            }
        }
    }
}