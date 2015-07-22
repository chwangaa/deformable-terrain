using Improbable.Debug;
using Improbable.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Assets.Gamelogic.Visualizers
{
    class ColorVisualizer : MonoBehaviour
    {
        [Require] public ColorReader Color;

        public Renderer[] Renderers;

        private void OnEnable()
        {
            Color.ValueUpdated += HandleValueUpdated;
        }

        private void OnDisable()
        {
            Color.ValueUpdated -= HandleValueUpdated;
        }

        private void HandleValueUpdated(Vector3f color)
        {
            if (Renderers != null)
            {
                var unityColor = new Color(color.x, color.y, color.z);
                foreach (var renderer in Renderers)
                {
                    renderer.material.color = unityColor;
                }
            }
        }
    }
}