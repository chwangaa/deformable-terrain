using UnityEditor;
using UnityEngine;

namespace Improbable.Unity.EditorTools.Init
{
    [InitializeOnLoad]
    public class LayerCheck
    {
        static LayerCheck()
        {
            if (LayerMask.NameToLayer("[Improbable-31]") != 31)
            {
                Debug.LogError("Unity layer 31 is reserved, and should be named \"[Improbable-31]\".");
            }
        }
    }
}
