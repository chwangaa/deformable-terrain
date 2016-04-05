// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.bot.BotTargetVelocityData in improbable/entity/bot/bot_target_velocity.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Entity.Bot
{
[ReaderInterface]
[CanonicalName("improbable.entity.bot.BotTargetVelocity")]
public interface BotTargetVelocityReader : IEntityStateReader
{
    Improbable.Math.Vector3d TargetVelocity { get; }

    event System.Action<Improbable.Math.Vector3d> TargetVelocityUpdated;
}

public interface IBotTargetVelocityUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    IBotTargetVelocityUpdater TargetVelocity(Improbable.Math.Vector3d newValue);
}

[WriterInterface]
[CanonicalName("improbable.entity.bot.BotTargetVelocity")]
public interface BotTargetVelocityWriter : BotTargetVelocityReader, IUpdateable<IBotTargetVelocityUpdater> { }

public class BotTargetVelocity : global::Improbable.Entity.State.StateBase<Improbable.Entity.Bot.BotTargetVelocityData, Schema.Improbable.Entity.Bot.BotTargetVelocityData>, BotTargetVelocityWriter, IBotTargetVelocityUpdater
{
    public BotTargetVelocity(global::Improbable.EntityId entityId, Improbable.Entity.Bot.BotTargetVelocityData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Entity.Bot.BotTargetVelocityDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(BotTargetVelocity));
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

    public Improbable.Math.Vector3d TargetVelocity { get { return Data.TargetVelocity; } }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Math.Vector3d>> updatedCallbacksTargetVelocity =
        new global::System.Collections.Generic.List<System.Action<Improbable.Math.Vector3d>>();
    public event System.Action<Improbable.Math.Vector3d> TargetVelocityUpdated
    {
        add
        {
            updatedCallbacksTargetVelocity.Add(value);
            value(Data.TargetVelocity);
        }
        remove { updatedCallbacksTargetVelocity.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, updatedCallbacksTargetVelocity);
    }

    public IBotTargetVelocityUpdater Update
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
                Updater = new BotTargetVelocityUpdate(EntityId, new bool[0], new Schema.Improbable.Entity.Bot.BotTargetVelocityData());
            }
            return this;
        }
    }

    IBotTargetVelocityUpdater IBotTargetVelocityUpdater.TargetVelocity(Improbable.Math.Vector3d newValue)
    {
        if (Updater.Proto.TargetVelocity != null || !TargetVelocity.Equals(newValue))
        {
            Updater.Proto.TargetVelocity = Improbable.Math.Vector3dHelper.Instance.ToProto(newValue);
        }
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Entity.Bot.BotTargetVelocityData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        bool updatedTargetVelocity = update.TargetVelocity != null;
        anythingUpdated |= updatedTargetVelocity;
        if (updatedTargetVelocity) TriggerCallbacks(updatedCallbacksTargetVelocity, Data.TargetVelocity);

        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Entity.Bot.BotTargetVelocityData stateUpdate)
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

public class BotTargetVelocityUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Entity.Bot.BotTargetVelocityData, Schema.Improbable.Entity.Bot.BotTargetVelocityData>
{
    public const int STATE_UPDATE_FIELD_ID = 190129;
    public BotTargetVelocityUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Entity.Bot.BotTargetVelocityData proto)
        : base(entityId, statesToClear, Improbable.Entity.Bot.BotTargetVelocityDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new BotTargetVelocity(entityId, GetData(), stateSender);
    }

    public static BotTargetVelocityUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Entity.Bot.BotTargetVelocityData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        return new BotTargetVelocityUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), null, protoState);
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = {};
}
}