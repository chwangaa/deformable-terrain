using UnityEditor;

namespace Improbable.Unity.EditorTools.PrefabExport
{
    [InitializeOnLoad]
    internal static class IncrementalExportInvoker
    {
        private const string SETTING_KEY = "Improbable.AutomaticallyExportEntityPrefabs";
        private static int counter = 0;

        static IncrementalExportInvoker()
        {
            EditorApplication.update += Update;
        }

        [MenuItem("Improbable/Prefabs/Enable Auto Prefab Export", priority = -1)]
        public static void Enable()
        {
            EditorPrefs.SetBool(SETTING_KEY, true);
        }

        [MenuItem("Improbable/Prefabs/Enable Auto Prefab Export", true, priority = -1)]
        public static bool CanEnable()
        {
            return !EditorPrefs.GetBool(SETTING_KEY, false);
        }

        [MenuItem("Improbable/Prefabs/Disable Auto Prefab Export", priority = -1)]
        public static void Disable()
        {
            EditorPrefs.SetBool(SETTING_KEY, false);
        }

        [MenuItem("Improbable/Prefabs/Disable Auto Prefab Export", true, priority = -1)]
        public static bool CanDisable()
        {
            return EditorPrefs.GetBool(SETTING_KEY, false);
        }

        private static void Update()
        {
            counter++;
            if (counter > 100)
            {
                if (EditorPrefs.GetBool(SETTING_KEY, false))
                {
                    EntityPrefabExporter.ExportEntityPrefabs();
                }
                counter = 0;
            }
        }
    }
}