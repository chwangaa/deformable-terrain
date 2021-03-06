// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.metrics.ClientPhysicsLatencyReplyData in improbable/corelib/metrics/client_physics_latency_reply.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Corelib.Metrics
{
[ReaderInterface]
[CanonicalName("improbable.corelib.metrics.ClientPhysicsLatencyReply")]
public interface ClientPhysicsLatencyReplyReader : IEntityStateReader
{

    event System.Action<Improbable.Corelib.Metrics.ClientPhysicsPingReceived> ClientPhysicsPingReceived;
}

public interface IClientPhysicsLatencyReplyUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    IClientPhysicsLatencyReplyUpdater TriggerClientPhysicsPingReceived(
        int receivedPingTimestampMillis);
}

[WriterInterface]
[CanonicalName("improbable.corelib.metrics.ClientPhysicsLatencyReply")]
public interface ClientPhysicsLatencyReplyWriter : ClientPhysicsLatencyReplyReader, IUpdateable<IClientPhysicsLatencyReplyUpdater> { }

public class ClientPhysicsLatencyReply : global::Improbable.Entity.State.StateBase<Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData, Schema.Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData>, ClientPhysicsLatencyReplyWriter, IClientPhysicsLatencyReplyUpdater
{
    public ClientPhysicsLatencyReply(global::Improbable.EntityId entityId, Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(ClientPhysicsLatencyReply));
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

    
    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Corelib.Metrics.ClientPhysicsPingReceived>> eventCallbacksClientPhysicsPingReceived =
        new global::System.Collections.Generic.List<System.Action<Improbable.Corelib.Metrics.ClientPhysicsPingReceived>>();
    public event System.Action<Improbable.Corelib.Metrics.ClientPhysicsPingReceived> ClientPhysicsPingReceived
    {
        add { eventCallbacksClientPhysicsPingReceived.Add(value); }
        remove { eventCallbacksClientPhysicsPingReceived.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, eventCallbacksClientPhysicsPingReceived);
    }

    public IClientPhysicsLatencyReplyUpdater Update
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
                Updater = new ClientPhysicsLatencyReplyUpdate(EntityId, new bool[0], new Schema.Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData());
            }
            return this;
        }
    }

    global::System.Collections.Generic.IList<Improbable.Corelib.Metrics.ClientPhysicsPingReceived> triggeredClientPhysicsPingReceived = new global::System.Collections.Generic.List<Improbable.Corelib.Metrics.ClientPhysicsPingReceived>();
    IClientPhysicsLatencyReplyUpdater IClientPhysicsLatencyReplyUpdater.TriggerClientPhysicsPingReceived(
        int receivedPingTimestampMillis)
    {
        var eventData = new Improbable.Corelib.Metrics.ClientPhysicsPingReceived(
            receivedPingTimestampMillis);
        triggeredClientPhysicsPingReceived.Add(eventData);
        Updater.Proto.ClientPhysicsPingReceived.Add(Improbable.Corelib.Metrics.ClientPhysicsPingReceivedHelper.Instance.ToProto(eventData));
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData stateUpdate)
    {
        bool anythingUpdated = false;
        bool updatedClientPhysicsPingReceived = stateUpdate.ClientPhysicsPingReceived.Count > 0;
        TriggerEventCallbacks(eventCallbacksClientPhysicsPingReceived, stateUpdate.ClientPhysicsPingReceived, Improbable.Corelib.Metrics.ClientPhysicsPingReceivedHelper.Instance);
        anythingUpdated |= updatedClientPhysicsPingReceived;
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents()
    {
        bool anythingUpdated = false;
        bool updatedClientPhysicsPingReceived = triggeredClientPhysicsPingReceived.Count > 0;
        TriggerEventCallbacks(eventCallbacksClientPhysicsPingReceived, triggeredClientPhysicsPingReceived);
        if (triggeredClientPhysicsPingReceived != null) triggeredClientPhysicsPingReceived.Clear();
        anythingUpdated |= updatedClientPhysicsPingReceived;
        return anythingUpdated;
    }
}

public class ClientPhysicsLatencyReplyUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData, Schema.Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData>
{
    public const int STATE_UPDATE_FIELD_ID = 190101;
    public ClientPhysicsLatencyReplyUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData proto)
        : base(entityId, statesToClear, Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new ClientPhysicsLatencyReply(entityId, GetData(), stateSender);
    }

    public static ClientPhysicsLatencyReplyUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Corelib.Metrics.ClientPhysicsLatencyReplyData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        return new ClientPhysicsLatencyReplyUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), null, protoState);
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = {};
}
}
