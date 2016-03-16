using Assets.Improbable.Core.Physical;
using Improbable.Corelib.Physical.Visualizers;
using Improbable.Corelib.Slots;
using UnityEngine;

namespace Improbable.Corelib.Physical
{
    public abstract class PhysicalBase<T> : MonoBehaviour, IHierarchyNodeRegistrable
    {
        [Tooltip("When true, we will try to find a IRigidbodyVisualizer whose Rigidbody's sleeping state will be used to omit threshold checks when possible.")]
        public bool ExpectsIRigidbodyVisualizer;

        [Header("State Update Rate Configuration")]
        [Tooltip("The minimum number of seconds between sending each state update.")]
        public float NetworkUpdatePeriodThreshold;

        [Tooltip("The minimum number of seconds between sending each state update when slotted (i.e., when this entities HierarchyNode exists and has a parent).")]
        public float SlottedNetworkUpdatePeriodThreshold;

        private ThresholdBasedUpdateNotifier<T> thresholdBasedUpdateNotifier;
        private HierarchyNodeReader hierarchyNode;
        private bool hadParent;

        private IRigidbodyVisualizer cachedRigidbodyVisualizer;
        protected Rigidbody Rigidbody
        {
            get
            {
                if (cachedRigidbodyVisualizer == null)
                {
                    cachedRigidbodyVisualizer = GetComponent<IRigidbodyVisualizer>();
                }
                return cachedRigidbodyVisualizer.Rigidbody;
            }
        }

        protected Transform CachedTransform { get; private set; }

        protected void Awake()
        {
            CachedTransform = transform;
        }

        protected virtual void OnEnable()
        {
            SetupThresholdBasedUpdateNotifier();
        }

        protected void FixedUpdate()
        {
            if (RigidbodyIsSleeping())
            {
                return;
            }
            
            thresholdBasedUpdateNotifier.RegisterNewValue(Time.time, GetLatestValue());
        }

        private bool RigidbodyIsSleeping()
        {
            return ExpectsIRigidbodyVisualizer && Rigidbody != null && Rigidbody.IsSleeping();
        }

        public void RegisterHierarchyNode(HierarchyNodeReader hierarchyNodeReader)
        {
            hierarchyNode = hierarchyNodeReader;
            hierarchyNode.ParentUpdated += OnParentUpdated;
        }

        protected abstract T GetLatestValue();

        protected abstract bool IsPastThreshold(T lastValue, T newValue);

        protected abstract void OnShouldUpdate(float timeDelta, T newValue);

        private void OnParentUpdated(EntityId? parentEntityId)
        {
            if (hadParent != HasParent())
            {
                hadParent = HasParent();
                SetupThresholdBasedUpdateNotifier();
            }
        }

        private void SetupThresholdBasedUpdateNotifier()
        {
            thresholdBasedUpdateNotifier = new ThresholdBasedUpdateNotifier<T>(GetNetworkUpdatePeriodThreshold(), IsPastThreshold, GetLatestValue());
            thresholdBasedUpdateNotifier.ShouldUpdate += OnShouldUpdate;
        }

        private float GetNetworkUpdatePeriodThreshold()
        {
            return HasParent() ? SlottedNetworkUpdatePeriodThreshold : NetworkUpdatePeriodThreshold;
        }

        private bool HasParent()
        {
            return hierarchyNode != null && hierarchyNode.Parent.HasValue;
        }
    }
}