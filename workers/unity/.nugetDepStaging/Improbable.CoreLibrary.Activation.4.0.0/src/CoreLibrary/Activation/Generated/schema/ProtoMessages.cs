//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: improbable/corelib/worker/checkout/entity_loading_control.proto
// Note: requires additional types generated from: improbable/entity_state.proto
namespace Schema.Improbable.Corelib.Worker.Checkout
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EntityLoadingControlData")]
  public partial class EntityLoadingControlData : global::ProtoBuf.IExtensible
  {
    public EntityLoadingControlData() {}
    

    private Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates? _loadedState;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"loaded_state", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates LoadedState
    {
      get { return _loadedState?? Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates.Requested; }
      set { _loadedState = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool LoadedStateSpecified
    {
      get { return _loadedState != null; }
      set { if (value == (_loadedState== null)) _loadedState = value ? LoadedState : (Schema.Improbable.Corelib.Worker.Checkout.EntityLoadingControlData.EntityLoadingStates?)null; }
    }
    private bool ShouldSerializeLoadedState() { return LoadedStateSpecified; }
    private void ResetLoadedState() { LoadedStateSpecified = false; }
    

    private int? _tries;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"tries", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Tries
    {
      get { return _tries?? default(int); }
      set { _tries = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool TriesSpecified
    {
      get { return _tries != null; }
      set { if (value == (_tries== null)) _tries = value ? Tries : (int?)null; }
    }
    private bool ShouldSerializeTries() { return TriesSpecified; }
    private void ResetTries() { TriesSpecified = false; }
    

    private int? _maxTries;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"max_tries", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int MaxTries
    {
      get { return _maxTries?? default(int); }
      set { _maxTries = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool MaxTriesSpecified
    {
      get { return _maxTries != null; }
      set { if (value == (_maxTries== null)) _maxTries = value ? MaxTries : (int?)null; }
    }
    private bool ShouldSerializeMaxTries() { return MaxTriesSpecified; }
    private void ResetMaxTries() { MaxTriesSpecified = false; }
    

    private int? _retryWait;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"retry_wait", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int RetryWait
    {
      get { return _retryWait?? default(int); }
      set { _retryWait = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool RetryWaitSpecified
    {
      get { return _retryWait != null; }
      set { if (value == (_retryWait== null)) _retryWait = value ? RetryWait : (int?)null; }
    }
    private bool ShouldSerializeRetryWait() { return RetryWaitSpecified; }
    private void ResetRetryWait() { RetryWaitSpecified = false; }
    

    private bool? _triggerCallbackOnTimeout;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"trigger_callback_on_timeout", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool TriggerCallbackOnTimeout
    {
      get { return _triggerCallbackOnTimeout?? default(bool); }
      set { _triggerCallbackOnTimeout = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool TriggerCallbackOnTimeoutSpecified
    {
      get { return _triggerCallbackOnTimeout != null; }
      set { if (value == (_triggerCallbackOnTimeout== null)) _triggerCallbackOnTimeout = value ? TriggerCallbackOnTimeout : (bool?)null; }
    }
    private bool ShouldSerializeTriggerCallbackOnTimeout() { return TriggerCallbackOnTimeoutSpecified; }
    private void ResetTriggerCallbackOnTimeout() { TriggerCallbackOnTimeoutSpecified = false; }
    
    private readonly global::System.Collections.Generic.List<long> _entities = new global::System.Collections.Generic.List<long>();
    [global::ProtoBuf.ProtoMember(6, Name=@"entities", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<long> Entities
    {
      get { return _entities; }
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EntityLoadingStates")]
    public enum EntityLoadingStates
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Requested", Value=0)]
      Requested = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Loaded", Value=1)]
      Loaded = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Error", Value=2)]
      Error = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Idle", Value=3)]
      Idle = 3
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
// Generated from: improbable/corelib/worker/checkout/entity_loading_response.proto
// Note: requires additional types generated from: improbable/entity_state.proto
namespace Schema.Improbable.Corelib.Worker.Checkout
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EntityLoadingResponseData")]
  public partial class EntityLoadingResponseData : global::ProtoBuf.IExtensible
  {
    public EntityLoadingResponseData() {}
    

    private bool? _loaded;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"loaded", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool Loaded
    {
      get { return _loaded?? default(bool); }
      set { _loaded = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool LoadedSpecified
    {
      get { return _loaded != null; }
      set { if (value == (_loaded== null)) _loaded = value ? Loaded : (bool?)null; }
    }
    private bool ShouldSerializeLoaded() { return LoadedSpecified; }
    private void ResetLoaded() { LoadedSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}
// Generated from: improbable/corelibrary/activation/activated.proto
// Note: requires additional types generated from: improbable/entity_state.proto
namespace Schema.Improbable.Corelibrary.Activation
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ActivatedData")]
  public partial class ActivatedData : global::ProtoBuf.IExtensible
  {
    public ActivatedData() {}
    

    private bool? _isActive;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"is_active", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool IsActive
    {
      get { return _isActive?? default(bool); }
      set { _isActive = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool IsActiveSpecified
    {
      get { return _isActive != null; }
      set { if (value == (_isActive== null)) _isActive = value ? IsActive : (bool?)null; }
    }
    private bool ShouldSerializeIsActive() { return IsActiveSpecified; }
    private void ResetIsActive() { IsActiveSpecified = false; }
    

    private bool? _activateRequested;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"activate_requested", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool ActivateRequested
    {
      get { return _activateRequested?? default(bool); }
      set { _activateRequested = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool ActivateRequestedSpecified
    {
      get { return _activateRequested != null; }
      set { if (value == (_activateRequested== null)) _activateRequested = value ? ActivateRequested : (bool?)null; }
    }
    private bool ShouldSerializeActivateRequested() { return ActivateRequestedSpecified; }
    private void ResetActivateRequested() { ActivateRequestedSpecified = false; }
    

    private int? _activationCallbacksPending;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"activation_callbacks_pending", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int ActivationCallbacksPending
    {
      get { return _activationCallbacksPending?? default(int); }
      set { _activationCallbacksPending = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool ActivationCallbacksPendingSpecified
    {
      get { return _activationCallbacksPending != null; }
      set { if (value == (_activationCallbacksPending== null)) _activationCallbacksPending = value ? ActivationCallbacksPending : (int?)null; }
    }
    private bool ShouldSerializeActivationCallbacksPending() { return ActivationCallbacksPendingSpecified; }
    private void ResetActivationCallbacksPending() { ActivationCallbacksPendingSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}