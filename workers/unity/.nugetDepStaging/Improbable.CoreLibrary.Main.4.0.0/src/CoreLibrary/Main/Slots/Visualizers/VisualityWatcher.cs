using System;
using Improbable.CoreLib.Visualizers.Slots;
using Improbable.Unity.Visualizer;
using Improbable.Util.Collections;
using UnityEngine;

namespace Improbable.Corelib.Slots.Visualizers
{
    public class VisualityWatcher : MonoBehaviour
    {
        [Require] protected VisualizedSlotsReader visualizedSlots;

        private SlotContainerVisualizer slotContainerVisualizer;

        protected void OnEnable()
        {
            slotContainerVisualizer = GetComponent<SlotContainerVisualizer>();

            if (slotContainerVisualizer == null)
            {
                throw new Exception("The Visuality watcher must be used with the SlotContainerVisualizer");
            }

            visualizedSlots.SlotsUpdated += OnSlotsUpdated;
        }

        private void OnSlotsUpdated(IReadOnlyList<string> visualSlots)
        {
            slotContainerVisualizer.SetSlotsToTrack(visualSlots);
        }
    }
}
