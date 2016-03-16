using System.Collections.Generic;

namespace Improbable.Unity.Visualizer
{
    public interface IVisualizerExtractor
    {
        IList<object> ExtractVisualizers();
    }
}