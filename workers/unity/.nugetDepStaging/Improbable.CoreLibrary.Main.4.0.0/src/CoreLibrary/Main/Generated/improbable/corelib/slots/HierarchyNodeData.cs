// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.slots.HierarchyNodeData in improbable/corelib/slots/hierarchy_node.proto.

namespace Improbable.Corelib.Slots
{
public struct HierarchyNodeData : global::System.IEquatable<HierarchyNodeData>
{
    public readonly Improbable.EntityId? Parent;
    public readonly global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> Children;

    public HierarchyNodeData (Improbable.EntityId? parent,
        global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> children)
    {
        Parent = parent;
        Children = children;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is HierarchyNodeData))
            return false;
        return Equals((HierarchyNodeData) obj);
    }

    public static bool operator ==(HierarchyNodeData obj1, HierarchyNodeData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(HierarchyNodeData obj1, HierarchyNodeData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(HierarchyNodeData obj)
    {
        return true
            && global::Improbable.Util.Collections.CollectionUtil.OptionsEqual(Parent, obj.Parent)
            && global::Improbable.Util.Collections.CollectionUtil.ListsEqual(Children, obj.Children);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + (Parent != null ? Parent.GetHashCode() : 0);
        res = (res * 977) + (Children != null ? Children.GetHashCode() : 0);
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class HierarchyNodeDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Corelib.Slots.HierarchyNodeData, Schema.Improbable.Corelib.Slots.HierarchyNodeData>
{
    static readonly HierarchyNodeDataHelper _instance = new HierarchyNodeDataHelper();
    public static HierarchyNodeDataHelper Instance { get { return _instance; } }
    private HierarchyNodeDataHelper() {}

    public Schema.Improbable.Corelib.Slots.HierarchyNodeData ToProto(Improbable.Corelib.Slots.HierarchyNodeData data)
    {
        var proto = new Schema.Improbable.Corelib.Slots.HierarchyNodeData();
        if (data.Parent != null) proto.Parent = Improbable.EntityIdHelper.Instance.ToProto(data.Parent.Value);
        global::Improbable.Tools.ToProto<Improbable.EntityId, long>(data.Children, proto.Children, Improbable.EntityIdHelper.Instance);
        return proto;
    }

    //Shallow merge method
    public Improbable.Corelib.Slots.HierarchyNodeData MergeFromProto(Schema.Improbable.Corelib.Slots.HierarchyNodeData proto, bool[] statesToClear, Improbable.Corelib.Slots.HierarchyNodeData data)
    {
        return new Improbable.Corelib.Slots.HierarchyNodeData(
            (proto.ParentSpecified || statesToClear != null && statesToClear[0]) ? (!proto.ParentSpecified ? (Improbable.EntityId?) null : Improbable.EntityIdHelper.Instance.FromProto(proto.Parent)) : data.Parent,
            (proto.Children.Count > 0 || statesToClear != null && statesToClear[1]) ? global::Improbable.Tools.FromProto<Improbable.EntityId, long>(proto.Children, Improbable.EntityIdHelper.Instance) : data.Children
        );
    }

    public Improbable.Corelib.Slots.HierarchyNodeData FromProto(Schema.Improbable.Corelib.Slots.HierarchyNodeData proto)
    {
        return new Improbable.Corelib.Slots.HierarchyNodeData(
            !proto.ParentSpecified ? (Improbable.EntityId?) null : Improbable.EntityIdHelper.Instance.FromProto(proto.Parent),
            global::Improbable.Tools.FromProto<Improbable.EntityId, long>(proto.Children, Improbable.EntityIdHelper.Instance)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Corelib.Slots.HierarchyNodeData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Corelib.Slots.HierarchyNodeData protoTo, bool[] statesToClearTo)
    {
        if ((protoFrom.ParentSpecified || statesToClearFrom != null && statesToClearFrom[0]))
        {
            statesToClearTo[0] = statesToClearFrom[0];
            protoTo.Parent = protoFrom.Parent;
            protoTo.ParentSpecified = protoFrom.ParentSpecified;
        }
        if ((protoFrom.Children.Count > 0 || statesToClearFrom != null && statesToClearFrom[1]))
        {
            statesToClearTo[1] = statesToClearFrom[1];
            protoTo.Children.Clear();
            protoTo.Children.AddRange(protoFrom.Children);
        }
    }
}
}