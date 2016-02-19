using Improbable.Corelib.Physical.Visualizers;
using Improbable.Entity.Physical;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Pinnacle.Visualizers
{
    class PlayerRigidbodyVisualizer : MonoBehaviour, IRigidbodyVisualizer
    {
        [Require] public RigidbodyDataReader RigidbodyDetails;

        private RigidbodyParametersBinder RigidbodyParameters;

        public RigidbodyDataReader RigidbodyData
        {
            get { return RigidbodyDetails; }
        }

        public Rigidbody Rigidbody
        {
            get { return GetComponent<Rigidbody>(); }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public void OnEnable()
        {
            RigidbodyParameters = new RigidbodyParametersBinder(this);
            RigidbodyParametersBinder.AddRigidbody(gameObject);
            RigidbodyParameters.ListenToRigidbodyParameters();
        }

        public void OnDisable()
        {
            RigidbodyParameters.StopListeningToRigidbodyParameters();
            Destroy(GetComponent<Rigidbody>());
        }
    }
}