using Improbable.Corelib.Interpolation;
using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Core.GameLogic.Visualizers
{
    [EngineType(EnginePlatform.Client)]
    public class InterpolatingPositionVisualizer : MonoBehaviour
    {
        private readonly PositionInterpolator positionInterpolator = new PositionInterpolator();
        [Require] protected PositionReader Position;

        private Transform cachedTransform;

        private Coordinates StatePosition
        {
            get { return Position.Value; }
        }

        protected void OnEnable()
        {
            cachedTransform = transform;
            ResetInterpolator();
            RegisterOnStateUpdates();
            InitializePosition();
        }

        protected void Update()
        {
            var deltaTimeToAdvance = Time.deltaTime;
            SetPosition(positionInterpolator.GetInterpolatedValue(deltaTimeToAdvance));
        }

        private void ResetInterpolator()
        {
            var initialValueAbsoluteTime = Position.Timestamp;
            positionInterpolator.Reset(StatePosition, initialValueAbsoluteTime);
        }

        private void InitializePosition()
        {
            SetPosition(StatePosition);
        }

        private void RegisterOnStateUpdates()
        {
            Position.ValueUpdated += UpdatePosition;
        }

        private void UpdatePosition(Coordinates position)
        {
            positionInterpolator.AddValue(Position.Value, Position.Timestamp);
        }

        private void SetPosition(Coordinates unityVector)
        {
            cachedTransform.position = unityVector.ToUnityVector();
        }
    }
}