using System;
using Improbable.Entity.Physical;
using Improbable.Unity.Common.Core.Math;
using UnityEngine;
using CollisionDetectionMode = Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode;
using InterpolationMode = Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode;

namespace Improbable.Corelib.Physical.Visualizers
{
    /// <summary>
    ///     This binds Rigidbody parameters to those updated by the RigisbodyData state from a
    ///     IRigidbodyVisualizer as long as we don't have authority on it.
    /// </summary>
    public class RigidbodyParametersBinder {

        private Vector3 rigidbodyCurrentForce = Vector3.zero;
        private Vector3 rigidbodyCurrentTorque = Vector3.zero;
        private readonly IRigidbodyVisualizer rigidbodyVisualizer;

        public RigidbodyParametersBinder(IRigidbodyVisualizer rigidbodyVisualizer)
        {
            this.rigidbodyVisualizer = rigidbodyVisualizer;
        }

        public void UpdateParameters()
        {
            UpdateMass(rigidbodyVisualizer.RigidbodyData.Mass);
            UpdateDrag(rigidbodyVisualizer.RigidbodyData.Drag);
            UpdateAngularDrag(rigidbodyVisualizer.RigidbodyData.AngularDrag);
            UpdatePositionConstraints(rigidbodyVisualizer.RigidbodyData.FreezePosition);
            UpdateRotationConstraints(rigidbodyVisualizer.RigidbodyData.FreezeRotation);
            UpdateUseGravity(rigidbodyVisualizer.RigidbodyData.UseGravity);
            UpdateIsKinematic(rigidbodyVisualizer.RigidbodyData.IsKinematic);
            UpdateInterpolationMode(rigidbodyVisualizer.RigidbodyData.Interpolation);
            UpdateCollisionDetectionMode(rigidbodyVisualizer.RigidbodyData.CollisionDetection);
            UpdateForce(rigidbodyVisualizer.RigidbodyData.Force);
            UpdateTorque(rigidbodyVisualizer.RigidbodyData.Torque);
        }

        public void ListenToRigidbodyParameters()
        {
            rigidbodyVisualizer.RigidbodyData.MassUpdated += UpdateMass;
            rigidbodyVisualizer.RigidbodyData.DragUpdated += UpdateDrag;
            rigidbodyVisualizer.RigidbodyData.AngularDragUpdated += UpdateAngularDrag;
            rigidbodyVisualizer.RigidbodyData.FreezePositionUpdated += UpdatePositionConstraints;
            rigidbodyVisualizer.RigidbodyData.FreezeRotationUpdated += UpdateRotationConstraints;
            rigidbodyVisualizer.RigidbodyData.UseGravityUpdated += UpdateUseGravity;
            rigidbodyVisualizer.RigidbodyData.IsKinematicUpdated += UpdateIsKinematic;
            rigidbodyVisualizer.RigidbodyData.InterpolationUpdated += UpdateInterpolationMode;
            rigidbodyVisualizer.RigidbodyData.CollisionDetectionUpdated += UpdateCollisionDetectionMode;
            rigidbodyVisualizer.RigidbodyData.Impulse += ApplyImpulse;
            rigidbodyVisualizer.RigidbodyData.SetVelocity += SetVelocity;
            rigidbodyVisualizer.RigidbodyData.ForceUpdated += UpdateForce;
            rigidbodyVisualizer.RigidbodyData.TorqueUpdated += UpdateTorque;
        }

        public void StopListeningToRigidbodyParameters()
        {
            rigidbodyVisualizer.RigidbodyData.MassUpdated -= UpdateMass;
            rigidbodyVisualizer.RigidbodyData.DragUpdated -= UpdateDrag;
            rigidbodyVisualizer.RigidbodyData.AngularDragUpdated -= UpdateAngularDrag;
            rigidbodyVisualizer.RigidbodyData.FreezePositionUpdated -= UpdatePositionConstraints;
            rigidbodyVisualizer.RigidbodyData.FreezeRotationUpdated -= UpdateRotationConstraints;
            rigidbodyVisualizer.RigidbodyData.UseGravityUpdated -= UpdateUseGravity;
            rigidbodyVisualizer.RigidbodyData.IsKinematicUpdated -= UpdateIsKinematic;
            rigidbodyVisualizer.RigidbodyData.InterpolationUpdated -= UpdateInterpolationMode;
            rigidbodyVisualizer.RigidbodyData.CollisionDetectionUpdated -= UpdateCollisionDetectionMode;
            rigidbodyVisualizer.RigidbodyData.Impulse -= ApplyImpulse;
            rigidbodyVisualizer.RigidbodyData.SetVelocity -= SetVelocity;
            rigidbodyVisualizer.RigidbodyData.ForceUpdated -= UpdateForce;
            rigidbodyVisualizer.RigidbodyData.TorqueUpdated -= UpdateTorque;
        }

        public void FixedUpdate()
        {
            if (!IsRigidbodyDataAuthoritativeHere())
            {
                ApplyForce();
                ApplyTorque();
            }
        }

        public static Rigidbody AddRigidbody(GameObject gameObject)
        {
            var rigidbody = gameObject.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
            return rigidbody;
        }

        protected bool IsRigidbodyDataAuthoritativeHere()
        {
            return rigidbodyVisualizer.RigidbodyData.IsAuthoritativeHere;
        }
        
        protected void UpdateMass(float mass)
        {
            rigidbodyVisualizer.Rigidbody.mass = mass;
        }

        protected void UpdateUseGravity(bool useGravity)
        {
            rigidbodyVisualizer.Rigidbody.useGravity = useGravity;
        }

        protected void UpdateDrag(float drag)
        {
            rigidbodyVisualizer.Rigidbody.drag = drag;
        }

        protected void UpdateAngularDrag(float drag)
        {
            rigidbodyVisualizer.Rigidbody.angularDrag = drag;
        }

        protected void UpdateIsKinematic(bool isKinematic)
        {
            rigidbodyVisualizer.Rigidbody.isKinematic = isKinematic;
        }

        protected void UpdatePositionConstraints(FreezeConstraints freezePositionConstraints)
        {
            var freezeRotationConstraints = rigidbodyVisualizer.RigidbodyData.FreezeRotation;
            SetConstraints(freezePositionConstraints, freezeRotationConstraints);
        }

        protected void UpdateRotationConstraints(FreezeConstraints freezeRotationConstraints)
        {
            var freezePositionConstraints = rigidbodyVisualizer.RigidbodyData.FreezePosition;
            SetConstraints(freezePositionConstraints, freezeRotationConstraints);
        }

        protected void SetConstraints(FreezeConstraints freezePositionConstraints, FreezeConstraints freezeRotationConstraints)
        {
            var positionXContraints = freezePositionConstraints.X ? RigidbodyConstraints.FreezePositionX : RigidbodyConstraints.None;
            var positionYContraints = freezePositionConstraints.Y ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None;
            var positionZContraints = freezePositionConstraints.Z ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.None;
            var rotationXContraints = freezeRotationConstraints.X ? RigidbodyConstraints.FreezeRotationX : RigidbodyConstraints.None;
            var rotationYContraints = freezeRotationConstraints.Y ? RigidbodyConstraints.FreezeRotationY : RigidbodyConstraints.None;
            var rotationZContraints = freezeRotationConstraints.Z ? RigidbodyConstraints.FreezeRotationZ : RigidbodyConstraints.None;

            rigidbodyVisualizer.Rigidbody.constraints = (positionXContraints | positionYContraints | positionZContraints | rotationXContraints | rotationYContraints | rotationZContraints);
        }

        protected void UpdateCollisionDetectionMode(CollisionDetectionMode collisionDetectionMode)
        {
            switch (collisionDetectionMode)
            {
                case CollisionDetectionMode.Discrete:
                    rigidbodyVisualizer.Rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Discrete;
                    break;
                case CollisionDetectionMode.Continuous:
                    rigidbodyVisualizer.Rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.Continuous;
                    break;
                case CollisionDetectionMode.Continuousdynamic:
                    rigidbodyVisualizer.Rigidbody.collisionDetectionMode = UnityEngine.CollisionDetectionMode.ContinuousDynamic;
                    break;
                default:
                    throw new ArgumentException("Collision detection mode: " + collisionDetectionMode + " is not a valid collision detection type.");
            }
        }

        protected void UpdateInterpolationMode(InterpolationMode interpolationMode)
        {
            switch (interpolationMode)
            {
                case InterpolationMode.None:
                    rigidbodyVisualizer.Rigidbody.interpolation = RigidbodyInterpolation.None;
                    break;
                case InterpolationMode.Interpolate:
                    rigidbodyVisualizer.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                    break;
                case InterpolationMode.Extrapolate:
                    rigidbodyVisualizer.Rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
                    break;
                default:
                    throw new ArgumentException("Interpolation mode: " + interpolationMode + " is not a valid interpolation type.");
            }
        }

        private void ApplyTorque()
        {
            if (rigidbodyCurrentTorque != Vector3.zero)
            {
                rigidbodyVisualizer.Rigidbody.AddTorque(rigidbodyCurrentTorque, ForceMode.Force);
            }
        }

        private void ApplyForce()
        {
            if (rigidbodyCurrentForce != Vector3.zero)
            {
                rigidbodyVisualizer.Rigidbody.AddForce(rigidbodyCurrentForce, ForceMode.Force);
            }
        }

        private void ApplyImpulse(Impulse impulse)
        {
            rigidbodyVisualizer.Rigidbody.AddForce(impulse.Value.ToUnityVector(), ForceMode.Impulse);
        }

        private void SetVelocity(SetVelocity velocity)
        {
            rigidbodyVisualizer.Rigidbody.velocity = velocity.Value.ToUnityVector();
        }

        private void UpdateForce(Improbable.Math.Vector3d force)
        {
            rigidbodyCurrentForce = force.ToUnityVector();
        }

        private void UpdateTorque(Improbable.Math.Vector3d torque)
        {
            rigidbodyCurrentTorque = torque.ToUnityVector();
        }
    }
}