using System;
using System.IO;
using System.Linq;
using Improbable.Unity.Util;

namespace Improbable.Unity.EditorTools.Util
{
    /// <summary>
    ///     Contains paths to all resources for the World with the given name.
    /// </summary>
    public static class EditorPaths
    {
        public const string SPATIALOS_JSON_FILE = "../../spatialos.json";
        public const string PREFAB_COMPILE_DIRECTORY = "Assets/Improbable/EntityPrefabs";
        public const string PREFAB_SOURCE_DIRECTORY = "Assets/EntityPrefabs";
        public const string PREFAB_RESOURCES_DIRECTORY = "Assets/Resources/EntityPrefabs";
        public const string WORLDSCENE_RESOURCES_DIR = "Assets/src/main/resources";
        public const string SERVER_SUFFIX = "@UnityFSim";
        public const string CLIENT_SUFFIX = "@UnityClient";

        public static string dataDirectory;
        public static string assetDatabaseDirectory;
        public static string scalaResourcesFolder;
        public static string prefabExportDirectory;

        static EditorPaths()
        {
            HasSpatialOsJson = UnityPrefabPaths.IsDirectoryUnderSpatialProject(Directory.GetCurrentDirectory());
            if (HasSpatialOsJson)
            {
                dataDirectory = "../../build";
                assetDatabaseDirectory = dataDirectory + "/assembly";
                scalaResourcesFolder = "../gsim/src/main/resources";
                prefabExportDirectory = assetDatabaseDirectory + "/unity";
            }
            else
            {
                dataDirectory = "../../../Data";
                assetDatabaseDirectory = dataDirectory + "/AssetDatabase";
                scalaResourcesFolder = "../../../gamelogic/src/main/resources";
                prefabExportDirectory = assetDatabaseDirectory + "/EntityPrefab";
            }
        }

        public static bool HasSpatialOsJson { get; private set; }
        
        public static string AssetDatabaseDirectory
        {
            get { return string.Format("{0}/{1}", Directory.GetCurrentDirectory(), assetDatabaseDirectory); }
        }

        public static string TerrainDirectory(string worldName)
        {
            return string.Format("{0}/Terrain/{1}", AssetDatabaseDirectory, worldName);
        }

        public static string ResourcesDirectory(string worldName)
        {
            return string.Format("{0}/{1}", WORLDSCENE_RESOURCES_DIR, worldName);
        }

        public static string WorldPrefabPath(string worldName)
        {
            return string.Format("{0}/{1}.prefab", ResourcesDirectory(worldName), worldName);
        }

        public static string ObjExportPath(string worldName)
        {
            return string.Format("{0}/world.obj", TerrainDirectory(worldName));
        }

        private static string WorldFilePrefix(string worldName)
        {
            return string.Format("{0}/{1}", TerrainDirectory(worldName), worldName);
        }

        public static string FSimBundleExportPath(string worldName)
        {
            return string.Format("{0}@UnityFSim.unity3d", WorldFilePrefix(worldName));
        }

        public static string ClientBundleExportPath(string worldName)
        {
            return string.Format("{0}@UnityClient.unity3d", WorldFilePrefix(worldName));
        }

        public static string EntityExportPath()
        {
            return string.Format("{0}/entityList.txt", scalaResourcesFolder);
        }
    }
}
