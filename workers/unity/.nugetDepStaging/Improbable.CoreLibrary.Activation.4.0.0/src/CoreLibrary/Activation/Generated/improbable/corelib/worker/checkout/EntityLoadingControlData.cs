// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.worker.checkout.EntityLoadingControlData in improbable/corelib/worker/checkout/entity_loading_control.proto.

namespace Improbable.Corelib.Worker.Checkout
{
public struct EntityLoadingControlData : global::System.IEquatable<EntityLoadingControlData>
{
    public readonly Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates LoadedState;
    public readonly int Tries;
    public readonly int MaxTries;
    public readonly int RetryWait;
    public readonly bool TriggerCallbackOnTimeout;
    public readonly global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> Entities;

    public EntityLoadingControlData (Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates loadedState,
        int tries,
        int maxTries,
        int retryWait,
        bool triggerCallbackOnTimeout,
        global::Improbable.Util.Collections.IReadOnlyList<Improbable.EntityId> entities)
    {
        LoadedState = loadedState;
        Tries = tries;
        MaxTries = maxTries;
        RetryWait = retryWait;
        TriggerCallbackOnTimeout = triggerCallbackOnTimeout;
        Entities = entities;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is EntityLoadingControlData))
            return false;
        return Equals((EntityLoadingControlData) obj);
    }

    public static bool operator ==(EntityLoadingControlData obj1, EntityLoadingControlData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(EntityLoadingControlData obj1, EntityLoadingControlData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(EntityLoadingControlData obj)
    {
        return true
            && LoadedState.Equals(obj.LoadedState)
            && Tries.Equals(obj.Tries)
            && MaxTries.Equals(obj.MaxTries)
            && RetryWait.Equals(obj.RetryWait)
            && TriggerCallbackOnTimeout.Equals(obj.TriggerCallbackOnTimeout)
            && global::Improbable.Util.Collections.CollectionUtil.ListsEqual(Entities, obj.Entities);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + LoadedState.GetHashCode();
        res = (res * 977) + Tries.GetHashCode();
        res = (res * 977) + MaxTries.GetHashCode();
        res = (res * 977) + RetryWait.GetHashCode();
        res = (res * 977) + TriggerCallbackOnTimeout.GetHashCode();
        res = (res * 977) + (Entities != null ? Entities.GetHashCode() : 0);
        return res;
    }

    public enum EntityLoadingStates {
        Requested = 0,
        Loaded = 1,
        Error = 2,
        Idle = 3
    }
}

//For internal use only, not to be used in user code.
public sealed class EntityLoadingControlDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Corelib.Worker.Checkout.EntityLoadingControlData, Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData>
{
    static readonly EntityLoadingControlDataHelper _instance = new EntityLoadingControlDataHelper();
    public static EntityLoadingControlDataHelper Instance { get { return _instance; } }
    private EntityLoadingControlDataHelper() {}

    public Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData ToProto(Improbable.Corelib.Worker.Checkout.EntityLoadingControlData data)
    {
        var proto = new Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData();
        proto.LoadedState = (Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates) data.LoadedState;
        proto.Tries = data.Tries;
        proto.MaxTries = data.MaxTries;
        proto.RetryWait = data.RetryWait;
        proto.TriggerCallbackOnTimeout = data.TriggerCallbackOnTimeout;
        global::Improbable.Tools.ToProto<Improbable.EntityId, long>(data.Entities, proto.Entities, Improbable.EntityIdHelper.Instance);
        return proto;
    }

    //Shallow merge method
    public Improbable.Corelib.Worker.Checkout.EntityLoadingControlData MergeFromProto(Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData proto, bool[] statesToClear, Improbable.Corelib.Worker.Checkout.EntityLoadingControlData data)
    {
        return new Improbable.Corelib.Worker.Checkout.EntityLoadingControlData(
            proto.LoadedStateSpecified ? (Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates) proto.LoadedState : data.LoadedState,
            proto.TriesSpecified ? proto.Tries : data.Tries,
            proto.MaxTriesSpecified ? proto.MaxTries : data.MaxTries,
            proto.RetryWaitSpecified ? proto.RetryWait : data.RetryWait,
            proto.TriggerCallbackOnTimeoutSpecified ? proto.TriggerCallbackOnTimeout : data.TriggerCallbackOnTimeout,
            (proto.Entities.Count > 0 || statesToClear != null && statesToClear[0]) ? global::Improbable.Tools.FromProto<Improbable.EntityId, long>(proto.Entities, Improbable.EntityIdHelper.Instance) : data.Entities
        );
    }

    public Improbable.Corelib.Worker.Checkout.EntityLoadingControlData FromProto(Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData proto)
    {
        return new Improbable.Corelib.Worker.Checkout.EntityLoadingControlData(
            (Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates) proto.LoadedState,
            proto.Tries,
            proto.MaxTries,
            proto.RetryWait,
            proto.TriggerCallbackOnTimeout,
            global::Improbable.Tools.FromProto<Improbable.EntityId, long>(proto.Entities, Improbable.EntityIdHelper.Instance)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData protoTo, bool[] statesToClearTo)
    {
        if (protoFrom.LoadedStateSpecified)
        {
            protoTo.LoadedState = protoFrom.LoadedState;
            protoTo.LoadedStateSpecified = protoFrom.LoadedStateSpecified;
        }
        if (protoFrom.TriesSpecified)
        {
            protoTo.Tries = protoFrom.Tries;
            protoTo.TriesSpecified = protoFrom.TriesSpecified;
        }
        if (protoFrom.MaxTriesSpecified)
        {
            protoTo.MaxTries = protoFrom.MaxTries;
            protoTo.MaxTriesSpecified = protoFrom.MaxTriesSpecified;
        }
        if (protoFrom.RetryWaitSpecified)
        {
            protoTo.RetryWait = protoFrom.RetryWait;
            protoTo.RetryWaitSpecified = protoFrom.RetryWaitSpecified;
        }
        if (protoFrom.TriggerCallbackOnTimeoutSpecified)
        {
            protoTo.TriggerCallbackOnTimeout = protoFrom.TriggerCallbackOnTimeout;
            protoTo.TriggerCallbackOnTimeoutSpecified = protoFrom.TriggerCallbackOnTimeoutSpecified;
        }
        if ((protoFrom.Entities.Count > 0 || statesToClearFrom != null && statesToClearFrom[0]))
        {
            statesToClearTo[0] = statesToClearFrom[0];
            protoTo.Entities.Clear();
            protoTo.Entities.AddRange(protoFrom.Entities);
        }
    }
}
}
