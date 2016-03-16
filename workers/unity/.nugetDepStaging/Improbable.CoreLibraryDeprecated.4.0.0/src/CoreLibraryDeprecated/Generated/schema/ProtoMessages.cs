//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: improbable/entity/bot/bot_parameters.proto
// Note: requires additional types generated from: improbable/entity_state.proto
namespace Schema.Improbable.Entity.Bot
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BotParametersData")]
  public partial class BotParametersData : global::ProtoBuf.IExtensible
  {
    public BotParametersData() {}
    

    private float? _targetSpeed;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"target_speed", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float TargetSpeed
    {
      get { return _targetSpeed?? default(float); }
      set { _targetSpeed = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool TargetSpeedSpecified
    {
      get { return _targetSpeed != null; }
      set { if (value == (_targetSpeed== null)) _targetSpeed = value ? TargetSpeed : (float?)null; }
    }
    private bool ShouldSerializeTargetSpeed() { return TargetSpeedSpecified; }
    private void ResetTargetSpeed() { TargetSpeedSpecified = false; }
    

    private float? _turnRate;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"turn_rate", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float TurnRate
    {
      get { return _turnRate?? default(float); }
      set { _turnRate = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool TurnRateSpecified
    {
      get { return _turnRate != null; }
      set { if (value == (_turnRate== null)) _turnRate = value ? TurnRate : (float?)null; }
    }
    private bool ShouldSerializeTurnRate() { return TurnRateSpecified; }
    private void ResetTurnRate() { TurnRateSpecified = false; }
    

    private float? _power;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"power", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float Power
    {
      get { return _power?? default(float); }
      set { _power = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool PowerSpecified
    {
      get { return _power != null; }
      set { if (value == (_power== null)) _power = value ? Power : (float?)null; }
    }
    private bool ShouldSerializePower() { return PowerSpecified; }
    private void ResetPower() { PowerSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
// Generated from: improbable/entity/bot/bot_target_velocity.proto
// Note: requires additional types generated from: improbable/entity_state.proto
// Note: requires additional types generated from: improbable/math/vector3d.proto
namespace Schema.Improbable.Entity.Bot
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BotTargetVelocityData")]
  public partial class BotTargetVelocityData : global::ProtoBuf.IExtensible
  {
    public BotTargetVelocityData() {}
    

    private Schema.Improbable.Math.Vector3d _targetVelocity = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"target_velocity", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Schema.Improbable.Math.Vector3d TargetVelocity
    {
      get { return _targetVelocity; }
      set { _targetVelocity = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
// Generated from: improbable/game/g3/entity/player/player_orientation_controller_data.proto
// Note: requires additional types generated from: improbable/entity_state.proto
namespace Schema.Improbable.Game.G3.Entity.Player
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PlayerOrientationControllerDataData")]
  public partial class PlayerOrientationControllerDataData : global::ProtoBuf.IExtensible
  {
    public PlayerOrientationControllerDataData() {}
    

    private float? _azimuth;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"azimuth", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float Azimuth
    {
      get { return _azimuth?? default(float); }
      set { _azimuth = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool AzimuthSpecified
    {
      get { return _azimuth != null; }
      set { if (value == (_azimuth== null)) _azimuth = value ? Azimuth : (float?)null; }
    }
    private bool ShouldSerializeAzimuth() { return AzimuthSpecified; }
    private void ResetAzimuth() { AzimuthSpecified = false; }
    

    private float? _pitch;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"pitch", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public float Pitch
    {
      get { return _pitch?? default(float); }
      set { _pitch = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool PitchSpecified
    {
      get { return _pitch != null; }
      set { if (value == (_pitch== null)) _pitch = value ? Pitch : (float?)null; }
    }
    private bool ShouldSerializePitch() { return PitchSpecified; }
    private void ResetPitch() { PitchSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}