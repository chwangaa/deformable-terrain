using UnityEngine;

namespace Assets.Gamelogic.Visualizers
{
    class CameraVisualizer : MonoBehaviour
    {
        public Camera OurCamera;
        public Transform CameraRoot;

        private float RotateSpeed = 60.0f;

        void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                CameraRoot.transform.Rotate(-Vector3.up * Time.deltaTime * RotateSpeed, Space.World);
            } else if (Input.GetKey(KeyCode.E))
            {
                CameraRoot.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed, Space.World);
            }
        }
    }
}
