using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Improbable.Debug;
using UnityEngine;
using UnityEngine.UI;
using WorldScene.Visualizers;

namespace Assets.Gamelogic.Visualizers
{

    class ColorVisualizer : MonoBehaviour, IVisualizer
    {

        [Data]
        private IColor Color;

        public Renderer[] Renderers;

        void OnEnable()
        {
            Color.ValueUpdated += HandleValueUpdated;
        }

        
        void OnDisable()
        {
            Color.ValueUpdated -= HandleValueUpdated;
        }

        private void HandleValueUpdated(Improbable.Math.Vector3f color)
        {
            if (Renderers != null)
            {
                var unityColor = new UnityEngine.Color(color.x, color.y, color.z);
                foreach (var renderer in Renderers)
                {
                    renderer.material.color = unityColor;
                }
            }
        }
    }
}