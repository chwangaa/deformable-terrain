using System;

namespace Improbable.Unity.Visualizer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class EngineTypeAttribute : Attribute
    {
        public EngineTypeAttribute(EnginePlatform enginePlatform)
        {
            EnginePlatform = enginePlatform;
        }

        public EnginePlatform EnginePlatform { get; private set; }
    }
}