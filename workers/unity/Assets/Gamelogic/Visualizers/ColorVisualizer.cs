using Improbable.ColorState;
using Improbable.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Visualizers
{
    public class ColorVisualizer : MonoBehaviour
    {
        [Require] public ColorStateReader Color;

        public Renderer[] Renderers;

        public void OnEnable()
        {
            Color.ValueUpdated += HandleValueUpdated;
        }

        public void OnDisable()
        {
            Color.ValueUpdated -= HandleValueUpdated;
        }

        private void HandleValueUpdated(Vector3f color)
        {
            if (Renderers != null)
            {
                var unityColor = new Color(color.X, color.Y, color.Z);
                foreach (var renderer in Renderers)
                {
                    renderer.material.color = unityColor;
                }
            }
        }
    }
}