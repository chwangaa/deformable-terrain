// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.corelib.slots.SlotsData in improbable/corelib/slots/slots.proto.

namespace Improbable.Corelib.Slots
{
public struct SlotsData : global::System.IEquatable<SlotsData>
{
    public readonly global::Improbable.Util.Collections.IReadOnlyDictionary<string, Improbable.EntityId> SlotToEntityId;

    public SlotsData (global::Improbable.Util.Collections.IReadOnlyDictionary<string, Improbable.EntityId> slotToEntityId)
    {
        SlotToEntityId = slotToEntityId;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is SlotsData))
            return false;
        return Equals((SlotsData) obj);
    }

    public static bool operator ==(SlotsData obj1, SlotsData obj2)
    {
        return obj1.Equals(obj2);
    }

    public static bool operator !=(SlotsData obj1, SlotsData obj2)
    {
        return !obj1.Equals(obj2);
    }

    public bool Equals(SlotsData obj)
    {
        return true
            && global::Improbable.Util.Collections.CollectionUtil.DictionariesEqual(SlotToEntityId, obj.SlotToEntityId);
    }

    public override int GetHashCode()
    {
        int res = 1327;
        res = (res * 977) + (SlotToEntityId != null ? SlotToEntityId.GetHashCode() : 0);
        return res;
    }
}

//For internal use only, not to be used in user code.
public sealed class SlotsDataHelper : global::Improbable.Tools.IProtoConverter<Improbable.Corelib.Slots.SlotsData, Schema.Improbable.Corelib.Slots.SlotsData>
{
    static readonly SlotsDataHelper _instance = new SlotsDataHelper();
    public static SlotsDataHelper Instance { get { return _instance; } }
    private SlotsDataHelper() {}

    public Schema.Improbable.Corelib.Slots.SlotsData ToProto(Improbable.Corelib.Slots.SlotsData data)
    {
        var proto = new Schema.Improbable.Corelib.Slots.SlotsData();
        global::Improbable.Tools.ToProto(data.SlotToEntityId, proto.SlotToEntityId, Improbable.Corelib.Slots.SlotsDataHelper.SlotToEntityIdEntryHelper.Instance);
        return proto;
    }

    //Shallow merge method
    public Improbable.Corelib.Slots.SlotsData MergeFromProto(Schema.Improbable.Corelib.Slots.SlotsData proto, bool[] statesToClear, Improbable.Corelib.Slots.SlotsData data)
    {
        return new Improbable.Corelib.Slots.SlotsData(
            (proto.SlotToEntityId.Count > 0 || statesToClear != null && statesToClear[0]) ? global::Improbable.Tools.FromProto(proto.SlotToEntityId, Improbable.Corelib.Slots.SlotsDataHelper.SlotToEntityIdEntryHelper.Instance) : data.SlotToEntityId
        );
    }

    public Improbable.Corelib.Slots.SlotsData FromProto(Schema.Improbable.Corelib.Slots.SlotsData proto)
    {
        return new Improbable.Corelib.Slots.SlotsData(
            global::Improbable.Tools.FromProto(proto.SlotToEntityId, Improbable.Corelib.Slots.SlotsDataHelper.SlotToEntityIdEntryHelper.Instance)
        );
    }

    //Shallow merge method
    public void MergeProto(Schema.Improbable.Corelib.Slots.SlotsData protoFrom, bool[] statesToClearFrom, Schema.Improbable.Corelib.Slots.SlotsData protoTo, bool[] statesToClearTo)
    {
        if ((protoFrom.SlotToEntityId.Count > 0 || statesToClearFrom != null && statesToClearFrom[0]))
        {
            statesToClearTo[0] = statesToClearFrom[0];
            protoTo.SlotToEntityId.Clear();
            protoTo.SlotToEntityId.AddRange(protoFrom.SlotToEntityId);
        }
    }

    //For internal use only, not to be used by user code
    public sealed class SlotToEntityIdEntryHelper : global::Improbable.Tools.IProtoKeyValueConverter<string, Improbable.EntityId, Schema.Improbable.Corelib.Slots.SlotsData.SlotToEntityIdEntry>
    {
        static readonly SlotToEntityIdEntryHelper _instance = new SlotToEntityIdEntryHelper();
        public static SlotToEntityIdEntryHelper Instance { get { return _instance; } }
        private SlotToEntityIdEntryHelper() {}

        public Schema.Improbable.Corelib.Slots.SlotsData.SlotToEntityIdEntry ToProto(System.Collections.Generic.KeyValuePair<string, Improbable.EntityId> keyValue)
        {
            var proto = new Schema.Improbable.Corelib.Slots.SlotsData.SlotToEntityIdEntry();
            proto.Key = keyValue.Key;
            proto.Value = Improbable.EntityIdHelper.Instance.ToProto(keyValue.Value);
            return proto;
        }

        public global::System.Collections.Generic.KeyValuePair<string, Improbable.EntityId> FromProto(Schema.Improbable.Corelib.Slots.SlotsData.SlotToEntityIdEntry proto)
        {
            return new global::System.Collections.Generic.KeyValuePair<string, Improbable.EntityId>(proto.Key, Improbable.EntityIdHelper.Instance.FromProto(proto.Value));
        }
    }
}
}
