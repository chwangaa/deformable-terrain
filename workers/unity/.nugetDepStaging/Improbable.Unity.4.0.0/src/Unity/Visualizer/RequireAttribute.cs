using System;

namespace Improbable.Unity.Visualizer
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RequireAttribute : Attribute {}
}
