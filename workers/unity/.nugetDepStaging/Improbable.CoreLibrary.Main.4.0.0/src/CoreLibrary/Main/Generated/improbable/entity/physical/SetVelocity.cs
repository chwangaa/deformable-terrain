// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.physical.SetVelocity in improbable/entity/physical/rigidbody_data.proto.

namespace Improbable.Entity.Physical
{
public struct SetVelocity : global::System.IEquatable<SetVelocity>
{
    public readonly Improbable.Math.Vector3d Value;

    public SetVelocity (Improbable.Math.Vector3d value)
    {
        Value = value;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is SetVelocity))
            return false;
        return Equals((SetVelocity) obj);
    }

    public static bool operator ==(SetVelocity obj1, SetVelocity obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(SetVelocity obj1, SetVelocity obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(SetVelocity obj)
    {
        return true
            && Value.Equals(obj.Value);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + Value.GetHashCode();
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class SetVelocityHelper : global::Improbable.Tools.IProtoConverter<Improbable.Entity.Physical.SetVelocity, Schema.Improbable.Entity.Physical.SetVelocity>
{
    static readonly SetVelocityHelper _instance = new SetVelocityHelper();
    public static SetVelocityHelper Instance { get { return _instance; } }
    private SetVelocityHelper() {}

    public Schema.Improbable.Entity.Physical.SetVelocity ToProto(Improbable.Entity.Physical.SetVelocity data)
    {
        var proto = new Schema.Improbable.Entity.Physical.SetVelocity();
        proto.Value = Improbable.Math.Vector3dHelper.Instance.ToProto(data.Value);
        return proto;
    }

    //Shallow merge method
    public Improbable.Entity.Physical.SetVelocity MergeFromProto(Schema.Improbable.Entity.Physical.SetVelocity proto, bool[] statesToClear, Improbable.Entity.Physical.SetVelocity data)
    {
        return new Improbable.Entity.Physical.SetVelocity(
            proto.Value != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Value) : data.Value
        );
    }

    public Improbable.Entity.Physical.SetVelocity FromProto(Schema.Improbable.Entity.Physical.SetVelocity proto)
    {
        return new Improbable.Entity.Physical.SetVelocity(
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Value)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Entity.Physical.SetVelocity protoFrom, bool[] statesToClearFrom, Schema.Improbable.Entity.Physical.SetVelocity protoTo, bool[] statesToClearTo)
    {
        if (protoFrom.Value != null)
        {
            protoTo.Value = protoFrom.Value;
        }
    }
}
}