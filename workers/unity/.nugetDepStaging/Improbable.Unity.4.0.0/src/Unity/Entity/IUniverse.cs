using Improbable.Unity.Entity;

namespace Improbable.Core.Entity
{
    /// <summary>
    /// Contains all of the entities currently in the game
    /// </summary>
    /// <remarks>This is really just a dictionary with a destroy method?</remarks>
    public interface IUniverse
    {
        // TODO: Does this make any sense on this interface? The game should not have access to these fields
        void AddEntity(EntityId entityId, IEntityObject entity);
        void Destroy(EntityId entityId);
        
        bool ContainsEntity(EntityId entityId);
        IEntityObject Get(EntityId entityId);
    }
}