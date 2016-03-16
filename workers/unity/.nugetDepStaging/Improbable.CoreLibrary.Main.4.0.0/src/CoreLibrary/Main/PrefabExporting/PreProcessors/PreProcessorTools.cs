using UnityEngine;

namespace Improbable.Corelib.PrefabExporting.PreProcessors
{
    class PreProcessorTools
    {
        /// <summary>
        /// Decides whether the ClientPlayer has authority over given
        /// gameobject or not.
        /// </summary>
        public static bool IsPlayerPrefab(GameObject targetGameObject)
        {
            return targetGameObject.name.EndsWith("@Player");
        }
    }
}
