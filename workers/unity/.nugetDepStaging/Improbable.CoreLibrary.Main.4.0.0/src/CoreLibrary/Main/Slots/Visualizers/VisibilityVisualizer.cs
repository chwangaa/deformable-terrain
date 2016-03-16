using Improbable.Corelib.Visual;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Improbable.Corelib.Slots.Visualizers
{
    [EngineType(EnginePlatform.Client)]
    public class VisibilityVisualizer : MonoBehaviour
    {
        [Require] protected VisualityReader Visibility;

        protected void OnEnable()
        {
            Visibility.IsVisualUpdated += IsVisualizedUpdated;
        }

        private void IsVisualizedUpdated(bool visible)
        {
            if (visible)
            {
                MakeVisible();
            }
            else
            {
                MakeInvisible();
            }
        }

        protected virtual void MakeVisible()
        {
            Color c = GetComponent<Renderer>().material.color;
            c.a = 1f;
            GetComponent<Renderer>().material.color = c;
        }

        protected virtual void MakeInvisible()
        {
            Color c = GetComponent<Renderer>().material.color;
            c.a = 0.5f;
            GetComponent<Renderer>().material.color = c;
        }
    }
}