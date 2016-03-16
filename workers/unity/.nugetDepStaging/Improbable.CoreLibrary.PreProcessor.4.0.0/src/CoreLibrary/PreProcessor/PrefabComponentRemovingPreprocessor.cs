using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Unity;
using Improbable.Unity.Export;
using Improbable.Unity.Visualizer;
using log4net;
using UnityEngine;

// ReSharper disable RedundantTypeArgumentsOfMethod Type arguments required for Mono

namespace Improbable.Corelib.PreProcessors
{
    /// <summary>
    ///     A Pre-Processor which can be added to Prefabs in order to optimize it for different EnginePlatforms.
    ///     It should be run before the prefab is instantiated into a game object, which can be done at export or load time.
    ///     It currently removes Client only Components when exporting to an FSim and strips Visualizers tagged for different
    ///     EnginePlatforms.
    /// </summary>
    public class PrefabComponentRemovingPreprocessor : MonoBehaviour, IPrefabExportProcessor
    {
        private static readonly Type[] FSimComponentsToRemove =
        {
            typeof(Renderer),
            typeof(ParticleSystem),
            typeof(MeshFilter),
            typeof(AudioSource)
        };

        private static readonly Type[] EmptyTypeArray = new Type[0];
        private static readonly List<Component> EmptyComponentList = new List<Component>(0);
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PrefabComponentRemovingPreprocessor));
        private EnginePlatform engine;

        public void ExportProcess(EnginePlatform engine)
        {
            this.engine = engine;
            RemoveComponents(GetDifferentEngineTypedComponents);
            RemoveComponents(GetComponentInstancesToRemove);
        }

        protected virtual IEnumerable<Type> GetComponentsToRemove(EnginePlatform engine)
        {
            if (engine == EnginePlatform.FSim)
            {
                return FSimComponentsToRemove;
            }
            else
            {
                return EmptyTypeArray;
            }
        }

        private void RemoveComponents(Func<List<Component>> getComponentsToRemove)
        {
            var previouslyFoundComponents = EmptyComponentList;
            var currentlyFoundComponents = getComponentsToRemove();

            while (previouslyFoundComponents.Count != currentlyFoundComponents.Count)
            {
                DestroyComponents(currentlyFoundComponents);
                previouslyFoundComponents = currentlyFoundComponents;
                currentlyFoundComponents = getComponentsToRemove();
            }
            if (currentlyFoundComponents.Count != 0)
            {
                LogUnremovableComponents(currentlyFoundComponents);
            }
        }

        private void LogUnremovableComponents(List<Component> components)
        {
            var unremovableComponents = string.Join(", ", components.Select(c => c.name).ToArray());
            Logger.WarnFormat("GameObject {0}: Could not remove all Components. {1} still have references to them. See logs for more info.", gameObject, unremovableComponents);
        }

        private void DestroyComponents(List<Component> components)
        {
            for (int i = 0; i < components.Count; i++)
            {
                var component = components[i];
                DestroyComponent(component);
            }
        }

        private List<Component> GetDifferentEngineTypedComponents()
        {
            List<Component> differentEngineTypedComponents = new List<Component>();
            var allBehaviours = gameObject.GetComponentsInChildren<Component>(true);
            for (int i = 0; i < allBehaviours.Length; i++)
            {
                var component = allBehaviours[i];
                if (component != null && HasDifferentEngineTypeAttribute(component, engine))
                {
                    differentEngineTypedComponents.Add(component);
                }
            }
            return differentEngineTypedComponents;
        }

        private bool HasDifferentEngineTypeAttribute(Component component, EnginePlatform engine)
        {
            var engineAnnotations = component.GetType().GetCustomAttributes(typeof(EngineTypeAttribute), true);
            for (int i = 0; i < engineAnnotations.Length; i++)
            {
                var annotation = engineAnnotations[i];
                if (((EngineTypeAttribute) annotation).EnginePlatform != engine)
                {
                    return true;
                }
            }
            return false;
        }

        private void DestroyComponent(Component component)
        {
            Logger.DebugFormat("GameObject {0}: Destroying component {1}",
                               gameObject.name,
                               component.GetType().FullName);
            DestroyImmediate(component, true);
        }

        private List<Component> GetComponentInstancesToRemove()
        {
            var componentInstancesToRemove = new List<Component>();
            foreach (Type componentToStrip in GetComponentsToRemove(engine))
            {
                componentInstancesToRemove.AddRange(gameObject.GetComponentsInChildren(componentToStrip, true));
            }
            return componentInstancesToRemove;
        }
    }
}