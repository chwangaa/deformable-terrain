using Improbable.Entity.Behaviour;

namespace Improbable.Unity.Entity
{
    public interface IEntityObserverFactory
    {
        IEntityBehaviour MakeEntityBehaviour(string shortTraitName, IEntityObject entityObject);
    }
}