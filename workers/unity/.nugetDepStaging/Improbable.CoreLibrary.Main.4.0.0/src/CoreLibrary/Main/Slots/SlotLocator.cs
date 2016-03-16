using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Improbable.Corelib.Slots
{
    public static class SlotLocator
    {
        public const string SLOT_GAME_OBJECT_NAME_PREFIX = "#";

        public static List<GameObject> FindSlotGameObjects(GameObject target)
        {
            var transform = target.GetComponent<Transform>();
            var slots = new List<GameObject>();

            foreach (Transform child in transform)
            {
                var go = child.gameObject;
                if (!go.IsEntityObject() && go != target)
                {
                    if (IsSlot(go))
                    {
                        slots.Add(go);
                    }
                    slots.AddRange(FindSlotGameObjects(go));
                }
            }

            return slots;
        }

        public static List<string> FindSlotNamesWithin(GameObject target)
        {
            return (from gameObject in FindSlotGameObjects(target)
                    select gameObject.name.Substring(1)).ToList();
        }

        public static bool IsSlot(GameObject gameObject)
        {
            return gameObject.name.StartsWith(SLOT_GAME_OBJECT_NAME_PREFIX);
        }

        public static bool IsWithinSlot(GameObject gameObject)
        {
            if (gameObject.transform != null && gameObject.transform.parent != null)
            {
                return IsSlot(gameObject.transform.parent.gameObject);
            }
            return false;
        }

        /// <param name="slotContainer">the game object of the slot container.</param>
        /// <param name="slotName">the name of the slot to look for.</param>
        /// <param name="slot">will be set to the found slot's game object, otherwise it will be set to <c>null</c>.</param>
        /// <returns><c>true</c> if the slot was found anywhere in the hierarchy, otherwise it returns <c>false</c>.</returns>
        public static bool TryGetSlot(GameObject slotContainer, string slotName, out GameObject slot)
        {
            string objName = "#" + slotName;
            var slotContainerTransform = slotContainer.GetComponent<Transform>();
            foreach (var transformOfChild in slotContainer.GetComponentsInChildren<Transform>(true))
            {
                if (transformOfChild.gameObject.name == objName && transformOfChild.parent == slotContainerTransform)
                {
                    slot = transformOfChild.gameObject;
                    return true;
                }
            }
            slot = null;
            return false;
        }

        public static GameObject CreateSlot(GameObject gameObject, string slotName)
        {
            var go = new GameObject("#" + slotName);
            go.transform.parent = gameObject.transform;
            return go;
        }
    }
}