// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.physical.Impulse in improbable/entity/physical/rigidbody_data.proto.

namespace Improbable.Entity.Physical
{
public struct Impulse : global::System.IEquatable<Impulse>
{
    public readonly Improbable.Math.Vector3d Value;

    public Impulse (Improbable.Math.Vector3d value)
    {
        Value = value;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Impulse))
            return false;
        return Equals((Impulse) obj);
    }

    public static bool operator ==(Impulse obj1, Impulse obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(Impulse obj1, Impulse obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(Impulse obj)
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
public sealed class ImpulseHelper : global::Improbable.Tools.IProtoConverter<Improbable.Entity.Physical.Impulse, Schema.Improbable.Entity.Physical.Impulse>
{
    static readonly ImpulseHelper _instance = new ImpulseHelper();
    public static ImpulseHelper Instance { get { return _instance; } }
    private ImpulseHelper() {}

    public Schema.Improbable.Entity.Physical.Impulse ToProto(Improbable.Entity.Physical.Impulse data)
    {
        var proto = new Schema.Improbable.Entity.Physical.Impulse();
        proto.Value = Improbable.Math.Vector3dHelper.Instance.ToProto(data.Value);
        return proto;
    }

    //Shallow merge method
    public Improbable.Entity.Physical.Impulse MergeFromProto(Schema.Improbable.Entity.Physical.Impulse proto, bool[] statesToClear, Improbable.Entity.Physical.Impulse data)
    {
        return new Improbable.Entity.Physical.Impulse(
            proto.Value != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Value) : data.Value
        );
    }

    public Improbable.Entity.Physical.Impulse FromProto(Schema.Improbable.Entity.Physical.Impulse proto)
    {
        return new Improbable.Entity.Physical.Impulse(
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Value)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Entity.Physical.Impulse protoFrom, bool[] statesToClearFrom, Schema.Improbable.Entity.Physical.Impulse protoTo, bool[] statesToClearTo)
    {
        if (protoFrom.Value != null)
        {
            protoTo.Value = protoFrom.Value;
        }
    }
}
}
