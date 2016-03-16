// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.bot.BotTargetVelocityData in improbable/entity/bot/bot_target_velocity.proto.

namespace Improbable.Entity.Bot
{
public struct BotTargetVelocityData : global::System.IEquatable<BotTargetVelocityData>
{
    public readonly Improbable.Math.Vector3d TargetVelocity;

    public BotTargetVelocityData (Improbable.Math.Vector3d targetVelocity)
    {
        TargetVelocity = targetVelocity;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is BotTargetVelocityData))
            return false;
        return Equals((BotTargetVelocityData) obj);
    }

    public static bool operator ==(BotTargetVelocityData obj1, BotTargetVelocityData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(BotTargetVelocityData obj1, BotTargetVelocityData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(BotTargetVelocityData obj)
    {
        return true
            && TargetVelocity.Equals(obj.TargetVelocity);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + TargetVelocity.GetHashCode();
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class BotTargetVelocityDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Entity.Bot.BotTargetVelocityData, Schema.Improbable.Entity.Bot.BotTargetVelocityData>
{
    static readonly BotTargetVelocityDataHelper _instance = new BotTargetVelocityDataHelper();
    public static BotTargetVelocityDataHelper Instance { get { return _instance; } }
    private BotTargetVelocityDataHelper() {}

    public Schema.Improbable.Entity.Bot.BotTargetVelocityData ToProto(Improbable.Entity.Bot.BotTargetVelocityData data)
    {
        var proto = new Schema.Improbable.Entity.Bot.BotTargetVelocityData();
        proto.TargetVelocity = Improbable.Math.Vector3dHelper.Instance.ToProto(data.TargetVelocity);
        return proto;
    }

    //Shallow merge method
    public Improbable.Entity.Bot.BotTargetVelocityData MergeFromProto(Schema.Improbable.Entity.Bot.BotTargetVelocityData proto, bool[] statesToClear, Improbable.Entity.Bot.BotTargetVelocityData data)
    {
        return new Improbable.Entity.Bot.BotTargetVelocityData(
            proto.TargetVelocity != null ? Improbable.Math.Vector3dHelper.Instance.FromProto(proto.TargetVelocity) : data.TargetVelocity
        );
    }

    public Improbable.Entity.Bot.BotTargetVelocityData FromProto(Schema.Improbable.Entity.Bot.BotTargetVelocityData proto)
    {
        return new Improbable.Entity.Bot.BotTargetVelocityData(
            Improbable.Math.Vector3dHelper.Instance.FromProto(proto.TargetVelocity)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Entity.Bot.BotTargetVelocityData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Entity.Bot.BotTargetVelocityData protoTo, bool[] statesToClearTo)
    {
        if (protoFrom.TargetVelocity != null)
        {
            protoTo.TargetVelocity = protoFrom.TargetVelocity;
        }
    }
}
}
