using UnityEditor;

namespace Improbable.Unity.EditorTools.Build
{
    [InitializeOnLoad]
    public static class AssemblyPatchInvoker
    {
        private const string SETTING_KEY = "Improbable.AutoPatchPlayerAssemblies";

        static AssemblyPatchInvoker()
        {
            if (EditorPrefs.GetBool(SETTING_KEY, false))
            {
                UnityPlayerBuilderMenu.IncrementallyPatchAllPlayers();
            }
        }

        [MenuItem("Improbable/Build/Enable Auto Assembly Patching", priority = -1)]
        public static void Enable()
        {
            UnityPlayerBuilderMenu.IncrementallyPatchAllPlayers();
            EditorPrefs.SetBool(SETTING_KEY, true);
        }

        [MenuItem("Improbable/Build/Enable Auto Assembly Patching", true, priority = -1)]
        public static bool CanEnable()
        {
            return !EditorPrefs.GetBool(SETTING_KEY, false);
        }

        [MenuItem("Improbable/Build/Disable Auto Assembly Patching", priority = -1)]
        public static void Disable()
        {
            EditorPrefs.SetBool(SETTING_KEY, false);
        }

        [MenuItem("Improbable/Build/Disable Auto Assembly Patching", true, priority = -1)]
        public static bool CanDisable()
        {
            return EditorPrefs.GetBool(SETTING_KEY, false);
        }
    }
}