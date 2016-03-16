using System;
using System.Collections.Generic;
using Improbable.Unity.Core;

namespace Improbable.Unity
{
    public static class EngineTypeUtils
    {
        private static readonly Dictionary<string, EnginePlatform> ENGINE_NAME_TO_PLATFORM_MAP;
        private static readonly Dictionary<EnginePlatform, string> ENGINE_PLATFORM_TO_NAME_MAP;
        public const string CLIENT_ENGINE_TYPE = "UnityClient";
        public const string FSIM_ENGINE_TYPE = "UnityFSim";

        static EngineTypeUtils()
        {
            ENGINE_NAME_TO_PLATFORM_MAP = BuildEnginePlatformToEnumMap();
            ENGINE_PLATFORM_TO_NAME_MAP = CreateReverseEnginePlatformMap();
        }

        public static EnginePlatform CurrentEnginePlatform
        {
            get
            {
                EnginePlatform enginePlatform;
                if (ENGINE_NAME_TO_PLATFORM_MAP.TryGetValue(EngineConfiguration.Instance.EngineType, out enginePlatform))
                {
                    return enginePlatform;
                }
                throw new NotSupportedException("The engine type '" + EngineConfiguration.Instance.EngineType + "' is not known. Please check the start-up configuration.");
            }
        }

        public static String ToEngineName(EnginePlatform enginePlatform)
        {
            return ENGINE_PLATFORM_TO_NAME_MAP[enginePlatform];
        }

        private static Dictionary<string, EnginePlatform> BuildEnginePlatformToEnumMap()
        {
            return new Dictionary<string, EnginePlatform>
            {
                { CLIENT_ENGINE_TYPE, EnginePlatform.Client },
                { FSIM_ENGINE_TYPE, EnginePlatform.FSim }
            };
        }

        private static Dictionary<EnginePlatform, string> CreateReverseEnginePlatformMap()
        {
            var enginePlatformToStringMap = new Dictionary<EnginePlatform, string>();
            foreach (var enginePlatform in ENGINE_NAME_TO_PLATFORM_MAP)
            {
                enginePlatformToStringMap.Add(enginePlatform.Value, enginePlatform.Key);
            }
            return enginePlatformToStringMap;
        }
    }
}