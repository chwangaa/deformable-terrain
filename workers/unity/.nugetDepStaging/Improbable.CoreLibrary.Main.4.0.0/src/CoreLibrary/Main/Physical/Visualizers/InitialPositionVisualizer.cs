using Improbable.Entity.Physical;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.CoreLib.Physical.Visualizers
{
    public class InitialPositionVisualizer : MonoBehaviour
    {
        [Require] protected PositionReader Position;

        protected void OnEnable()
        {
            transform.position = Position.Value.ToUnityVector();
        }
    }
}