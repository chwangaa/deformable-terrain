using System.IO;
using Improbable.Unity.EditorTools.Util;

namespace Improbable.Unity.EditorTools.Snapshots
{
    public class SnapshotLoader
    {
        private const string SNAPSHOT_EXTENSION = ".snapshot";
        private readonly SnapshotApi snapshotApi;
        private readonly string snapshotDir = Path.Combine(EditorPaths.dataDirectory, "Snapshots");

        public SnapshotLoader(string debugApiUrl, string devApiUrl)
        {
            snapshotApi = new SnapshotApi(debugApiUrl, devApiUrl);
        }

        public SnapshotResponse SaveSnapshot(string snapshotName)
        {
            CreateSnapshotDir();
            string snapshotPath = GetSnapshotFilepath(snapshotName);
            return snapshotApi.SaveSnapshot(snapshotPath);
        }

        public SnapshotResponse LoadLocalSnapshot(string snapshotName)
        {
            string snapshotPath = GetSnapshotFilepath(snapshotName);
            return snapshotApi.LoadSnapshot(snapshotPath);
        }

        private string GetSnapshotFilepath(string snapshotName)
        {
            return Path.Combine(snapshotDir, snapshotName + SNAPSHOT_EXTENSION);
        }

        private void CreateSnapshotDir()
        {
            Directory.CreateDirectory(snapshotDir);
        }
    }
}