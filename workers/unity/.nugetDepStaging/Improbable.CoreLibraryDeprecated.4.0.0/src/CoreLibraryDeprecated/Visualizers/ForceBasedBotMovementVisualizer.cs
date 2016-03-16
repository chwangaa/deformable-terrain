using Improbable.Corelib.Physical;
using Improbable.Entity.Bot;
using Improbable.Unity;
using Improbable.Unity.Common.Core.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Core.GameLogic.Visualizers
{
    [EngineType(EnginePlatform.FSim)]
    public class ForceBasedBotMovementVisualizer : MonoBehaviour
    {
        [Require] protected BotTargetVelocityReader BotTargetVelocity;
        [Require] protected BotParametersReader BotParameters;
        [Require] protected GroundedReader Grounded;

        private const float KP = 1.0f;
        private const float KI = 0.01f;
        private const float KD = 0.0f;
        private Vector3 error;
        private Vector3 previousError;
        private Vector3 integral;
        private Vector3 derivative;

        protected void FixedUpdate()
        {
            if (Grounded.IsGrounded)
            {
                var currentVelocity = CachedRigidbody.velocity;
                var targetVelocity = BotTargetVelocity.TargetVelocity.ToUnityVector();
                AttemptTargetVelocity(currentVelocity, targetVelocity);
            }
        }

        private void AttemptTargetVelocity(Vector3 currentVelocity, Vector3 targetVelocity)
        {
            var constrainedTargetVelocity = ConstrainRotation(currentVelocity, targetVelocity);
            var forceToApply = GetPIDValue(currentVelocity, constrainedTargetVelocity) * BotParameters.Power;
            CachedRigidbody.AddForce(forceToApply, ForceMode.Acceleration);
        }

        private Vector3 ConstrainRotation(Vector3 currentVelocity, Vector3 targetVelocity)
        {
            var maxRadiansDelta = BotParameters.TurnRate * Mathf.Deg2Rad * Time.deltaTime;
            return Vector3.RotateTowards(currentVelocity, targetVelocity, maxRadiansDelta, float.MaxValue);
        }

        private Vector3 GetPIDValue(Vector3 currentValue, Vector3 targetValue)
        {
            var deltaTime = Time.deltaTime;
            error = targetValue - currentValue;
            integral = integral + (error * deltaTime);
            derivative = (error - previousError) / deltaTime;
            previousError = error;
            return (KP * error) + (KI * integral) + (KD * derivative);
        }

        private Rigidbody cachedRigidBody;

        private Rigidbody CachedRigidbody
        {
            get
            {
                if (cachedRigidBody == null)
                {
                    cachedRigidBody = GetComponent<Rigidbody>();
                }
                return cachedRigidBody;
            }
        }
    }
}