namespace Improbable.Unity.Entity
{
    public interface IVisualizerActivator
    {
        void Activate(object visualizer);
        void Deactivate(object visualizer);
    }
}