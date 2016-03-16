using System.IO;
using Assets.Improbable.Gel.Util;

namespace Improbable.Unity.Util
{
    public static class UnityPrefabPaths
    {
        public const string SPATIALOS_JSON = "spatialos.json";
        public const string ENTITY_PREFAB_FOLDER_NAME = "EntityPrefabs";
        public const string ENTITY_PREFAB_FOLDER_PATH = "Assets/Resources/" + ENTITY_PREFAB_FOLDER_NAME;

        public static string GetPrefabPathRelativeToResources(string prefabName)
        {
            return ENTITY_PREFAB_FOLDER_NAME + "/" + prefabName;
        }

        public static string GetPrefabPathRelativeToProjectRoot(string prefabName)
        {
            return ENTITY_PREFAB_FOLDER_PATH + "/" + prefabName;
        }

        public static bool IsDirectoryUnderSpatialProject(string searchDirectory)
        {
            return GetSpatialProjectRoot(searchDirectory) != null;
        }

        public static string GetSpatialProjectRoot(string searchDirectory)
        {
            while (searchDirectory != null && !File.Exists(Path.Combine(searchDirectory, SPATIALOS_JSON)))
            {
                var parent = Directory.GetParent(searchDirectory);
                searchDirectory = parent != null ? parent.FullName : null;
            }

            return searchDirectory;
        }

    }
}
