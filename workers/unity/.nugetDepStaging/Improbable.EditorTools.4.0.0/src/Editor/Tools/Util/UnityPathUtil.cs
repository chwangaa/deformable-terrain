using System.IO;
using Assets.Improbable.Gel.Util;

namespace Improbable.Unity.EditorTools.Util
{
    public static class UnityPathUtil
    {
        public static void EnsureDirectoryClean(string directory)
        {
            EnsureDirectoryRemoved(directory);
            PathUtil.EnsureDirectoryExists(directory);
        }

        public static void EnsureDirectoryRemoved(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
                DeleteMetaFile(directory);
            }
        }

        public static void EnsureFileRemoved(string file)
        {
            if (File.Exists(file))
            {
                DeleteFile(file);
            }
        }

        public static void DeleteFile(string file)
        {
            File.Delete(file);
            DeleteMetaFile(file);
        }

        public static void DeleteMetaFile(string path)
        {
            var metaPath = path + ".meta";
            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
            }
        }
    }
}