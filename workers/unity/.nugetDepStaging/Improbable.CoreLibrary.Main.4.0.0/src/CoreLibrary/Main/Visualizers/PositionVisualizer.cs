using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Core.GameLogic.Visualizers
{
    [EngineType(EnginePlatform.FSim)]
    public class PositionVisualizer : MonoBehaviour
    {
        [Require] protected PositionReader Position;

        protected void OnEnable()
        {
            Position.ValueUpdated += UpdatePositionIfNotAuthoritative;
            UpdatePosition(Position.Value);
        }

        private void UpdatePositionIfNotAuthoritative(Coordinates position)
        {
            if (!Position.IsAuthoritativeHere)
            {
                UpdatePosition(position);
            }
        }

        private void UpdatePosition(Coordinates position)
        {
            transform.position = position.ToUnityVector();
        }
    }
}