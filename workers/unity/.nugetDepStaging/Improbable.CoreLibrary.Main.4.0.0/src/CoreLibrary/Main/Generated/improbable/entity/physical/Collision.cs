// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.physical.Collision in improbable/entity/physical/collision_state.proto.

namespace Improbable.Entity.Physical
{
public struct Collision : global::System.IEquatable<Collision>
{
    public readonly Improbable.EntityId? EntityHit;
    public readonly Improbable.Math.Coordinates Point;
    public readonly Improbable.Math.Vector3d Normal;
    public readonly Improbable.Math.Vector3d RelativeVelocity;

    public Collision (Improbable.EntityId? entityHit,
        Improbable.Math.Coordinates point,
        Improbable.Math.Vector3d normal,
        Improbable.Math.Vector3d relativeVelocity)
    {
        EntityHit = entityHit;
        Point = point;
        Normal = normal;
        RelativeVelocity = relativeVelocity;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Collision))
            return false;
        return Equals((Collision) obj);
    }

    public static bool operator ==(Collision obj1, Collision obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(Collision obj1, Collision obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(Collision obj)
    {
        return true
            && global::Improbable.Util.Collections.CollectionUtil.OptionsEqual(EntityHit, obj.EntityHit)
            && Point.Equals(obj.Point)
            && Normal.Equals(obj.Normal)
            && RelativeVelocity.Equals(obj.RelativeVelocity);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + (EntityHit != null ? EntityHit.GetHashCode() : 0);
        res = (res * 977) + Point.GetHashCode();
        res = (res * 977) + Normal.GetHashCode();
        res = (res * 977) + RelativeVelocity.GetHashCode();
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class CollisionHelper : global::Improbable.Tools.IProtoConverter<Improbable.Entity.Physical.Collision, Schema.Improbable.Entity.Physical.Collision>
{
    static readonly CollisionHelper _instance = new CollisionHelper();
    public static CollisionHelper Instance { get { return _instance; } }
    private CollisionHelper() {}

    public Schema.Improbable.Entity.Physical.Collision ToProto(Improbable.Entity.Physical.Collision data)
    {
        var proto = new Schema.Improbable.Entity.Physical.Collision();
        if (data.EntityHit != null) proto.EntityHit = Improbable.EntityIdHelper.Instance.ToProto(data.EntityHit.Value);
        proto.Point = Improbable.Math.CoordinatesHelper.Instance.ToProto(data.Point);
        proto.Normal = Improbable.Math.Vector3dHelper.Instance.ToProto(data.Normal);
        proto.RelativeVelocity = Improbable.Math.Vector3dHelper.Instance.ToProto(data.RelativeVelocity);
        return proto;
    }

    //Shallow merge method
    public Improbable.Entity.Physical.Collision MergeFromProto(Schema.Improbable.Entity.Physical.Collision proto, bool[] statesToClear, Improbable.Entity.Physical.Collision data)
    {
        return new Improbable.Entity.Physical.Collision(
            (proto.EntityHitSpecified || statesToClear != null && statesToClear[0]) ? (!proto.EntityHitSpecified ? (Improbable.EntityId?) null : Improbable.EntityIdHelper.Instance.FromProto(proto.EntityHit)) : data.EntityHit,
            proto.Point != null ? Improbable.Math.CoordinatesHelper.Instance.FromProto(proto.Point) : data.Point,
            proto.Normal != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Normal) : data.Normal,
            proto.RelativeVelocity != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.RelativeVelocity) : data.RelativeVelocity
        );
    }

    public Improbable.Entity.Physical.Collision FromProto(Schema.Improbable.Entity.Physical.Collision proto)
    {
        return new Improbable.Entity.Physical.Collision(
            !proto.EntityHitSpecified ? (Improbable.EntityId?) null : Improbable.EntityIdHelper.Instance.FromProto(proto.EntityHit),
            Improbable.Math.CoordinatesHelper.Instance.FromProto(proto.Point),
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.Normal),
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.RelativeVelocity)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Entity.Physical.Collision protoFrom, bool[] statesToClearFrom, Schema.Improbable.Entity.Physical.Collision protoTo, bool[] statesToClearTo)
    {
        if ((protoFrom.EntityHitSpecified || statesToClearFrom != null && statesToClearFrom[0]))
        {
            statesToClearTo[0] = statesToClearFrom[0];
            protoTo.EntityHit = protoFrom.EntityHit;
            protoTo.EntityHitSpecified = protoFrom.EntityHitSpecified;
        }
        if (protoFrom.Point != null)
        {
            protoTo.Point = protoFrom.Point;
        }
        if (protoFrom.Normal != null)
        {
            protoTo.Normal = protoFrom.Normal;
        }
        if (protoFrom.RelativeVelocity != null)
        {
            protoTo.RelativeVelocity = protoFrom.RelativeVelocity;
        }
    }
}
}
