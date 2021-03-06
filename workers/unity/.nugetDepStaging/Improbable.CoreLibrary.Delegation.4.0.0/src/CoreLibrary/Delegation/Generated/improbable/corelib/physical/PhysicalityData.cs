// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.physical.PhysicalityData in improbable/corelib/physical/physicality.proto.

namespace Improbable.Corelib.Physical
{
public struct PhysicalityData : global::System.IEquatable<PhysicalityData>
{
    public readonly bool IsPhysical;

    public PhysicalityData (bool isPhysical)
    {
        IsPhysical = isPhysical;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is PhysicalityData))
            return false;
        return Equals((PhysicalityData) obj);
    }

    public static bool operator ==(PhysicalityData obj1, PhysicalityData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(PhysicalityData obj1, PhysicalityData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(PhysicalityData obj)
    {
        return true
            && IsPhysical.Equals(obj.IsPhysical);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + IsPhysical.GetHashCode();
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class PhysicalityDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Corelib.Physical.PhysicalityData, Schema.Improbable.Corelib.Physical.PhysicalityData>
{
    static readonly PhysicalityDataHelper _instance = new PhysicalityDataHelper();
    public static PhysicalityDataHelper Instance { get { return _instance; } }
    private PhysicalityDataHelper() {}

    public Schema.Improbable.Corelib.Physical.PhysicalityData ToProto(Improbable.Corelib.Physical.PhysicalityData data)
    {
        var proto = new Schema.Improbable.Corelib.Physical.PhysicalityData();
        proto.IsPhysical = data.IsPhysical;
        return proto;
    }

    //Shallow merge method
    public Improbable.Corelib.Physical.PhysicalityData MergeFromProto(Schema.Improbable.Corelib.Physical.PhysicalityData proto, bool[] statesToClear, Improbable.Corelib.Physical.PhysicalityData data)
    {
        return new Improbable.Corelib.Physical.PhysicalityData(
            proto.IsPhysicalSpecified ? proto.IsPhysical : data.IsPhysical
        );
    }

    public Improbable.Corelib.Physical.PhysicalityData FromProto(Schema.Improbable.Corelib.Physical.PhysicalityData proto)
    {
        return new Improbable.Corelib.Physical.PhysicalityData(
            proto.IsPhysical
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Corelib.Physical.PhysicalityData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Corelib.Physical.PhysicalityData protoTo, bool[] statesToClearTo)
    {
        if (protoFrom.IsPhysicalSpecified)
        {
            protoTo.IsPhysical = protoFrom.IsPhysical;
            protoTo.IsPhysicalSpecified = protoFrom.IsPhysicalSpecified;
        }
    }
}
}
