using System;
using Improbable.CoreLib.Visualizers.Slots;
using UnityEngine;

namespace Improbable.Corelib.Slots.Visualizers
{
    public class DisabledChild
    {
        public String slot;
        public EntityId entityId;
        public GameObject gameObject;

        public DisabledChild(String slot, EntityId entityId, GameObject gameObject)
        {
            this.slot = slot;
            this.entityId = entityId;
            this.gameObject = gameObject;
        }
    }

    public class DisabledWatcher : MonoBehaviour
    {
        public EntityId entityId;
        public string slot;
        public SlotContainerVisualizer parent;

        protected void OnDisable()
        {
            if (parent && parent.gameObject.activeInHierarchy)
            {
                parent.OnChildDisabled(new DisabledChild(slot, entityId, gameObject));
            }
        }
    }
}

