// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.worker.checkout.EntityLoadingControlData in improbable/corelib/worker/checkout/entity_loading_control.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Corelib.Worker.Checkout
{
[ReaderInterface]
[CanonicalName("improbable.corelib.worker.checkout.EntityLoadingControl")]
public interface EntityLoadingControlReader : IEntityStateReader
{
    Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates LoadedState { get; }
    int Tries { get; }
    int MaxTries { get; }
    int RetryWait { get; }
    bool TriggerCallbackOnTimeout { get; }
    global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> Entities { get; }

    event System.Action<Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates> LoadedStateUpdated;
    event System.Action<int> TriesUpdated;
    event System.Action<int> MaxTriesUpdated;
    event System.Action<int> RetryWaitUpdated;
    event System.Action<bool> TriggerCallbackOnTimeoutUpdated;
    event System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>> EntitiesUpdated;
}

public interface IEntityLoadingControlUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    IEntityLoadingControlUpdater LoadedState(Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates newValue);
    IEntityLoadingControlUpdater Tries(int newValue);
    IEntityLoadingControlUpdater MaxTries(int newValue);
    IEntityLoadingControlUpdater RetryWait(int newValue);
    IEntityLoadingControlUpdater TriggerCallbackOnTimeout(bool newValue);
    IEntityLoadingControlUpdater Entities(global::System.Collections.Generic.IList<Improbable.EntityId> newValue);
}

[WriterInterface]
[CanonicalName("improbable.corelib.worker.checkout.EntityLoadingControl")]
public interface EntityLoadingControlWriter : EntityLoadingControlReader, IUpdateable<IEntityLoadingControlUpdater> { }

public class EntityLoadingControl : global::Improbable.Entity.State.StateBase<Improbable.Corelib.Worker.Checkout.EntityLoadingControlData, Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData>, EntityLoadingControlWriter, IEntityLoadingControlUpdater
{
    public EntityLoadingControl(global::Improbable.EntityId entityId, Improbable.Corelib.Worker.Checkout.EntityLoadingControlData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Corelib.Worker.Checkout.EntityLoadingControlDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(EntityLoadingControl));
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

    public Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates LoadedState { get { return Data.LoadedState; } }
    public int Tries { get { return Data.Tries; } }
    public int MaxTries { get { return Data.MaxTries; } }
    public int RetryWait { get { return Data.RetryWait; } }
    public bool TriggerCallbackOnTimeout { get { return Data.TriggerCallbackOnTimeout; } }
    public global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> Entities { get { return Data.Entities; } }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates>> updatedCallbacksLoadedState =
        new global::System.Collections.Generic.List<System.Action<Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates>>();
    public event System.Action<Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates> LoadedStateUpdated
    {
        add
        {
            updatedCallbacksLoadedState.Add(value);
            value(Data.LoadedState);
        }
        remove { updatedCallbacksLoadedState.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<int>> updatedCallbacksTries =
        new global::System.Collections.Generic.List<System.Action<int>>();
    public event System.Action<int> TriesUpdated
    {
        add
        {
            updatedCallbacksTries.Add(value);
            value(Data.Tries);
        }
        remove { updatedCallbacksTries.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<int>> updatedCallbacksMaxTries =
        new global::System.Collections.Generic.List<System.Action<int>>();
    public event System.Action<int> MaxTriesUpdated
    {
        add
        {
            updatedCallbacksMaxTries.Add(value);
            value(Data.MaxTries);
        }
        remove { updatedCallbacksMaxTries.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<int>> updatedCallbacksRetryWait =
        new global::System.Collections.Generic.List<System.Action<int>>();
    public event System.Action<int> RetryWaitUpdated
    {
        add
        {
            updatedCallbacksRetryWait.Add(value);
            value(Data.RetryWait);
        }
        remove { updatedCallbacksRetryWait.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<bool>> updatedCallbacksTriggerCallbackOnTimeout =
        new global::System.Collections.Generic.List<System.Action<bool>>();
    public event System.Action<bool> TriggerCallbackOnTimeoutUpdated
    {
        add
        {
            updatedCallbacksTriggerCallbackOnTimeout.Add(value);
            value(Data.TriggerCallbackOnTimeout);
        }
        remove { updatedCallbacksTriggerCallbackOnTimeout.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>>> updatedCallbacksEntities =
        new global::System.Collections.Generic.List<System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>>>();
    public event System.Action<global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId>> EntitiesUpdated
    {
        add
        {
            updatedCallbacksEntities.Add(value);
            value(Data.Entities);
        }
        remove { updatedCallbacksEntities.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, updatedCallbacksLoadedState);
        UnsubscribeEventHandler(visualizer, updatedCallbacksTries);
        UnsubscribeEventHandler(visualizer, updatedCallbacksMaxTries);
        UnsubscribeEventHandler(visualizer, updatedCallbacksRetryWait);
        UnsubscribeEventHandler(visualizer, updatedCallbacksTriggerCallbackOnTimeout);
        UnsubscribeEventHandler(visualizer, updatedCallbacksEntities);
    }

    public IEntityLoadingControlUpdater Update
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
                Updater = new EntityLoadingControlUpdate(EntityId, new bool[1], new Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData());
            }
            return this;
        }
    }

    IEntityLoadingControlUpdater IEntityLoadingControlUpdater.LoadedState(Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates newValue)
    {
        if (Updater.Proto.LoadedStateSpecified || !LoadedState.Equals(newValue))
        {
            Updater.Proto.LoadedState = (Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates) newValue;
        }
        return this;
    }

    IEntityLoadingControlUpdater IEntityLoadingControlUpdater.Tries(int newValue)
    {
        if (Updater.Proto.TriesSpecified || !Tries.Equals(newValue))
        {
            Updater.Proto.Tries = newValue;
        }
        return this;
    }

    IEntityLoadingControlUpdater IEntityLoadingControlUpdater.MaxTries(int newValue)
    {
        if (Updater.Proto.MaxTriesSpecified || !MaxTries.Equals(newValue))
        {
            Updater.Proto.MaxTries = newValue;
        }
        return this;
    }

    IEntityLoadingControlUpdater IEntityLoadingControlUpdater.RetryWait(int newValue)
    {
        if (Updater.Proto.RetryWaitSpecified || !RetryWait.Equals(newValue))
        {
            Updater.Proto.RetryWait = newValue;
        }
        return this;
    }

    IEntityLoadingControlUpdater IEntityLoadingControlUpdater.TriggerCallbackOnTimeout(bool newValue)
    {
        if (Updater.Proto.TriggerCallbackOnTimeoutSpecified || !TriggerCallbackOnTimeout.Equals(newValue))
        {
            Updater.Proto.TriggerCallbackOnTimeout = newValue;
        }
        return this;
    }

    IEntityLoadingControlUpdater IEntityLoadingControlUpdater.Entities(global::System.Collections.Generic.IList<Improbable.EntityId> newValue)
    {
        if ((Updater.Proto.Entities.Count > 0 || Updater.StatesToClear != null && Updater.StatesToClear[0]) || !global::Improbable.Util.Collections.CollectionUtil.ListsEqual(Entities, newValue))
        {
            global::Improbable.Tools.ToProto<Improbable.EntityId, long>(newValue, Updater.Proto.Entities, Improbable.EntityIdHelper.Instance);
            Updater.StatesToClear[0] = newValue.Count == 0;
        }
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        bool updatedLoadedState = update.LoadedStateSpecified;
        anythingUpdated |= updatedLoadedState;
        if (updatedLoadedState) TriggerCallbacks(updatedCallbacksLoadedState, Data.LoadedState);

        bool updatedTries = update.TriesSpecified;
        anythingUpdated |= updatedTries;
        if (updatedTries) TriggerCallbacks(updatedCallbacksTries, Data.Tries);

        bool updatedMaxTries = update.MaxTriesSpecified;
        anythingUpdated |= updatedMaxTries;
        if (updatedMaxTries) TriggerCallbacks(updatedCallbacksMaxTries, Data.MaxTries);

        bool updatedRetryWait = update.RetryWaitSpecified;
        anythingUpdated |= updatedRetryWait;
        if (updatedRetryWait) TriggerCallbacks(updatedCallbacksRetryWait, Data.RetryWait);

        bool updatedTriggerCallbackOnTimeout = update.TriggerCallbackOnTimeoutSpecified;
        anythingUpdated |= updatedTriggerCallbackOnTimeout;
        if (updatedTriggerCallbackOnTimeout) TriggerCallbacks(updatedCallbacksTriggerCallbackOnTimeout, Data.TriggerCallbackOnTimeout);

        bool updatedEntities = (update.Entities.Count > 0 || statesToClear != null && statesToClear[0]);
        anythingUpdated |= updatedEntities;
        if (updatedEntities) TriggerCallbacks(updatedCallbacksEntities, Data.Entities);

        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData stateUpdate)
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

public class EntityLoadingControlUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Corelib.Worker.Checkout.EntityLoadingControlData, Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData>
{
    public const int STATE_UPDATE_FIELD_ID = 190115;
    public EntityLoadingControlUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData proto)
        : base(entityId, statesToClear, Improbable.Corelib.Worker.Checkout.EntityLoadingControlDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new EntityLoadingControl(entityId, GetData(), stateSender);
    }

    public static EntityLoadingControlUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        bool[] statesToClear = new bool[1];
        for (int i = 0; i < proto.FieldsToClear.Count; i++)
        {
            statesToClear[FieldIdToIndex(proto.FieldsToClear[i])] = true;
        }
        return new EntityLoadingControlUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), statesToClear, protoState);
    }

    private static uint FieldIdToIndex(uint id)
    {
        switch (id)
        {
            case 6: //entities
                return 0;
            default:
                throw new ArgumentException(string.Format("Unexpected error: {0} is not a valid clearable field number for state Improbable.Corelib.Worker.Checkout.EntityLoadingControl.", id));
        }
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = { 6, };
}
}
