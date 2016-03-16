using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Assets.Improbable.Gel.Util;
using Improbable.Unity.EditorTools.Util;
using Improbable.Util.IO;
using Debug = UnityEngine.Debug;

namespace Improbable.Unity.EditorTools.Build
{
    internal class Windows7ZipPackage : IZipPackage
    {
        private const string ZipExecutable = "7z.exe";
        private readonly string zipAbsolutePath;
        private string comment;

        public Windows7ZipPackage(string zipAbsolutePath)
        {
            this.zipAbsolutePath = zipAbsolutePath;
        }

        public static bool IsSupported
        {
            get { return SystemPath.ExistsOnPath(ZipExecutable); }
        }

        public void AddDirectory(string basePath, string subFolder)
        {
            AddDirectory(basePath, subFolder, null);
        }

        public void AddDirectory(string basePath, string subFolder, string filePattern)
        {
            if (!Directory.Exists(basePath))
            {
                throw new Exception(string.Format("Your working directory {0} does not exist, aborting adding directory with 7zip!", basePath));
            }

            var zipProcess = StartZipProcess(basePath, subFolder, filePattern);
            var output = zipProcess.StandardOutput.ReadToEnd();
            var errOut = zipProcess.StandardError.ReadToEnd();
            zipProcess.WaitForExit();
            if (zipProcess.ExitCode != 0)
            {
                throw new Exception(string.Format("Could not package the folder {0}/{1}. The following error occurred: {2}, {3}\n", basePath, subFolder, output, errOut));
            }
        }

        public string Comment
        {
            set { comment = value; }
        }


        public void Dispose()
        {
            using (var tmpZip = new IonicZipPackage(zipAbsolutePath))
            {
                tmpZip.Comment = comment;
            }
        }

        private Process StartZipProcess(string basePath, string subFolder, string filePattern)
        {
            var p = ConfigureZipProcess(basePath, subFolder, filePattern);
            try
            {
                p.Start();
            }
            catch (Win32Exception e)
            {
                Debug.Log(string.Format("{0} could not launch. Triggering exception: Error Code 0x{1:X8}\n{2}\n", ZipExecutable, e.ErrorCode, e.Message));
                throw;
            }
            return p;
        }

        private Process ConfigureZipProcess(string basePath, string subFolder, string filePattern)
        {
            var zipExecutableFullPath = SystemPath.GetFullProgramPath(ZipExecutable);
            var zipFileFullPath = Path.GetFullPath(zipAbsolutePath);
            var workingDir = Path.GetFullPath(basePath);

            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = zipExecutableFullPath,
                    Arguments = ZipArgs(subFolder, filePattern, zipFileFullPath),
                    WorkingDirectory = workingDir,
                    CreateNoWindow = true
                }
            };
            return p;
        }

        private static string ZipArgs(string subFolder, string filePattern, string zipFileFullPath)
        {
            var zipArgs = string.IsNullOrEmpty(filePattern) ?
                string.Format("a -tzip {0} {1}", zipFileFullPath, subFolder) :
                string.Format("a -tzip {0} -ir!{1}{2}", zipFileFullPath, PathUtil.EnsureTrailingSlash(subFolder), filePattern);
            return zipArgs;
        }
    }
}