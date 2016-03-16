using System.Collections.Generic;
using Improbable;
using Improbable.Core.Entity;
using Improbable.Entity;
using Improbable.Unity.Entity;

// ReSharper disable CheckNamespace
namespace UnityEngine
// ReSharper restore CheckNamespace
{
    public static class GameObjectExtensions
    {
        private static readonly Dictionary<GameObject, IEntityObject> GameObjectToEntityObjectCache = new Dictionary<GameObject, IEntityObject>();
        private static readonly Dictionary<IEntityObject, List<GameObject>> EntityObjectToGameObjectCache = new Dictionary<IEntityObject, List<GameObject>>();

        public static EntityId EntityId(this GameObject obj)
        {
            var entityObject = FindEntityObject(obj);
            return entityObject == null ? Improbable.EntityId.InvalidEntityId : entityObject.EntityId;
        }

        public static bool IsEntityObject(this GameObject obj)
        {
            return obj.GetComponent<EntityObjectStorage>() != null;
        }

        /// <summary>
        /// Finds Unity's game object that belongs to the entity. The result is cached.
        /// To clean the cache, call the <see cref="RemoveFromLookupCache"/> method.
        /// </summary>
        public static IEntityObject GetEntityObject(this GameObject gameObject)
        {
            IEntityObject entityObject;
            if (GameObjectToEntityObjectCache.TryGetValue(gameObject, out entityObject))
            {
                return entityObject;
            }
            entityObject = FindEntityObject(gameObject);
            if (entityObject != null)
            {
                GameObjectToEntityObjectCache.Add(gameObject, entityObject);
                AddToReverseLookup(gameObject, entityObject);
            }
            return entityObject;
        }

        private static void AddToReverseLookup(GameObject gameObject, IEntityObject entityObject)
        {
            List<GameObject> gameObjects;
            if (!EntityObjectToGameObjectCache.TryGetValue(entityObject, out gameObjects))
            {
                gameObjects = new List<GameObject>();
                EntityObjectToGameObjectCache[entityObject] = gameObjects;
            }
            gameObjects.Add(gameObject);
        }

        private static IEntityObject FindEntityObject(GameObject gameObject)
        {
            var currentGameObject = gameObject;
            while (currentGameObject != null)
            {
                if (currentGameObject.IsEntityObject())
                {
                    return currentGameObject.GetComponent<EntityObjectStorage>().Entity;
                }

                currentGameObject = currentGameObject.transform.parent
                    ? currentGameObject.transform.parent.gameObject
                    : null;
            }
            return null;
        }

        public static void RemoveFromLookupCache(IEntityObject entityObject)
        {
            if (entityObject != null)
            {
                List<GameObject> objectsToRemove;
                if (EntityObjectToGameObjectCache.TryGetValue(entityObject, out objectsToRemove))
                {
                    for (int index = 0; index < objectsToRemove.Count; index++)
                    {
                        var invalidObject = objectsToRemove[index];
                        if (invalidObject != null)
                        {
                            GameObjectToEntityObjectCache.Remove(invalidObject);
                        }
                    }
                    EntityObjectToGameObjectCache.Remove(entityObject);
                }
            }
        }
    }
}