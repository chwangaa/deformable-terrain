using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Unity.Visualizer;
using log4net;
using UnityEngine;

namespace Improbable.Unity.Assets
{
    class BehaviourEngineCompatibilityCache
    {
        private readonly HashSet<Type> compatibleBehaviours;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BehaviourEngineCompatibilityCache));

        public BehaviourEngineCompatibilityCache(EnginePlatform platform)
        {
            compatibleBehaviours = new HashSet<Type>();   
            var allTypes = AppDomain
                            .CurrentDomain
                            .GetAssemblies()
                            .SelectMany(assembly => assembly.GetTypes());
            foreach (var type in allTypes)
            {
                var attributes = type.GetCustomAttributes(typeof(EngineTypeAttribute), false);
                if (typeof(MonoBehaviour).IsAssignableFrom(type))
                {
                    if (IsPlatformCompatible(attributes, platform))
                    {
                        compatibleBehaviours.Add(type);
                    }
                }
                else if (attributes.Length > 0)
                {
                    Logger.WarnFormat("{0} uses EngineTypeAttribute but is not MonoBehavoiur. The attribute will be ignored.", type.FullName);
                }
            }
        }

        public bool IsCompatibleBehaviour(Type behaviourType)
        {
            return compatibleBehaviours.Contains(behaviourType);
        }

        private static bool IsPlatformCompatible(object[] engineTypes, EnginePlatform platform)
        {
            EnginePlatform enginePlatformMask = 0;
            for (int i = 0; i < engineTypes.Length; i++)
            {
                enginePlatformMask |= ((EngineTypeAttribute)engineTypes[i]).EnginePlatform;
            }
            return engineTypes.Length == 0 || (enginePlatformMask & platform) != 0;
        }
    }
}
