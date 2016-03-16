using Improbable.Entity.Physical;
using UnityEngine;

namespace Improbable.Corelib.Physical.Visualizers
{
    public interface IRigidbodyVelocitiesVisualizer
    {
        RigidbodyEngineDataReader RigidbodyEngineData { get; }
        Rigidbody Rigidbody { get; }
    }
}