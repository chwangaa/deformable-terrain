using Improbable.Entity.Physical;
using UnityEngine;

namespace Improbable.Corelib.Physical.Visualizers
{
    public interface IRigidbodyVisualizer
    {
        RigidbodyDataReader RigidbodyData { get; }
        Rigidbody Rigidbody { get; }
    }
}
