using Improbable.Corelib.Math;
using Improbable.Corelib.Util;
using Improbable.CoreLib.Visualizers.Slots;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using Improbable.Util.Collections;
using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Improbable.Corelib.Slots.Visualizers
{
    public class DynamicSlotsVisualizer : MonoBehaviour
    {
        [Require] protected DynamicSlotsReader slots;

        protected void OnEnable()
        {
            slots.SlotsUpdated += OnSlotsUpdated;
        }

        private void OnSlotsUpdated(IReadOnlyDictionary<string, RelativeTransform> slotNamesToTransforms)
        {
            bool shouldRepopulateSlots = false;
            foreach (var slotNameAndTransform in slotNamesToTransforms)
            {
                GameObject slot;
                if (!SlotLocator.TryGetSlot(gameObject, slotNameAndTransform.Key, out slot))
                {
                    shouldRepopulateSlots = true;
                    slot = SlotLocator.CreateSlot(gameObject, slotNameAndTransform.Key);
                }
                SetLocalTransform(slot, slotNameAndTransform.Value);
            }

            if (shouldRepopulateSlots)
            {
                GetComponent<SlotContainerVisualizer>().RefreshSlots();
            }
        }

        private static void SetLocalTransform(GameObject slot, RelativeTransform newSlotTransform)
        {
            var slotTransform = slot.GetComponent<Transform>();
            slotTransform.localPosition = newSlotTransform.Position.ToUnityVector();
            slotTransform.localRotation = newSlotTransform.Rotation.ToUnityQuaternion();
        }
    }
}
