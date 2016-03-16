// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.slots.DynamicSlotsData in improbable/corelib/slots/dynamic_slots.proto.

namespace Improbable.Corelib.Slots
{
public struct DynamicSlotsData : global::System.IEquatable<DynamicSlotsData>
{
    public readonly global::Improbable.Util.Collections.IReadOnlyDictionary<string, Improbable.Corelib.Math.RelativeTransform> Slots;

    public DynamicSlotsData (global::Improbable.Util.Collections.IReadOnlyDictionary<string, Improbable.Corelib.Math.RelativeTransform> slots)
    {
        Slots = slots;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is DynamicSlotsData))
            return false;
        return Equals((DynamicSlotsData) obj);
    }

    public static bool operator ==(DynamicSlotsData obj1, DynamicSlotsData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(DynamicSlotsData obj1, DynamicSlotsData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(DynamicSlotsData obj)
    {
        return true
            && global::Improbable.Util.Collections.CollectionUtil.DictionariesEqual(Slots, obj.Slots);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + (Slots != null ? Slots.GetHashCode() : 0);
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class DynamicSlotsDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Corelib.Slots.DynamicSlotsData, Schema.Improbable.Corelib.Slots.DynamicSlotsData>
{
    static readonly DynamicSlotsDataHelper _instance = new DynamicSlotsDataHelper();
    public static DynamicSlotsDataHelper Instance { get { return _instance; } }
    private DynamicSlotsDataHelper() {}

    public Schema.Improbable.Corelib.Slots.DynamicSlotsData ToProto(Improbable.Corelib.Slots.DynamicSlotsData data)
    {
        var proto = new Schema.Improbable.Corelib.Slots.DynamicSlotsData();
        global::Improbable.Tools.ToProto(data.Slots, proto.Slots, Improbable.Corelib.Slots.DynamicSlotsDataHelper.SlotsEntryHelper.Instance);
        return proto;
    }

    //Shallow merge method
    public Improbable.Corelib.Slots.DynamicSlotsData MergeFromProto(Schema.Improbable.Corelib.Slots.DynamicSlotsData proto, bool[] statesToClear, Improbable.Corelib.Slots.DynamicSlotsData data)
    {
        return new Improbable.Corelib.Slots.DynamicSlotsData(
            (proto.Slots.Count > 0 || statesToClear != null && statesToClear[0]) ? global::Improbable.Tools.FromProto(proto.Slots, Improbable.Corelib.Slots.DynamicSlotsDataHelper.SlotsEntryHelper.Instance) : data.Slots
        );
    }

    public Improbable.Corelib.Slots.DynamicSlotsData FromProto(Schema.Improbable.Corelib.Slots.DynamicSlotsData proto)
    {
        return new Improbable.Corelib.Slots.DynamicSlotsData(
            global::Improbable.Tools.FromProto(proto.Slots, Improbable.Corelib.Slots.DynamicSlotsDataHelper.SlotsEntryHelper.Instance)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Corelib.Slots.DynamicSlotsData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Corelib.Slots.DynamicSlotsData protoTo, bool[] statesToClearTo)
    {
        if ((protoFrom.Slots.Count > 0 || statesToClearFrom != null && statesToClearFrom[0]))
        {
            statesToClearTo[0] = statesToClearFrom[0];
            protoTo.Slots.Clear();
            protoTo.Slots.AddRange(protoFrom.Slots);
        }
    }

    //For internal use only, not to be used by user code
    public sealed class SlotsEntryHelper : global::Improbable.Tools.IProtoKeyValueConverter<string, Improbable.Corelib.Math.RelativeTransform, Schema.Improbable.Corelib.Slots.DynamicSlotsData.SlotsEntry>
    {
        static readonly SlotsEntryHelper _instance = new SlotsEntryHelper();
        public static SlotsEntryHelper Instance { get { return _instance; } }
        private SlotsEntryHelper() {}

        public Schema.Improbable.Corelib.Slots.DynamicSlotsData.SlotsEntry ToProto(System.Collections.Generic.KeyValuePair<string, Improbable.Corelib.Math.RelativeTransform> keyValue)
        {
            var proto = new Schema.Improbable.Corelib.Slots.DynamicSlotsData.SlotsEntry();
            proto.Key = keyValue.Key;
            proto.Value = Improbable.Corelib.Math.RelativeTransformHelper.Instance.ToProto(keyValue.Value);
            return proto;
        }

        public global::System.Collections.Generic.KeyValuePair<string, Improbable.Corelib.Math.RelativeTransform> FromProto(Schema.Improbable.Corelib.Slots.DynamicSlotsData.SlotsEntry proto)
        {
            return new global::System.Collections.Generic.KeyValuePair<string, Improbable.Corelib.Math.RelativeTransform>(proto.Key, Improbable.Corelib.Math.RelativeTransformHelper.Instance.FromProto(proto.Value));
        }
    }
}
}
