using Improbable.Entity.Physical;
using Improbable.Math;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Core.GameLogic.Visualizers
{
    public class TeleportVisualizer : MonoBehaviour
    {
        [Require] protected TeleportAckWriter Acknowledge;
        [Require] protected TeleportRequestReader TeleportRequest;
        [Require] protected PositionWriter Position;
        [Require] protected RotationWriter Rotation;

        private int MostRecentlyExecuted = -1;

        protected void OnEnable()
        {
            MostRecentlyExecuted = Acknowledge.LastExecutedRequestId;

            TeleportRequest.RequestIdUpdated += OnNewTeleportRequest;
        }

        private void OnNewTeleportRequest(int requestId)
        {
            if (requestId != MostRecentlyExecuted)
            {
                MostRecentlyExecuted = requestId;

                var cachedTransform = GetComponent<Transform>();
                UpdatePosition(cachedTransform, TeleportRequest.Position);
                UpdateRotation(cachedTransform, TeleportRequest.Euler);

                Acknowledge
                    .Update
                    .LastExecutedRequestId(MostRecentlyExecuted)
                    .FinishAndSend();
            }
        }

        private void UpdatePosition(Transform cachedTransform, Coordinates position)
        {
            cachedTransform.position = position.ToUnityVector();
            Position.Update
                    .Value(position)
                    .FinishAndSend();
        }

        private void UpdateRotation(Transform cachedTransform, Vector3d rotation)
        {
            cachedTransform.rotation = Quaternion.Euler(rotation.ToUnityVector());
            Rotation
                .Update
                .Euler(new Vector3d(rotation.X, rotation.Y, rotation.Z))
                .FinishAndSend();
        }
    }
}