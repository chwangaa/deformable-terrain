using System;
using System.IO;
using Improbable.Unity.EditorTools.Util;
using Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter;
using Improbable.Unity.EditorTools.WorldExportWizard.ObjExporter.TerrainExporter;

namespace Improbable.Unity.EditorTools.WorldExportWizard
{
    internal class WorldExporter
    {
        public static void Export(string worldEnviromentName, bool exportWorldAssetBundle, bool exportAutodeskNavmesh, bool exportJson, TerrainSaveResolution terrainSaveResolution)
        {
            ClearExportDirectories(worldEnviromentName);
            CreateExportDirectories(worldEnviromentName);
            var exportedEntities = EntityExportUtil.DisableExportedEntities();

            if (exportWorldAssetBundle)
            {
                WorldAssetBundleExporter.Export(worldEnviromentName);
            }

            EntityExportUtil.ReenableExportedEntities(exportedEntities);
        }

        private static void ClearExportDirectories(String worldEnvironmentName)
        {
            try
            {
                Directory.Delete(EditorPaths.ResourcesDirectory(worldEnvironmentName), true);
            }
            catch (DirectoryNotFoundException) {}

            try
            {
                Directory.Delete(EditorPaths.TerrainDirectory(worldEnvironmentName), true);
            }
            catch (DirectoryNotFoundException) {}
        }

        private static void CreateExportDirectories(String worldEnvironmentName)
        {
            Directory.CreateDirectory(EditorPaths.ResourcesDirectory(worldEnvironmentName));
            Directory.CreateDirectory(EditorPaths.TerrainDirectory(worldEnvironmentName));
        }
    }
}