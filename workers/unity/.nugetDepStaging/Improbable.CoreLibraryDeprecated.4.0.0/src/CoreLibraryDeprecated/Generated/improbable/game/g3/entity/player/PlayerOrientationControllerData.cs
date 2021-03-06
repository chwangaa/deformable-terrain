// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.game.g3.entity.player.PlayerOrientationControllerDataData in improbable/game/g3/entity/player/player_orientation_controller_data.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Game.G3.Entity.Player
{
[ReaderInterface]
[CanonicalName("improbable.game.g3.entity.player.PlayerOrientationControllerData")]
public interface PlayerOrientationControllerDataReader : IEntityStateReader
{
    float Azimuth { get; }
    float Pitch { get; }

    event System.Action<float> AzimuthUpdated;
    event System.Action<float> PitchUpdated;
}

public interface IPlayerOrientationControllerDataUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    IPlayerOrientationControllerDataUpdater Azimuth(float newValue);
    IPlayerOrientationControllerDataUpdater Pitch(float newValue);
}

[WriterInterface]
[CanonicalName("improbable.game.g3.entity.player.PlayerOrientationControllerData")]
public interface PlayerOrientationControllerDataWriter : PlayerOrientationControllerDataReader, IUpdateable<IPlayerOrientationControllerDataUpdater> { }

public class PlayerOrientationControllerData : global::Improbable.Entity.State.StateBase<Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData, Schema.Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData>, PlayerOrientationControllerDataWriter, IPlayerOrientationControllerDataUpdater
{
    public PlayerOrientationControllerData(global::Improbable.EntityId entityId, Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(PlayerOrientationControllerData));
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

    public float Azimuth { get { return Data.Azimuth; } }
    public float Pitch { get { return Data.Pitch; } }

    private readonly global::System.Collections.Generic.List<System.Action<float>> updatedCallbacksAzimuth =
        new global::System.Collections.Generic.List<System.Action<float>>();
    public event System.Action<float> AzimuthUpdated
    {
        add
        {
            updatedCallbacksAzimuth.Add(value);
            value(Data.Azimuth);
        }
        remove { updatedCallbacksAzimuth.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<float>> updatedCallbacksPitch =
        new global::System.Collections.Generic.List<System.Action<float>>();
    public event System.Action<float> PitchUpdated
    {
        add
        {
            updatedCallbacksPitch.Add(value);
            value(Data.Pitch);
        }
        remove { updatedCallbacksPitch.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, updatedCallbacksAzimuth);
        UnsubscribeEventHandler(visualizer, updatedCallbacksPitch);
    }

    public IPlayerOrientationControllerDataUpdater Update
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
                Updater = new PlayerOrientationControllerDataUpdate(EntityId, new bool[0], new Schema.Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData());
            }
            return this;
        }
    }

    IPlayerOrientationControllerDataUpdater IPlayerOrientationControllerDataUpdater.Azimuth(float newValue)
    {
        if (Updater.Proto.AzimuthSpecified || !Azimuth.Equals(newValue))
        {
            Updater.Proto.Azimuth = newValue;
        }
        return this;
    }

    IPlayerOrientationControllerDataUpdater IPlayerOrientationControllerDataUpdater.Pitch(float newValue)
    {
        if (Updater.Proto.PitchSpecified || !Pitch.Equals(newValue))
        {
            Updater.Proto.Pitch = newValue;
        }
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        bool updatedAzimuth = update.AzimuthSpecified;
        anythingUpdated |= updatedAzimuth;
        if (updatedAzimuth) TriggerCallbacks(updatedCallbacksAzimuth, Data.Azimuth);

        bool updatedPitch = update.PitchSpecified;
        anythingUpdated |= updatedPitch;
        if (updatedPitch) TriggerCallbacks(updatedCallbacksPitch, Data.Pitch);

        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData stateUpdate)
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

public class PlayerOrientationControllerDataUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData, Schema.Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData>
{
    public const int STATE_UPDATE_FIELD_ID = 190130;
    public PlayerOrientationControllerDataUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData proto)
        : base(entityId, statesToClear, Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new PlayerOrientationControllerData(entityId, GetData(), stateSender);
    }

    public static PlayerOrientationControllerDataUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Game.G3.Entity.Player.PlayerOrientationControllerDataData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        return new PlayerOrientationControllerDataUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), null, protoState);
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = {};
}
}
