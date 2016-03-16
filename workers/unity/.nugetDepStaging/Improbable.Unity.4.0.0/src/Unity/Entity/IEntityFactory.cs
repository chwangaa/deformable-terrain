namespace Improbable.Unity.Entity
{
    public interface IEntityFactory
    {
        IEntityObject MakeEntity(EntityId entityId, string prefab, string context);
    }
}