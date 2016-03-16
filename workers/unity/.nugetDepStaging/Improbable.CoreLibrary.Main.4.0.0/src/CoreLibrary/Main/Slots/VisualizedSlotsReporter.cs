using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Slots
{
    public class VisualizedSlotsReporter : MonoBehaviour
    {
        [Require] protected VisualizedSlotsWriter State;

        public List<string> FoundSlots = new List<string>();
        public IList<string> StateSlots = new List<string>();

        protected void OnEnable()
        {
            FoundSlots = SlotLocator.FindSlotNamesWithin(gameObject);
            StateSlots = State.Slots.ToList();

            if (FoundSlots.Count() != StateSlots.Count())
            {
                State.Update.Slots(FoundSlots).FinishAndSend();
            }
            else if (FoundSlots.Intersect(StateSlots).Count() != FoundSlots.Count())
            {
                State.Update.Slots(FoundSlots).FinishAndSend();
            }
        }
    }
}