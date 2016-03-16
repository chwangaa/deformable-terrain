using System;
using Improbable.Unity.Common.Core.Math;
using UnityEngine;

namespace Improbable.Corelib.Physical.Visualizers
{
    public class RigidbodyVelocitiesBinder
    {
        private readonly IRigidbodyVelocitiesVisualizer rigidbodyVisualizer;
        private bool initialVelocitiesJustApplied;

        public RigidbodyVelocitiesBinder(IRigidbodyVelocitiesVisualizer rigidbodyVisualizer)
        {
            this.rigidbodyVisualizer = rigidbodyVisualizer;
        }

        public bool InitialVelocitiesApplied { get; private set; }

        public void StartBinding()
        {
            rigidbodyVisualizer.RigidbodyEngineData.VelocityUpdated += UpdateVelocity;
            rigidbodyVisualizer.RigidbodyEngineData.AngularVelocityUpdated += UpdateAngularVelocity;
        }

        public void StopBinding()
        {
            rigidbodyVisualizer.RigidbodyEngineData.VelocityUpdated -= UpdateVelocity;
            rigidbodyVisualizer.RigidbodyEngineData.AngularVelocityUpdated -= UpdateAngularVelocity;
        }

        public void FixedUpdate()
        {
            InitializeVelocities();
        }

        private void InitializeVelocities()
        {
            if (!InitialVelocitiesApplied)
            {
                // NOTE: initialVelocitiesJustApplied is used to add a one-frame delay, so that the physics can actually apply the force.
                if (initialVelocitiesJustApplied)
                {
                    InitialVelocitiesApplied = true;
                }
                else if (rigidbodyVisualizer.RigidbodyEngineData.IsAuthoritativeHere)
                {
                    try
                    {
                        AddForceToReachVelocity(rigidbodyVisualizer.RigidbodyEngineData.Velocity);
                        SetAngularVelocity(rigidbodyVisualizer.RigidbodyEngineData.AngularVelocity);
                        initialVelocitiesJustApplied = true;
                    }
                    catch (Exception)
                    {
                        // NOTE: This is a workaround for an exception that occurs when accessing Rigidbody.velocity.
                        // It seems to have some inconsistent internal state (a proper internal unity exception in
                        // unmanaged code). We believe that this must be connected with
                        // us using adding rigidbodies on just-activated game objects.
                    }
                }
            }
        }

        private void UpdateVelocity(Improbable.Math.Vector3d velocity)
        {
            if (!rigidbodyVisualizer.Rigidbody.isKinematic)
            {
                AddForceToReachVelocity(velocity);
            }
        }

        private void UpdateAngularVelocity(Improbable.Math.Vector3d angularVelocity)
        {
            if (!rigidbodyVisualizer.Rigidbody.isKinematic)
            {
                SetAngularVelocity(angularVelocity);
            }
        }

        private void SetAngularVelocity(Improbable.Math.Vector3d angularVelocity)
        {
            rigidbodyVisualizer.Rigidbody.AddTorque((angularVelocity.ToUnityVector() - rigidbodyVisualizer.Rigidbody.angularVelocity), ForceMode.VelocityChange);
        }

        private void AddForceToReachVelocity(Improbable.Math.Vector3d velocity)
        {
            rigidbodyVisualizer.Rigidbody.AddForce((velocity.ToUnityVector() - rigidbodyVisualizer.Rigidbody.velocity), ForceMode.VelocityChange);
        }
    }
}