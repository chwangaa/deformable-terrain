using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Core.Entity;
using Improbable.Core.GameLogic.Visualizers;
using Improbable.Corelib.Physical;
using Improbable.CoreLib.Physical.Visualizers;
using Improbable.Corelib.Slots;
using Improbable.Corelib.Slots.Visualizers;
using Improbable.TestGameLogic.Entities.Visualizers.Physical;
using Improbable.Unity.Visualizer;
using Improbable.Util.Collections;
using IoC;
using UnityEngine;

namespace Improbable.CoreLib.Visualizers.Slots
{
    public class SlotContainerVisualizer : MonoBehaviour
    {
        // NOTE: This cache is used to avoid allocation of a new list on every Update
        private readonly List<string> entitiesToPurgeCache = new List<string>();

        private bool potentiallyDirty = true;

        private HashSet<string> SlotsToTrack = null;

        /// <summary>
        ///     Contains the mapping from slot name to entity id of the currently
        ///     mounted entities (from the perspective of Unity)
        /// </summary>
        private IDictionary<string, EntityId> mountedEntities = new Dictionary<string, EntityId>();

        [Require] protected SlotsReader slots;


        /// <summary>
        ///     The list of visualizers that need to be enabled/disabled when mounting
        /// </summary>
        private readonly List<Type> visualizers = new List<Type>
        {
            typeof(RigidbodyVisualizer),
            typeof(InterpolatingPositionVisualizer),
            typeof(InterpolatingRotationVisualizer),
            typeof(RigidbodyPositionVisualizer),
            typeof(RigidbodyRotationVisualizer),
            typeof(PositionVisualizer),
            typeof(TargetRotationVisualizer),
            typeof(RigidbodySync),
            typeof(GroundedChecker),
            typeof(InitialPositionVisualizer),
            typeof(InitialRotationVisualizer)
        };

        /// <summary>
        ///     The game objects marked as childSlots that were found in the
        ///     children (recursive) of this game object
        /// </summary>
        private IDictionary<string, GameObject> childSlots;


        [Inject]
        public IUniverse Universe { private get; set; }


        protected void OnEnable()
        {
            RefreshSlots();
            slots.SlotToEntityIdUpdated += OnSlotsDataChanged;
            mountedEntities = new Dictionary<string, EntityId>();
        }

        private void OnSlotsDataChanged(IReadOnlyDictionary<string, EntityId> obj)
        {
            potentiallyDirty = true;
        }

        public void OnDisable()
        {
            SlotsToTrack = null;
            UnmountAllChildren();
        }

        public void RefreshSlots()
        {
            childSlots = SlotLocator.FindSlotGameObjects(gameObject).ToDictionary(s => s.name.Substring(1), s => s);
            potentiallyDirty = true;
        }

        public void SetSlotsToTrack(IReadOnlyList<string> importantSlots)
        {
            SlotsToTrack = new HashSet<string>(importantSlots);
            potentiallyDirty = true;
        }

        private void UnmountAllChildren()
        {
            foreach (var slotToEntity in mountedEntities)
            {
                Unmount(slotToEntity.Key, EntityGameObject(slotToEntity.Value));
            }
        }

        protected void Update()
        {
            if (potentiallyDirty)
            {
                var incorrectlyMountedEntities = UnmountEntities();
                var incorrectlyNotMountedEntities = MountEntities();
                potentiallyDirty = incorrectlyMountedEntities || incorrectlyNotMountedEntities;
            }
        }

        private bool UnmountEntities()
        {
            var outstandingChanges = false;
            entitiesToPurgeCache.Clear();
            foreach (var currentSlotToEntity in mountedEntities)
            {
                if (ShouldUnmount(currentSlotToEntity, slots.SlotToEntityId))
                {
                    if (Unmount(currentSlotToEntity.Key, EntityGameObject(currentSlotToEntity.Value)))
                    {
                        entitiesToPurgeCache.Add(currentSlotToEntity.Key);
                    }
                    else
                    {
                        outstandingChanges = true;
                    }
                }
            }
            PurgeUnmountedEntities();
            return outstandingChanges;
        }

        private bool MountEntities()
        {
            var outstandingChanges = false;
            foreach (var newSlotToEntity in slots.SlotToEntityId)
            {
                if (ShouldMount(newSlotToEntity))
                {
                    var slotId = newSlotToEntity.Key;
                    var entityId = newSlotToEntity.Value;

                    if (Mount(slotId, EntityGameObject(entityId)))
                    {
                        mountedEntities.Add(newSlotToEntity);
                    }
                    else
                    {
                        outstandingChanges = outstandingChanges || IsTrackingSlot(slotId);
                    }
                }
            }
            return outstandingChanges;
        }

        private bool IsTrackingSlot(string slotId)
        {
            return IsTrackingAllSlots || SlotsToTrack.Contains(slotId);
        }

        private bool IsTrackingAllSlots
        {
            get { return SlotsToTrack == null; }
        }

        private void PurgeUnmountedEntities()
        {
            foreach (string entityId in entitiesToPurgeCache)
            {
                mountedEntities.Remove(entityId);
            }
        }

        private bool ShouldMount(KeyValuePair<string, EntityId> newSlotToEntity)
        {
            EntityId value;
            return !(mountedEntities.TryGetValue(newSlotToEntity.Key, out value) &&
                     value.Equals(newSlotToEntity.Value));
        }

        private bool ShouldUnmount(KeyValuePair<string, EntityId> currentSlotToEntity, IReadOnlyDictionary<string, EntityId> newSlotToEntities)
        {
            EntityId value;
            return !(newSlotToEntities.TryGetValue(currentSlotToEntity.Key, out value) &&
                     currentSlotToEntity.Value.Equals(value));
        }

        private GameObject EntityGameObject(EntityId entityId)
        {
            var entity = Universe.Get(entityId);
            return entity != null ? entity.UnderlyingGameObject : null;
        }

        private bool Mount(string slot, GameObject gameObject)
        {
            if (gameObject == null)
            {
                return false;
            }

            if (SlotLocator.IsWithinSlot(gameObject))
            {
                return false;
            }

            if (!childSlots.ContainsKey(slot))
            {
                return false;
            }

            var parent = childSlots[slot].transform;
            gameObject.transform.parent = parent;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;

            AddSlottedChildBehaviour(slot, gameObject);
            DisableConflictingVisalizers(gameObject);

            return true;
        }

        private void AddSlottedChildBehaviour(string slot, GameObject gameObject)
        {
            var comp = gameObject.GetComponent<DisabledWatcher>() ?? gameObject.AddComponent<DisabledWatcher>();
            comp.slot = slot;
            comp.entityId = gameObject.EntityId();
            comp.parent = this;
        }

        private void RemoveSlottedChildBehavour(GameObject gameObject)
        {
            var child = gameObject.GetComponent<DisabledWatcher>();
            DestroyObject(child);
        }

        private bool Unmount(string slot, GameObject mountedGameObject)
        {
            if (mountedGameObject == null)
            {
                return false;
            }
            if (!childSlots.Keys.Contains(slot))
            {
                return false;
            }

            RemoveSlottedChildBehavour(gameObject);
            EnableConfictingVisualizers(mountedGameObject);
            mountedGameObject.transform.parent = null;

            return true;
        }

        public void OnChildDisabled(DisabledChild disabledChild)
        {
            mountedEntities.Remove(new KeyValuePair<string, EntityId>(disabledChild.slot, disabledChild.entityId));
            RemoveSlottedChildBehavour(disabledChild.gameObject);
        } 


        private void DisableConflictingVisalizers(GameObject gameObject)
        {
            ToggleConflictingVisalizers(gameObject, false);
        }


        private void EnableConfictingVisualizers(GameObject gameObject)
        {
            ToggleConflictingVisalizers(gameObject, true);
        }

        private void ToggleConflictingVisalizers(GameObject childGameObject, bool toState)
        {
            var entity = childGameObject.GetEntityObject();

            if (entity == null)
            {
                return;
            }

            var visualizersToDisable = new List<object>();
            var visualizersToEnable = new List<object>();
            // ReSharper disable once ForCanBeConvertedToForeach : efficiency reasons
            for (var index = 0; index < visualizers.Count; ++index)
            {
                var visualizer = visualizers[index];
                var comp = childGameObject.GetComponent(visualizer);
                if (comp != null)
                {
                    if (toState)
                    {
                        visualizersToEnable.Add(comp);
                    }
                    else
                    {
                        visualizersToDisable.Add(comp);
                    }
                }
            }
            entity.TryEnableVisualizers(visualizersToEnable);
            entity.DisableVisualizers(visualizersToDisable);
        }
    }
}