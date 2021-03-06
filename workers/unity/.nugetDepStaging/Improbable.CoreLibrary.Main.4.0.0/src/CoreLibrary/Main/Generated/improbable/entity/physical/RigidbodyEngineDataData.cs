// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.physical.RigidbodyEngineDataData in improbable/entity/physical/rigidbody_engine_data.proto.

namespace Improbable.Entity.Physical
{
public struct RigidbodyEngineDataData : global::System.IEquatable<RigidbodyEngineDataData>
{
    public readonly Improbable.Math.Vector3d Velocity;
    public readonly Improbable.Math.Vector3d AngularVelocity;
    public readonly Improbable.Math.Vector3d RelativeCentreOfMass;

    public RigidbodyEngineDataData (Improbable.Math.Vector3d velocity,
        Improbable.Math.Vector3d angularVelocity,
        Improbable.Math.Vector3d relativeCentreOfMass)
    {
        Velocity = velocity;
        AngularVelocity = angularVelocity;
        RelativeCentreOfMass = relativeCentreOfMass;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is RigidbodyEngineDataData))
            return false;
        return Equals((RigidbodyEngineDataData) obj);
    }

    public static bool operator ==(RigidbodyEngineDataData obj1, RigidbodyEngineDataData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(RigidbodyEngineDataData obj1, RigidbodyEngineDataData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(RigidbodyEngineDataData obj)
    {
        return true
            && Velocity.Equals(obj.Velocity)
            && AngularVelocity.Equals(obj.AngularVelocity)
            && RelativeCentreOfMass.Equals(obj.RelativeCentreOfMass);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + Velocity.GetHashCode();
        res = (res * 977) + AngularVelocity.GetHashCode();
        res = (res * 977) + RelativeCentreOfMass.GetHashCode();
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class RigidbodyEngineDataDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Entity.Physical.RigidbodyEngineDataData, Schema.Improbable.Entity.Physical.RigidbodyEngineDataData>
{
    static readonly RigidbodyEngineDataDataHelper _instance = new RigidbodyEngineDataDataHelper();
    public static RigidbodyEngineDataDataHelper Instance { get { return _instance; } }
    private RigidbodyEngineDataDataHelper() {}

    public Schema.Improbable.Entity.Physical.RigidbodyEngineDataData ToProto(Improbable.Entity.Physical.RigidbodyEngineDataData data)
    {
        var proto = new Schema.Improbable.Entity.Physical.RigidbodyEngineDataData();
        proto.Velocity = Improbable.Math.Vector3dHelper.Instance.ToProto(data.Velocity);
        proto.AngularVelocity = Improbable.Math.Vector3dHelper.Instance.ToProto(data.AngularVelocity);
        proto.RelativeCentreOfMass = Improbable.Math.Vector3dHelper.Instance.ToProto(data.RelativeCentreOfMass);
        return proto;
    }

    //Shallow merge method
    public Improbable.Entity.Physical.RigidbodyEngineDataData MergeFromProto(Schema.Improbable.Entity.Physical.RigidbodyEngineDataData proto, bool[] statesToClear, Improbable.Entity.Physical.RigidbodyEngineDataData data)
    {
        return new Improbable.Entity.Physical.RigidbodyEngineDataData(
            proto.Velocity != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Velocity) : data.Velocity,
            proto.AngularVelocity != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.AngularVelocity) : data.AngularVelocity,
            proto.RelativeCentreOfMass != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.RelativeCentreOfMass) : data.RelativeCentreOfMass
        );
    }

    public Improbable.Entity.Physical.RigidbodyEngineDataData FromProto(Schema.Improbable.Entity.Physical.RigidbodyEngineDataData proto)
    {
        return new Improbable.Entity.Physical.RigidbodyEngineDataData(
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Velocity),
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.AngularVelocity),
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.RelativeCentreOfMass)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Entity.Physical.RigidbodyEngineDataData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Entity.Physical.RigidbodyEngineDataData protoTo, bool[] statesToClearTo)
    {
        if (protoFrom.Velocity != null)
        {
            protoTo.Velocity = protoFrom.Velocity;
        }
        if (protoFrom.AngularVelocity != null)
        {
            protoTo.AngularVelocity = protoFrom.AngularVelocity;
        }
        if (protoFrom.RelativeCentreOfMass != null)
        {
            protoTo.RelativeCentreOfMass = protoFrom.RelativeCentreOfMass;
        }
    }
}
}
