using Improbable.Corelib.Util;
using Improbable.Corelibrary.Physical;
using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Visualizers.Transform
{
    public class TeleportTransformVisualizer : MonoBehaviour
    {
        [Require] protected TeleportAckWriter Acknowledge;
        [Require] protected TeleportRequestReader TeleportRequest;
        [Require] protected TransformWriter Transform;

        private int mostRecentlyExecuted = -1;

        protected void OnEnable()
        {
            mostRecentlyExecuted = Acknowledge.LastExecutedRequestId;

            TeleportRequest.PropertyUpdated += OnNewTeleportRequest;
        }

        private void OnNewTeleportRequest()
        {
            if (TeleportRequest.RequestId != mostRecentlyExecuted)
            {
                mostRecentlyExecuted = TeleportRequest.RequestId;

                var cachedTransform = GetComponent<UnityEngine.Transform>();
                UpdateTransform(cachedTransform, TeleportRequest.Position, QuaternionUtils.FromEuler(TeleportRequest.Euler.ToUnityVector()));

                Acknowledge
                    .Update
                    .LastExecutedRequestId(mostRecentlyExecuted)
                    .FinishAndSend();
            }
        }

        private void UpdateTransform(UnityEngine.Transform cachedTransform, Coordinates position, Math.Quaternion rotation)
        {
            cachedTransform.position = position.ToUnityVector();
            cachedTransform.rotation = rotation.ToUnityQuaternion();

            Transform.Update
                    .Position(position)
                    .Rotation(rotation)
                    .FinishAndSend();
        }
    }
}