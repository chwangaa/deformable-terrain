using Improbable.Corelib.Slots;

namespace Improbable.Corelib.Physical
{
    public interface IHierarchyNodeRegistrable
    {
        void RegisterHierarchyNode(HierarchyNodeReader hierarchyNodeReader);
    }
}