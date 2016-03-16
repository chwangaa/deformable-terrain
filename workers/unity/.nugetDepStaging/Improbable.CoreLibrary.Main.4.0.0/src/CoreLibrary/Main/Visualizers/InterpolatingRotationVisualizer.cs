using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;
using RotationInterpolator = Improbable.Corelib.Interpolation.RotationInterpolator;

namespace Improbable.Core.GameLogic.Visualizers
{
    [EngineType(EnginePlatform.Client)]
    public class InterpolatingRotationVisualizer : MonoBehaviour
    {
        [Require] protected RotationReader Rotation;

        private readonly RotationInterpolator rotationInterpolator = new RotationInterpolator();
        private Transform cachedTransform;

        private Quaternion StateRotation
        {
            get { return Quaternion.Euler(Rotation.Euler.ToUnityVector()); }
        }

        protected void OnEnable()
        {
            cachedTransform = transform;
            ResetInterpolators();
            RegisterOnStateUpdates();
            InitializePosition();
        }

        protected void Update()
        {
            var deltaTimeToAdvance = Time.deltaTime;
            SetRotation(rotationInterpolator.GetInterpolatedValue(deltaTimeToAdvance));
        }

        private void ResetInterpolators()
        {
            var initialValueAbsoluteTime = Rotation.Timestamp;
            rotationInterpolator.Reset(StateRotation, initialValueAbsoluteTime);
        }

        private void InitializePosition()
        {
            SetRotation(StateRotation);
        }

        private void RegisterOnStateUpdates()
        {
            Rotation.EulerUpdated += UpdateRotation;
        }

        private void UpdateRotation(Vector3d rotation)
        {
            rotationInterpolator.AddValue(StateRotation, Rotation.Timestamp);
        }

        private void SetRotation(Quaternion targetRotation)
        {
            cachedTransform.rotation = targetRotation;
        }
    }
}