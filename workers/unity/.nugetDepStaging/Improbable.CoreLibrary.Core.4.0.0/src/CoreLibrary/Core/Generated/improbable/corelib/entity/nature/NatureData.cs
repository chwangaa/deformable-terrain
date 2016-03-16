// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.entity.nature.NatureDataData in improbable/corelib/entity/nature/nature_data.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Corelib.Entity.Nature
{
[ReaderInterface]
[CanonicalName("improbable.corelib.entity.nature.NatureData")]
public interface NatureDataReader : IEntityStateReader
{
    global::Improbable.Util.Collections.IReadOnlyList<string> Natures { get; }

    event System.Action<global::Improbable.Util.Collections.IReadOnlyList<string>> NaturesUpdated;
}

public interface INatureDataUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    INatureDataUpdater Natures(global::System.Collections.Generic.IList<string> newValue);
}

[WriterInterface]
[CanonicalName("improbable.corelib.entity.nature.NatureData")]
public interface NatureDataWriter : NatureDataReader, IUpdateable<INatureDataUpdater> { }

public class NatureData : global::Improbable.Entity.State.StateBase<Improbable.Corelib.Entity.Nature.NatureDataData, Schema.Improbable.Corelib.Entity.Nature.NatureDataData>, NatureDataWriter, INatureDataUpdater
{
    public NatureData(global::Improbable.EntityId entityId, Improbable.Corelib.Entity.Nature.NatureDataData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Corelib.Entity.Nature.NatureDataDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(NatureData));
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

    public global::Improbable.Util.Collections.IReadOnlyList<string> Natures { get { return Data.Natures; } }

    private readonly global::System.Collections.Generic.List<System.Action<global::Improbable.Util.Collections.IReadOnlyList<string>>> updatedCallbacksNatures =
        new global::System.Collections.Generic.List<System.Action<global::Improbable.Util.Collections.IReadOnlyList<string>>>();
    public event System.Action<global::Improbable.Util.Collections.IReadOnlyList<string>> NaturesUpdated
    {
        add
        {
            updatedCallbacksNatures.Add(value);
            value(Data.Natures);
        }
        remove { updatedCallbacksNatures.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, updatedCallbacksNatures);
    }

    public INatureDataUpdater Update
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
                Updater = new NatureDataUpdate(EntityId, new bool[1], new Schema.Improbable.Corelib.Entity.Nature.NatureDataData());
            }
            return this;
        }
    }

    INatureDataUpdater INatureDataUpdater.Natures(global::System.Collections.Generic.IList<string> newValue)
    {
        if ((Updater.Proto.Natures.Count > 0 || Updater.StatesToClear != null && Updater.StatesToClear[0]) || !global::Improbable.Util.Collections.CollectionUtil.ListsEqual(Natures, newValue))
        {
            global::Improbable.Tools.ToProto<string, string>(newValue, Updater.Proto.Natures);
            Updater.StatesToClear[0] = newValue.Count == 0;
        }
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Corelib.Entity.Nature.NatureDataData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        bool updatedNatures = (update.Natures.Count > 0 || statesToClear != null && statesToClear[0]);
        anythingUpdated |= updatedNatures;
        if (updatedNatures) TriggerCallbacks(updatedCallbacksNatures, Data.Natures);

        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Corelib.Entity.Nature.NatureDataData stateUpdate)
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

public class NatureDataUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Corelib.Entity.Nature.NatureDataData, Schema.Improbable.Corelib.Entity.Nature.NatureDataData>
{
    public const int STATE_UPDATE_FIELD_ID = 190131;
    public NatureDataUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Corelib.Entity.Nature.NatureDataData proto)
        : base(entityId, statesToClear, Improbable.Corelib.Entity.Nature.NatureDataDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new NatureData(entityId, GetData(), stateSender);
    }

    public static NatureDataUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Corelib.Entity.Nature.NatureDataData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        bool[] statesToClear = new bool[1];
        for (int i = 0; i < proto.FieldsToClear.Count; i++)
        {
            statesToClear[FieldIdToIndex(proto.FieldsToClear[i])] = true;
        }
        return new NatureDataUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), statesToClear, protoState);
    }

    private static uint FieldIdToIndex(uint id)
    {
        switch (id)
        {
            case 1: //natures
                return 0;
            default:
                throw new ArgumentException(string.Format("Unexpected error: {0} is not a valid clearable field number for state Improbable.Corelib.Entity.Nature.NatureData.", id));
        }
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = { 1, };
}
}
