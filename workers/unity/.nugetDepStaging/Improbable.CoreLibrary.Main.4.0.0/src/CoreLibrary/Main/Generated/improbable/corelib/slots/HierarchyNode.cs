// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.slots.HierarchyNodeData in improbable/corelib/slots/hierarchy_node.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Corelib.Slots
{
[ReaderInterface]
[CanonicalName("improbable.corelib.slots.HierarchyNode")]
public interface HierarchyNodeReader : IEntityStateReader
{
    Improbable.EntityId? Parent { get; }
    global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> Children { get; }

    event System.Action<Improbable.EntityId?> ParentUpdated;
    event System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>> ChildrenUpdated;
}

public interface IHierarchyNodeUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    IHierarchyNodeUpdater Parent(Improbable.EntityId? newValue);
    IHierarchyNodeUpdater Children(global::System.Collections.Generic.IList<Improbable.EntityId> newValue);
}

[WriterInterface]
[CanonicalName("improbable.corelib.slots.HierarchyNode")]
public interface HierarchyNodeWriter : HierarchyNodeReader, IUpdateable<IHierarchyNodeUpdater> { }

public class HierarchyNode : global::Improbable.Entity.State.StateBase<Improbable.Corelib.Slots.HierarchyNodeData, Schema.Improbable.Corelib.Slots.HierarchyNodeData>, HierarchyNodeWriter, IHierarchyNodeUpdater
{
    public HierarchyNode(global::Improbable.EntityId entityId, Improbable.Corelib.Slots.HierarchyNodeData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Corelib.Slots.HierarchyNodeDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(HierarchyNode));
    private static bool ShouldLogFinishAndSendNoUpdate = true;
    private static bool ShouldLogUpdateNoFinishAndSend = true;

    protected override void LogFinishAndSendWithNoUpdate() {
        if (ShouldLogFinishAndSendNoUpdate)
        {
            ShouldLogFinishAndSendNoUpdate = false;
            LOGGER.ErrorFormat("Finish and send was called with no update in flight for entity {0}. " +
                               "This is probably due to having more StateUpdates in flight, which is an error. (Logged only once.)", EntityId);
        }
    }

    public Improbable.EntityId? Parent { get { return Data.Parent; } }
    public global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> Children { get { return Data.Children; } }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.EntityId?>> updatedCallbacksParent =
        new global::System.Collections.Generic.List<System.Action<Improbable.EntityId?>>();
    public event System.Action<Improbable.EntityId?> ParentUpdated
    {
        add
        {
            updatedCallbacksParent.Add(value);
            value(Data.Parent);
        }
        remove { updatedCallbacksParent.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>>> updatedCallbacksChildren =
        new global::System.Collections.Generic.List<System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>>>();
    public event System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>> ChildrenUpdated
    {
        add
        {
            updatedCallbacksChildren.Add(value);
            value(Data.Children);
        }
        remove { updatedCallbacksChildren.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, updatedCallbacksParent);
        UnsubscribeEventHandler(visualizer, updatedCallbacksChildren);
    }

    public IHierarchyNodeUpdater Update
    {
        get
        {
            if (Updating)
            {
                if (ShouldLogUpdateNoFinishAndSend)
                {
                    ShouldLogUpdateNoFinishAndSend = false;
                    LOGGER.ErrorFormat("Multiple state updates of entity {0} are in flight, which has undefined semantics. " +
                        "Each call to Update has to be followed by a FinishAndSend() before another call is made on the same state. (Logged only once.)", EntityId);
                }
            }
            else
            {
                Updating = true;
                Updater = new HierarchyNodeUpdate(EntityId, new bool[2], new Schema.Improbable.Corelib.Slots.HierarchyNodeData());
            }
            return this;
        }
    }

    IHierarchyNodeUpdater IHierarchyNodeUpdater.Parent(Improbable.EntityId? newValue)
    {
        if ((Updater.Proto.ParentSpecified || Updater.StatesToClear != null && Updater.StatesToClear[0]) || !global::Improbable.Util.Collections.CollectionUtil.OptionsEqual(Parent, newValue))
        {
            if (newValue != null)
                Updater.Proto.Parent = Improbable.EntityIdHelper.Instance.ToProto(newValue.Value);
            else
                Updater.Proto.ParentSpecified = false;
            Updater.StatesToClear[0] = newValue == null;
        }
        return this;
    }

    IHierarchyNodeUpdater IHierarchyNodeUpdater.Children(global::System.Collections.Generic.IList<Improbable.EntityId> newValue)
    {
        if ((Updater.Proto.Children.Count > 0 || Updater.StatesToClear != null && Updater.StatesToClear[1]) || !global::Improbable.Util.Collections.CollectionUtil.ListsEqual(Children, newValue))
        {
            global::Improbable.Tools.ToProto<Improbable.EntityId, long>(newValue, Updater.Proto.Children, Improbable.EntityIdHelper.Instance);
            Updater.StatesToClear[1] = newValue.Count == 0;
        }
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Corelib.Slots.HierarchyNodeData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        bool updatedParent = (update.ParentSpecified || statesToClear != null && statesToClear[0]);
        anythingUpdated |= updatedParent;
        if (updatedParent) TriggerCallbacks(updatedCallbacksParent, Data.Parent);

        bool updatedChildren = (update.Children.Count > 0 || statesToClear != null && statesToClear[1]);
        anythingUpdated |= updatedChildren;
        if (updatedChildren) TriggerCallbacks(updatedCallbacksChildren, Data.Children);

        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Corelib.Slots.HierarchyNodeData stateUpdate)
    {
        bool anythingUpdated = false;
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents()
    {
        bool anythingUpdated = false;
        return anythingUpdated;
    }
}

public class HierarchyNodeUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Corelib.Slots.HierarchyNodeData, Schema.Improbable.Corelib.Slots.HierarchyNodeData>
{
    public const int STATE_UPDATE_FIELD_ID = 190110;
    public HierarchyNodeUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Corelib.Slots.HierarchyNodeData proto)
        : base(entityId, statesToClear, Improbable.Corelib.Slots.HierarchyNodeDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new HierarchyNode(entityId, GetData(), stateSender);
    }

    public static HierarchyNodeUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Corelib.Slots.HierarchyNodeData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        bool[] statesToClear = new bool[2];
        for (int i = 0; i < proto.FieldsToClear.Count; i++)
        {
            statesToClear[FieldIdToIndex(proto.FieldsToClear[i])] = true;
        }
        return new HierarchyNodeUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), statesToClear, protoState);
    }

    private static uint FieldIdToIndex(uint id)
    {
        switch (id)
        {
            case 1: //parent
                return 0;
            case 2: //children
                return 1;
            default:
                throw new ArgumentException(string.Format("Unexpected error: {0} is not a valid clearable field number for state Improbable.Corelib.Slots.HierarchyNode.", id));
        }
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = { 1,  2, };
}
}