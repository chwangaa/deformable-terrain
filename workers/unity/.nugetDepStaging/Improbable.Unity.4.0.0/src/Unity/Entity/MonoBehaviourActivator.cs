using UnityEngine;

namespace Improbable.Unity.Entity
{
    public class MonoBehaviourActivator : IVisualizerActivator
    {
        public void Activate(object visualizer)
        {
            ((MonoBehaviour) visualizer).enabled = true;
        }

        public void Deactivate(object visualizer)
        {
            ((MonoBehaviour) visualizer).enabled = false;
        }
    }
}
