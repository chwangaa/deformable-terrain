using Improbable.Corelib.Physical.Visualizers;
using Improbable.Entity.Physical;
using UnityEngine;
using WorldScene.Visualizers;

namespace Improbable.Pinnacle.Visualizers
{
    public class PlayerRigidbodyVisualizer : MonoBehaviour, IVisualizer, IRigidbodyVisualizer
    {
        [Data] private IRigidbodyData rigidbodyData;
        [Data] private IRigidbodyEngineData rigidbodyEngineData;
        private RigidbodyParametersBinder rigidbodyParametersBinder;

        public IRigidbodyData RigidbodyData
        {
            get { return rigidbodyData; }
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
            rigidbodyParametersBinder = new RigidbodyParametersBinder(this);
            RigidbodyParametersBinder.AddRigidbody(gameObject);
            rigidbodyParametersBinder.ListenToRigidbodyParameters();
        }

        public void OnDisable()
        {
            rigidbodyParametersBinder.StopListeningToRigidbodyParameters();
            Destroy(GetComponent<Rigidbody>());
        }
    }
}