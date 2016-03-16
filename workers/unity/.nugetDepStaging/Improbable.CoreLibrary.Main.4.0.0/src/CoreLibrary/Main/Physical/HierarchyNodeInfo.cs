using Improbable.Corelib.Slots;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Physical
{
    public class HierarchyNodeInfo : MonoBehaviour
    {
        [Require] protected HierarchyNodeReader HierarchyNode;

        protected void OnEnable()
        {
            var hierarchyNodeRegistrables = GetComponents<IHierarchyNodeRegistrable>();

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < hierarchyNodeRegistrables.Length; i++)
            {
                hierarchyNodeRegistrables[i].RegisterHierarchyNode(HierarchyNode);
            }
        }
    }
}
