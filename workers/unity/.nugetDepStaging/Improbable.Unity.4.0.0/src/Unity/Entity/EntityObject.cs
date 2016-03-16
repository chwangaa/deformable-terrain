using System.Collections.Generic;
using Improbable.Core.Entity;
using Improbable.Entity.State;
using UnityEngine;

namespace Improbable.Unity.Entity
{
    public class EntityObject : IEntityObject
    {
        private readonly EntityObjectStorage entityObjectStorage;
        private readonly EntityAssetId assetId;
        private readonly IPrefabFactory<GameObject> prefabFactory;
        private readonly IEntityVisualizers entityVisualizers;

        public GameObject UnderlyingGameObject { get; private set; }
        public EntityId EntityId { get; private set; }
        public IEntityStateContainer EntityStateContainer { get; private set; }

        public EntityObject(EntityId entityId,
                            GameObject gameObject,
                            EntityAssetId assetId,
                            IPrefabFactory<GameObject> prefabFactory,
                            IEntityVisualizers entityVisualizers, 
                            IEntityStateContainer entityStateContainer)
        {
            EntityId = entityId;
            UnderlyingGameObject = gameObject;
            this.assetId = assetId;
            this.prefabFactory = prefabFactory;
            this.entityVisualizers = entityVisualizers;
            EntityStateContainer = entityStateContainer;

            entityObjectStorage = 
                UnderlyingGameObject.GetComponent<EntityObjectStorage>() ?? 
                UnderlyingGameObject.AddComponent<EntityObjectStorage>();
            entityObjectStorage.Initialize(this);
        }

        public override string ToString()
        {
            return string.Format("Entity: {0}, id: {1} prefab: {2}", UnderlyingGameObject.name, EntityId, assetId.PrefabName);
        }

        public void Destroy()
        {
            entityVisualizers.Dispose();
            entityObjectStorage.Clear();
            prefabFactory.DespawnComponent(UnderlyingGameObject, assetId);
        }

        public void DisableVisualizers(IList<object> visualizers)
        {
            entityVisualizers.DisableVisualizers(visualizers);
        }

        public void TryEnableVisualizers(IList<object> visualizers)
        {
            entityVisualizers.TryEnableVisualizers(visualizers);
        }
    }
}
