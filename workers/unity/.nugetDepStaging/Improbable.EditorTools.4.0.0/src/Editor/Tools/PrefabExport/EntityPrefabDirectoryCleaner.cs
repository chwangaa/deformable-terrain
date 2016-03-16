using System.IO;
using Improbable.Unity.EditorTools.Util;

namespace Improbable.Unity.EditorTools.PrefabExport
{
    internal static class EntityPrefabDirectoryCleaner
    {
        public static void CleanPrefabTargetDirectories()
        {
            var info = new DirectoryInfo(EditorPaths.prefabExportDirectory);
            if (info.Exists) {
                var files = info.GetFiles();
                foreach (var fileInfo in files)
                {
                    fileInfo.Delete();
                }
            }
        }
    }
}