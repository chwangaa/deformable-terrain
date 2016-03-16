using System.Collections.Generic;
using Improbable.Core;
using Improbable.Core.Network;
using Improbable.Entity.State;
using Improbable.Fapi.Protocol;
using Improbable.Unity.Common.Entity.State;
using Improbable.Unity.State;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Unity.Entity
{
    // TODO: Consider merging with AddEntityMessageProcessor
    public class UnityEntityFactory : IEntityFactory
    {
        private readonly IBridgeCommunicator bridgeCommunicator;
        private readonly IContainer container;
        private readonly IPrefabFactory<GameObject> prefabFactory;
        private readonly IEntityTemplateProvider templateProvider;

        public UnityEntityFactory(IBridgeCommunicator bridgeCommunicator,
                                  IContainer container,
                                  IPrefabFactory<GameObject> prefabFactory,
                                  IEntityTemplateProvider templateProvider)
        {
            this.bridgeCommunicator = bridgeCommunicator;
            this.container = container;
            this.prefabFactory = prefabFactory;
            this.templateProvider = templateProvider;
        }

        public IEntityObject MakeEntity(EntityId entityId, string prefabName, string context)
        {
            var entityAssetId = new EntityAssetId(prefabName, context);

            var loadedPrefab = templateProvider.GetEntityTemplate(entityAssetId);
            var prefab = prefabFactory.MakeComponent(loadedPrefab, entityAssetId);

            var visualizerExtractor = new VisualizerExtractor(prefab, VisualizerMetadataLookup.Instance);
            var extractVisualizers = visualizerExtractor.ExtractVisualizers();

            var updateStateSender = bridgeCommunicator as IStateSender;

            var entityStateContainer = new EntityStateContainer(entityId, updateStateSender, StateMetadataLookup.Instance);
            var monoBehaviourActivator = new MonoBehaviourActivator();
            var entityVisualizers = new EntityVisualizers(container, extractVisualizers, monoBehaviourActivator, VisualizerMetadataLookup.Instance);
            entityStateContainer.AddSubscriber(entityVisualizers);
            var entity = new EntityObject(entityId, prefab, entityAssetId, prefabFactory, entityVisualizers, entityStateContainer);

            UpdateInterestedStates(entityId, entityVisualizers.RequiredStates);
            entityVisualizers.OnRequiredStatesUpdated += stateNames => UpdateInterestedStates(entityId, stateNames);

            return entity;
        }

        private void UpdateInterestedStates(EntityId entityId, IEnumerable<string> stateNames)
        {
            InterestedStates.Enqueue(bridgeCommunicator, entityId, stateNames);
        }
    }
}
