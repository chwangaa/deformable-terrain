// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.physical.TeleportAckData in improbable/entity/physical/teleport_ack.proto.

namespace Improbable.Entity.Physical
{
public struct TeleportAckData : global::System.IEquatable<TeleportAckData>
{
    public readonly int LastExecutedRequestId;

    public TeleportAckData (int lastExecutedRequestId)
    {
        LastExecutedRequestId = lastExecutedRequestId;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is TeleportAckData))
            return false;
        return Equals((TeleportAckData) obj);
    }

    public static bool operator ==(TeleportAckData obj1, TeleportAckData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(TeleportAckData obj1, TeleportAckData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(TeleportAckData obj)
    {
        return true
            && LastExecutedRequestId.Equals(obj.LastExecutedRequestId);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + LastExecutedRequestId.GetHashCode();
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class TeleportAckDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Entity.Physical.TeleportAckData, Schema.Improbable.Entity.Physical.TeleportAckData>
{
    static readonly TeleportAckDataHelper _instance = new TeleportAckDataHelper();
    public static TeleportAckDataHelper Instance { get { return _instance; } }
    private TeleportAckDataHelper() {}

    public Schema.Improbable.Entity.Physical.TeleportAckData ToProto(Improbable.Entity.Physical.TeleportAckData data)
    {
        var proto = new Schema.Improbable.Entity.Physical.TeleportAckData();
        proto.LastExecutedRequestId = data.LastExecutedRequestId;
        return proto;
    }

    //Shallow merge method
    public Improbable.Entity.Physical.TeleportAckData MergeFromProto(Schema.Improbable.Entity.Physical.TeleportAckData proto, bool[] statesToClear, Improbable.Entity.Physical.TeleportAckData data)
    {
        return new Improbable.Entity.Physical.TeleportAckData(
            proto.LastExecutedRequestIdSpecified ? proto.LastExecutedRequestId : data.LastExecutedRequestId
        );
    }

    public Improbable.Entity.Physical.TeleportAckData FromProto(Schema.Improbable.Entity.Physical.TeleportAckData proto)
    {
        return new Improbable.Entity.Physical.TeleportAckData(
            proto.LastExecutedRequestId
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Entity.Physical.TeleportAckData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Entity.Physical.TeleportAckData protoTo, bool[] statesToClearTo)
    {
        if (protoFrom.LastExecutedRequestIdSpecified)
        {
            protoTo.LastExecutedRequestId = protoFrom.LastExecutedRequestId;
            protoTo.LastExecutedRequestIdSpecified = protoFrom.LastExecutedRequestIdSpecified;
        }
    }
}
}