using System;
using System.Collections.Generic;

namespace Improbable.Unity.Entity
{
    public interface IEntityVisualizers : IDisposable
    {
        void DisableVisualizers(IList<object> visualizersToDisable);
        void TryEnableVisualizers(IList<object> visualizersToEnable);
    }
}
