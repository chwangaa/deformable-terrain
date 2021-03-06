// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.metrics.EngineLatencyData in improbable/corelib/metrics/engine_latency.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Corelib.Metrics
{
[ReaderInterface]
[CanonicalName("improbable.corelib.metrics.EngineLatency")]
public interface EngineLatencyReader : IEntityStateReader
{
    int RoundTripMillis { get; }
    int RefreshPeriodMillis { get; }

    event System.Action<int> RoundTripMillisUpdated;
    event System.Action<int> RefreshPeriodMillisUpdated;
    event System.Action<Improbable.Corelib.Metrics.EnginePingSent> EnginePingSent;
}

public interface IEngineLatencyUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    IEngineLatencyUpdater RoundTripMillis(int newValue);
    IEngineLatencyUpdater RefreshPeriodMillis(int newValue);
    IEngineLatencyUpdater TriggerEnginePingSent(
        int timestampMillis);
}

[WriterInterface]
[CanonicalName("improbable.corelib.metrics.EngineLatency")]
public interface EngineLatencyWriter : EngineLatencyReader, IUpdateable<IEngineLatencyUpdater> { }

public class EngineLatency : global::Improbable.Entity.State.StateBase<Improbable.Corelib.Metrics.EngineLatencyData, Schema.Improbable.Corelib.Metrics.EngineLatencyData>, EngineLatencyWriter, IEngineLatencyUpdater
{
    public EngineLatency(global::Improbable.EntityId entityId, Improbable.Corelib.Metrics.EngineLatencyData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Corelib.Metrics.EngineLatencyDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(EngineLatency));
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

    public int RoundTripMillis { get { return Data.RoundTripMillis; } }
    public int RefreshPeriodMillis { get { return Data.RefreshPeriodMillis; } }

    private readonly global::System.Collections.Generic.List<System.Action<int>> updatedCallbacksRoundTripMillis =
        new global::System.Collections.Generic.List<System.Action<int>>();
    public event System.Action<int> RoundTripMillisUpdated
    {
        add
        {
            updatedCallbacksRoundTripMillis.Add(value);
            value(Data.RoundTripMillis);
        }
        remove { updatedCallbacksRoundTripMillis.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<int>> updatedCallbacksRefreshPeriodMillis =
        new global::System.Collections.Generic.List<System.Action<int>>();
    public event System.Action<int> RefreshPeriodMillisUpdated
    {
        add
        {
            updatedCallbacksRefreshPeriodMillis.Add(value);
            value(Data.RefreshPeriodMillis);
        }
        remove { updatedCallbacksRefreshPeriodMillis.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Corelib.Metrics.EnginePingSent>> eventCallbacksEnginePingSent =
        new global::System.Collections.Generic.List<System.Action<Improbable.Corelib.Metrics.EnginePingSent>>();
    public event System.Action<Improbable.Corelib.Metrics.EnginePingSent> EnginePingSent
    {
        add { eventCallbacksEnginePingSent.Add(value); }
        remove { eventCallbacksEnginePingSent.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, updatedCallbacksRoundTripMillis);
        UnsubscribeEventHandler(visualizer, updatedCallbacksRefreshPeriodMillis);
        UnsubscribeEventHandler(visualizer, eventCallbacksEnginePingSent);
    }

    public IEngineLatencyUpdater Update
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
                Updater = new EngineLatencyUpdate(EntityId, new bool[0], new Schema.Improbable.Corelib.Metrics.EngineLatencyData());
            }
            return this;
        }
    }

    IEngineLatencyUpdater IEngineLatencyUpdater.RoundTripMillis(int newValue)
    {
        if (Updater.Proto.RoundTripMillisSpecified || !RoundTripMillis.Equals(newValue))
        {
            Updater.Proto.RoundTripMillis = newValue;
        }
        return this;
    }

    IEngineLatencyUpdater IEngineLatencyUpdater.RefreshPeriodMillis(int newValue)
    {
        if (Updater.Proto.RefreshPeriodMillisSpecified || !RefreshPeriodMillis.Equals(newValue))
        {
            Updater.Proto.RefreshPeriodMillis = newValue;
        }
        return this;
    }

    global::System.Collections.Generic.IList<Improbable.Corelib.Metrics.EnginePingSent> triggeredEnginePingSent = new global::System.Collections.Generic.List<Improbable.Corelib.Metrics.EnginePingSent>();
    IEngineLatencyUpdater IEngineLatencyUpdater.TriggerEnginePingSent(
        int timestampMillis)
    {
        var eventData = new Improbable.Corelib.Metrics.EnginePingSent(
            timestampMillis);
        triggeredEnginePingSent.Add(eventData);
        Updater.Proto.EnginePingSent.Add(Improbable.Corelib.Metrics.EnginePingSentHelper.Instance.ToProto(eventData));
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Corelib.Metrics.EngineLatencyData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        bool updatedRoundTripMillis = update.RoundTripMillisSpecified;
        anythingUpdated |= updatedRoundTripMillis;
        if (updatedRoundTripMillis) TriggerCallbacks(updatedCallbacksRoundTripMillis, Data.RoundTripMillis);

        bool updatedRefreshPeriodMillis = update.RefreshPeriodMillisSpecified;
        anythingUpdated |= updatedRefreshPeriodMillis;
        if (updatedRefreshPeriodMillis) TriggerCallbacks(updatedCallbacksRefreshPeriodMillis, Data.RefreshPeriodMillis);

        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Corelib.Metrics.EngineLatencyData stateUpdate)
    {
        bool anythingUpdated = false;
        bool updatedEnginePingSent = stateUpdate.EnginePingSent.Count > 0;
        TriggerEventCallbacks(eventCallbacksEnginePingSent, stateUpdate.EnginePingSent, Improbable.Corelib.Metrics.EnginePingSentHelper.Instance);
        anythingUpdated |= updatedEnginePingSent;
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents()
    {
        bool anythingUpdated = false;
        bool updatedEnginePingSent = triggeredEnginePingSent.Count > 0;
        TriggerEventCallbacks(eventCallbacksEnginePingSent, triggeredEnginePingSent);
        if (triggeredEnginePingSent != null) triggeredEnginePingSent.Clear();
        anythingUpdated |= updatedEnginePingSent;
        return anythingUpdated;
    }
}

public class EngineLatencyUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Corelib.Metrics.EngineLatencyData, Schema.Improbable.Corelib.Metrics.EngineLatencyData>
{
    public const int STATE_UPDATE_FIELD_ID = 190102;
    public EngineLatencyUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Corelib.Metrics.EngineLatencyData proto)
        : base(entityId, statesToClear, Improbable.Corelib.Metrics.EngineLatencyDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new EngineLatency(entityId, GetData(), stateSender);
    }

    public static EngineLatencyUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Corelib.Metrics.EngineLatencyData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        return new EngineLatencyUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), null, protoState);
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = {};
}
}
