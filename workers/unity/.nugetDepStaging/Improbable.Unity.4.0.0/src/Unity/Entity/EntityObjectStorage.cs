using Improbable.Unity.Entity;
using UnityEngine;

namespace Improbable.Core.Entity
{
    /// <summary>
    /// Used to associate an Unity Object with our EntityObject
    /// </summary>
    public class EntityObjectStorage : MonoBehaviour
    {
        // Here so the UnityEditor can display the ID
        // as it cant display longs
        public string EntityIdentifier
        {
            get { return entityId.ToString(); }
        }

        public IEntityObject Entity { get; private set; }
        public EntityId entityId; //For Debugging purposes

        public EntityId EntityId
        {
            get { return entityId; }
        }

        public void Initialize(IEntityObject entityObject)
        {
            Entity = entityObject;
            entityId = Entity.EntityId;
        }

        public void Clear()
        {
            GameObjectExtensions.RemoveFromLookupCache(Entity);
            Entity = null;
            entityId = EntityId.InvalidEntityId;
        }
    }
}
