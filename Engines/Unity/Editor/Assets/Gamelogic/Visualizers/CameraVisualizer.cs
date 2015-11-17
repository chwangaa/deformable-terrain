using Improbable.Player;
using Improbable.Unity.Input.Sources;
using Improbable.Unity.Visualizer;
using IoC;
using UnityEngine;

namespace Assets.Gamelogic.Visualizers
{
    public class CameraVisualizer : MonoBehaviour
    {
        [Inject] public IInputSource InputSource { protected get; set; }
        
        [Require] protected LocalPlayerCheckStateWriter LocalPlayerCheck;

        public Camera OurCamera;
        public Transform CameraRoot;
        public float RotationSpeed;

        public void Update()
        {
            if (InputSource.GetKey(KeyCode.Q))
            {
                CameraRoot.transform.Rotate(-Vector3.up * Time.deltaTime * RotationSpeed, Space.World);
            }
            else if (InputSource.GetKey(KeyCode.E))
            {
                CameraRoot.transform.Rotate(Vector3.up * Time.deltaTime * RotationSpeed, Space.World);
            }
        }
    }
}
