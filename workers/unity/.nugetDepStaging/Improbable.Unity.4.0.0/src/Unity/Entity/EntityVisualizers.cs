using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Core;
using Improbable.Entity.State;
using Improbable.Unity.Visualizer;
using Improbable.Util.Injection;
using UnityEngine;

namespace Improbable.Unity.Entity
{
    // TODO: Consider replacing reflection invoked code with Reflection.Emit
    internal class EntityVisualizers : IEntityStateSubscriber, IEntityVisualizers
    {
        private readonly IVisualizerMetadataLookup visualizerMetadataLookup;

        private readonly IVisualizerActivator activator;
        private readonly IList<object> visualizers;
        private readonly HashSet<object> disabledVisualizers = new HashSet<object>();
        private HashSet<string> requiredStates = new HashSet<string>();
        private HashSet<string> scratchRequiredStateNames = new HashSet<string>();

        public delegate void RequiredStatesUpdated(List<string> stateNames);
        public event RequiredStatesUpdated OnRequiredStatesUpdated;

        public EntityVisualizers(IContainer container,
                                 IList<object> visualizers,
                                 IVisualizerActivator activator,
                                 IVisualizerMetadataLookup visualizerMetadataLookup)
        {
            this.activator = activator;
            this.visualizers = visualizers;
            this.visualizerMetadataLookup = visualizerMetadataLookup;
            Initialize(container);
            UpdateRequiredStateNames();
        }

        public void Dispose()
        {
            for (int i = 0; i < visualizers.Count; i++)
            {
                var visualizer = visualizers[i];
                RemoveAllStateUpdateEventHandlers(visualizer);
                activator.Deactivate(visualizer);
                NullAllFields(visualizer);
            }
        }

        public void OnStateAdded(IReadWriteEntityState state)
        {
            for (int i = 0; i < visualizers.Count; i++)
            {
                var visualizer = visualizers[i];
                Inject(visualizer, state);
            }
            UpdateRequiredStateNames();
        }

        public void OnStateRemoved(IReadWriteEntityState state)
        {
            for (int i = 0; i < visualizers.Count; i++)
            {
                var visualizer = visualizers[i];
                InjectNullAndDisable(visualizer, state.GetType());
            }
        }

        public void OnStateDelegated(IReadWriteEntityState state)
        {
            for (int i = 0; i < visualizers.Count; i++)
            {
                var visualizer = visualizers[i];
                var field = visualizerMetadataLookup.GetFieldInfo(state.GetType(), visualizer.GetType());
                if (field != null && visualizerMetadataLookup.IsWriter(field))
                {
                    InjectField(visualizer, field, state);

                    UpdateActivation(visualizer);
                }
            }
            UpdateRequiredStateNames();
        }

        public void OnStateUndelegated(IReadWriteEntityState state)
        {
            for (int i = 0; i < visualizers.Count; i++)
            {
                var visualizer = visualizers[i];
                var field = visualizerMetadataLookup.GetFieldInfo(state.GetType(), visualizer.GetType());
                if (field != null && visualizerMetadataLookup.IsWriter(field))
                {
                    InjectNullAndDisable(visualizer, field);
                }
            }
            UpdateRequiredStateNames();
        }

        public void DisableVisualizers(IList<object> visualizersToDisable)
        {
            var disabledVisualizersCount = 0;
            for (int i = 0; i < visualizersToDisable.Count; i++)
            {
                var visualizer = visualizersToDisable[i];
                if (!IsMarkedAsDisabled(visualizer))
                {
                    disabledVisualizersCount++;
                    DisableVisualizer(visualizer);
                }
            }
            if (disabledVisualizersCount > 0)
            {
                UpdateRequiredStateNames();
            }
        }

        private void DisableVisualizer(object visualizer)
        {
            disabledVisualizers.Add(visualizer);
            UpdateActivation(visualizer);
        }

        public void TryEnableVisualizers(IList<object> visualizersToEnable)
        {
            var enabledVisualizersCount = 0;
            for (int i = 0; i < visualizersToEnable.Count; i++)
            {
                object visualizer = visualizersToEnable[i];
                if (IsMarkedAsDisabled(visualizer))
                {
                    enabledVisualizersCount++;
                    EnableVisualizer(visualizer);
                }
                else
                {
                    Debug.LogWarningFormat("Visualiser {0} was not previously disabled, cannot enable.", visualizer.GetType().Name);
                }
            }
            if (enabledVisualizersCount > 0)
            {
                UpdateRequiredStateNames();
            }
        }

        private void EnableVisualizer(object visualizer)
        {
            disabledVisualizers.Remove(visualizer);
            UpdateActivation(visualizer);
        }

        /// <summary>
        ///     Returns a copy of the currently required states of the given set of visualizers.
        /// </summary>
        public IEnumerable<string> RequiredStates
        {
            get { return requiredStates; }
        }

        private void Initialize(IContainer container)
        {
            for (int i = 0; i < visualizers.Count; i++)
            {
                var visualizer = visualizers[i];
                container.Inject(visualizer);

                if (visualizerMetadataLookup.DontEnableOnStart(visualizer.GetType()))
                {
                    disabledVisualizers.Add(visualizer);
                }
                UpdateActivation(visualizer);
            }
            UpdateRequiredStateNames();
        }

        private bool IsMarkedAsDisabled(object visualizer)
        {
            return disabledVisualizers.Contains(visualizer);
        }

        private void UpdateRequiredStateNames()
        {
            CalculateRequiredStateNames();
            if (!scratchRequiredStateNames.SetEquals(requiredStates))
            {
                var tmp = scratchRequiredStateNames;
                scratchRequiredStateNames = requiredStates;
                requiredStates = tmp;
                if (OnRequiredStatesUpdated != null)
                {
                    OnRequiredStatesUpdated(requiredStates.ToList());
                }
            }
        }

        private void CalculateRequiredStateNames()
        {
            scratchRequiredStateNames.Clear();
            for (int i = 0; i < visualizers.Count; i++)
            {
                var visualizer = visualizers[i];

                if (!IsMarkedAsDisabled(visualizer) && AllFieldWritersInjected(visualizer))
                {
                    var visualizerType = visualizer.GetType();
                    var requiredReaders = visualizerMetadataLookup.GetRequiredReaderStateNames(visualizerType);
                    // NOTE: Using indexed for and Set.Add instead of UnionWith because UnionWith allocates memory (enumerators)
                    for (int stateNameIndex = 0; stateNameIndex < requiredReaders.Length; stateNameIndex++)
                    {
                        scratchRequiredStateNames.Add(requiredReaders[stateNameIndex]);
                    }
                }
            }
        }

        private void NullAllFields(object visualizer)
        {
            var fields = visualizerMetadataLookup.GetRequiredReadersWriters(visualizer.GetType());
            for (int index = 0; index < fields.Length; index++)
            {
                var fieldInfo = fields[index];
                fieldInfo.SetValue(visualizer, null);
            }
        }

        private void Inject(object visualizer, IReadWriteEntityState state)
        {
            var field = visualizerMetadataLookup.GetFieldInfo(state.GetType(), visualizer.GetType());
            if (field != null && (!visualizerMetadataLookup.IsWriter(field) || state.IsAuthoritativeHere))
            {
                InjectField(visualizer, field, state);
                UpdateActivation(visualizer);
            }
        }
        
        private void InjectField(object visualizer, IMemberAdapter field, IReadWriteEntityState state)
        {
            field.SetValue(visualizer, state);
        }

        private void UpdateActivation(object visualizer)
        {
            if (IsMarkedAsDisabled(visualizer))
            {
                activator.Deactivate(visualizer);
            }
            else if (visualizerMetadataLookup.AreAllRequiredFieldsInjectable(visualizer.GetType())
                && AllFieldReadersAndWritersInjected(visualizer))
            {
                activator.Activate(visualizer);
            }
        }

        private bool AllFieldReadersAndWritersInjected(object visualizer)
        {
            var required = visualizerMetadataLookup.GetRequiredReadersWriters(visualizer.GetType());
            return AllFieldsInjected(visualizer, required);
        }

        private bool AllFieldWritersInjected(object visualizer)
        {
            var requiredWriters = visualizerMetadataLookup.GetRequiredWriters(visualizer.GetType());
            return AllFieldsInjected(visualizer, requiredWriters);
        }

        private static bool AllFieldsInjected(object visualizer, IList<IMemberAdapter> fields)
        {
            for (var index = 0; index < fields.Count; index++)
            {
                var fieldInfo = fields[index];
                if (fieldInfo.GetValue(visualizer) == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void InjectNullAndDisable(object visualizer, Type stateType)
        {
            var field = visualizerMetadataLookup.GetFieldInfo(stateType, visualizer.GetType());
            if (field != null)
            {
                InjectNullAndDisable(visualizer, field);
            }
        }

        private void InjectNullAndDisable(object visualizer, IMemberAdapter field)
        {
            RemoveAllStateUpdateEventHandlers(visualizer);
            activator.Deactivate(visualizer);
            field.SetValue(visualizer, null);
        }

        private void RemoveAllStateUpdateEventHandlers(object visualizer)
        {
            if (visualizer == null)
            {
                throw new ArgumentNullException("visualizer");
            }

            var fields = visualizerMetadataLookup.GetRequiredReadersWriters(visualizer.GetType());
            for (int index = 0; index < fields.Length; index++)
            {
                var fieldInfo = fields[index];
                var injectedState = (IEventUnsubscriber) fieldInfo.GetValue(visualizer);
                if (injectedState != null)
                {
                    injectedState.UnsubscribeEventHandlers(visualizer);
                }
            }
        }
    }
}
