using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Improbable.Corelibrary.PreProcessor.Global
{
    /// <summary>
    ///     Class which adds a list of Components to a GameObject if not already present.
    ///     Checking for existing Components does not look in GameObject's children.
    /// </summary>
    public class GameObjectComponentAdder
    {
        private readonly List<Type> componentsToAdd;

        public GameObjectComponentAdder(IEnumerable<Type> components)
        {
            componentsToAdd = components.ToList();
        }

        public void AddComponentsTo(GameObject gameObject)
        {
            for (int i = 0; i < componentsToAdd.Count; i++)
            {
                var component = componentsToAdd[i];
                if (gameObject.GetComponent(component) == null)
                {
                    gameObject.AddComponent(component);
                }
            }
        }
    }
}