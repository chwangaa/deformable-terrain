using System.Collections.Generic;
using Improbable.Entity.State;
using UnityEngine;

namespace Improbable.Unity.Entity
{
    public interface IEntityObject
    {
        EntityId EntityId { get; }
        GameObject UnderlyingGameObject { get; }
        IEntityStateContainer EntityStateContainer { get; }

        void Destroy();
        void DisableVisualizers(IList<object> visualizers);
        void TryEnableVisualizers(IList<object> visualizers);
    }
}
