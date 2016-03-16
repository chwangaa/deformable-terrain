using System;
using UnityEditor;

namespace Improbable.Unity.EditorTools.PrefabExport
{
    public class EntityPrefabExportMenus
    {
        /// <summary>
        /// This is called whenever entity prefabs need to be exported. 
        /// This can be done from within the editor, or from external sources like build systems.
        /// 
        /// By default its value is the baseline behaviour, which can be saved off and invoked
        /// as part of a custom chain of events.
        /// </summary>
        public static Action OnExportEntityPrefabs = EntityPrefabExporter.ExportEntityPrefabs;

        /// <summary>
        /// This is called whenever entity prefabs need to be cleaned. 
        /// This can be done from within the editor, or from external sources like build systems.
        /// 
        /// By default its value is the baseline behaviour, which can be saved off and invoked
        /// as part of a custom chain of events.
        /// </summary>
        public static Action OnCleanEntityPrefabs = EntityPrefabDirectoryCleaner.CleanPrefabTargetDirectories;

        [MenuItem("Improbable/Prefabs/Clean EntityPrefabs")]
        public static void CleanAllEntityPrefabs()
        {
            DisplayErrors(OnCleanEntityPrefabs);
        }

        [MenuItem("Improbable/Prefabs/Export All EntityPrefabs %&#E")]
        [MenuItem("Assets/Export All EntityPrefabs %&#E")]
        public static void ExportAllEntityPrefabs()
        {
            DisplayErrors(OnExportEntityPrefabs);
        }

        private static void DisplayErrors(Action action)
        {
            try
            {
                action();
            }
            catch(Exception)
            {
                EditorUtility.DisplayDialog("Prefab Export Error", "An error occurred. Please check your logs for more information.", "ok");
                throw;
            }
            
        }
    }
}
