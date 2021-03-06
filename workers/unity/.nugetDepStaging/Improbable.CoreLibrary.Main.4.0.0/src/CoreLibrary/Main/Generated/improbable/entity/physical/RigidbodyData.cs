// Generated by ProtocGenFabric. DO NOT EDIT!
// source: schema.improbable.entity.physical.RigidbodyDataData in improbable/entity/physical/rigidbody_data.proto.

using System;
using Improbable.Core.Serialization;
using Improbable.Entity.State;

namespace Improbable.Entity.Physical
{
[ReaderInterface]
[CanonicalName("improbable.entity.physical.RigidbodyData")]
public interface RigidbodyDataReader : IEntityStateReader
{
    float Mass { get; }
    Improbable.Math.Vector3d Force { get; }
    Improbable.Math.Vector3d Torque { get; }
    float Drag { get; }
    float AngularDrag { get; }
    Improbable.Entity.Physical.FreezeConstraints FreezePosition { get; }
    Improbable.Entity.Physical.FreezeConstraints FreezeRotation { get; }
    bool UseGravity { get; }
    bool IsKinematic { get; }
    Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode Interpolation { get; }
    Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode CollisionDetection { get; }

    event System.Action<float> MassUpdated;
    event System.Action<Improbable.Math.Vector3d> ForceUpdated;
    event System.Action<Improbable.Math.Vector3d> TorqueUpdated;
    event System.Action<float> DragUpdated;
    event System.Action<float> AngularDragUpdated;
    event System.Action<Improbable.Entity.Physical.FreezeConstraints> FreezePositionUpdated;
    event System.Action<Improbable.Entity.Physical.FreezeConstraints> FreezeRotationUpdated;
    event System.Action<bool> UseGravityUpdated;
    event System.Action<bool> IsKinematicUpdated;
    event System.Action<Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode> InterpolationUpdated;
    event System.Action<Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode> CollisionDetectionUpdated;
    event System.Action<Improbable.Entity.Physical.Impulse> Impulse;
    event System.Action<Improbable.Entity.Physical.SetVelocity> SetVelocity;
}

public interface IRigidbodyDataUpdater : IEntityStateUpdater
{
    void FinishAndSend();
    IRigidbodyDataUpdater Mass(float newValue);
    IRigidbodyDataUpdater Force(Improbable.Math.Vector3d newValue);
    IRigidbodyDataUpdater Torque(Improbable.Math.Vector3d newValue);
    IRigidbodyDataUpdater Drag(float newValue);
    IRigidbodyDataUpdater AngularDrag(float newValue);
    IRigidbodyDataUpdater FreezePosition(Improbable.Entity.Physical.FreezeConstraints newValue);
    IRigidbodyDataUpdater FreezeRotation(Improbable.Entity.Physical.FreezeConstraints newValue);
    IRigidbodyDataUpdater UseGravity(bool newValue);
    IRigidbodyDataUpdater IsKinematic(bool newValue);
    IRigidbodyDataUpdater Interpolation(Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode newValue);
    IRigidbodyDataUpdater CollisionDetection(Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode newValue);
    IRigidbodyDataUpdater TriggerImpulse(
        Improbable.Math.Vector3d value);
    IRigidbodyDataUpdater TriggerSetVelocity(
        Improbable.Math.Vector3d value);
}

[WriterInterface]
[CanonicalName("improbable.entity.physical.RigidbodyData")]
public interface RigidbodyDataWriter : RigidbodyDataReader, IUpdateable<IRigidbodyDataUpdater> { }

public class RigidbodyData : global::Improbable.Entity.State.StateBase<Improbable.Entity.Physical.RigidbodyDataData, Schema.Improbable.Entity.Physical.RigidbodyDataData>, RigidbodyDataWriter, IRigidbodyDataUpdater
{
    public RigidbodyData(global::Improbable.EntityId entityId, Improbable.Entity.Physical.RigidbodyDataData data, IStateSender sender)
        : base(entityId, data, sender, Improbable.Entity.Physical.RigidbodyDataDataHelper.Instance) { }
    private static log4net.ILog LOGGER = log4net.LogManager.GetLogger(typeof(RigidbodyData));
    private static bool ShouldLogFinishAndSendNoUpdate = true;
    private static bool ShouldLogUpdateNoFinishAndSend = true;

    protected override void LogFinishAndSendWithNoUpdate() {
        if (ShouldLogFinishAndSendNoUpdate)
        {
            ShouldLogFinishAndSendNoUpdate = false;
            LOGGER.ErrorFormat("Finish and send was called with no update in flight for entity {0}. " +
                               "This is probably due to having more StateUpdates in flight, which is an error. (Logged only once.)", EntityId);
        }
    }

    public float Mass { get { return Data.Mass; } }
    public Improbable.Math.Vector3d Force { get { return Data.Force; } }
    public Improbable.Math.Vector3d Torque { get { return Data.Torque; } }
    public float Drag { get { return Data.Drag; } }
    public float AngularDrag { get { return Data.AngularDrag; } }
    public Improbable.Entity.Physical.FreezeConstraints FreezePosition { get { return Data.FreezePosition; } }
    public Improbable.Entity.Physical.FreezeConstraints FreezeRotation { get { return Data.FreezeRotation; } }
    public bool UseGravity { get { return Data.UseGravity; } }
    public bool IsKinematic { get { return Data.IsKinematic; } }
    public Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode Interpolation { get { return Data.Interpolation; } }
    public Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode CollisionDetection { get { return Data.CollisionDetection; } }

    private readonly global::System.Collections.Generic.List<System.Action<float>> updatedCallbacksMass =
        new global::System.Collections.Generic.List<System.Action<float>>();
    public event System.Action<float> MassUpdated
    {
        add
        {
            updatedCallbacksMass.Add(value);
            value(Data.Mass);
        }
        remove { updatedCallbacksMass.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Math.Vector3d>> updatedCallbacksForce =
        new global::System.Collections.Generic.List<System.Action<Improbable.Math.Vector3d>>();
    public event System.Action<Improbable.Math.Vector3d> ForceUpdated
    {
        add
        {
            updatedCallbacksForce.Add(value);
            value(Data.Force);
        }
        remove { updatedCallbacksForce.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Math.Vector3d>> updatedCallbacksTorque =
        new global::System.Collections.Generic.List<System.Action<Improbable.Math.Vector3d>>();
    public event System.Action<Improbable.Math.Vector3d> TorqueUpdated
    {
        add
        {
            updatedCallbacksTorque.Add(value);
            value(Data.Torque);
        }
        remove { updatedCallbacksTorque.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<float>> updatedCallbacksDrag =
        new global::System.Collections.Generic.List<System.Action<float>>();
    public event System.Action<float> DragUpdated
    {
        add
        {
            updatedCallbacksDrag.Add(value);
            value(Data.Drag);
        }
        remove { updatedCallbacksDrag.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<float>> updatedCallbacksAngularDrag =
        new global::System.Collections.Generic.List<System.Action<float>>();
    public event System.Action<float> AngularDragUpdated
    {
        add
        {
            updatedCallbacksAngularDrag.Add(value);
            value(Data.AngularDrag);
        }
        remove { updatedCallbacksAngularDrag.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.FreezeConstraints>> updatedCallbacksFreezePosition =
        new global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.FreezeConstraints>>();
    public event System.Action<Improbable.Entity.Physical.FreezeConstraints> FreezePositionUpdated
    {
        add
        {
            updatedCallbacksFreezePosition.Add(value);
            value(Data.FreezePosition);
        }
        remove { updatedCallbacksFreezePosition.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.FreezeConstraints>> updatedCallbacksFreezeRotation =
        new global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.FreezeConstraints>>();
    public event System.Action<Improbable.Entity.Physical.FreezeConstraints> FreezeRotationUpdated
    {
        add
        {
            updatedCallbacksFreezeRotation.Add(value);
            value(Data.FreezeRotation);
        }
        remove { updatedCallbacksFreezeRotation.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<bool>> updatedCallbacksUseGravity =
        new global::System.Collections.Generic.List<System.Action<bool>>();
    public event System.Action<bool> UseGravityUpdated
    {
        add
        {
            updatedCallbacksUseGravity.Add(value);
            value(Data.UseGravity);
        }
        remove { updatedCallbacksUseGravity.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<bool>> updatedCallbacksIsKinematic =
        new global::System.Collections.Generic.List<System.Action<bool>>();
    public event System.Action<bool> IsKinematicUpdated
    {
        add
        {
            updatedCallbacksIsKinematic.Add(value);
            value(Data.IsKinematic);
        }
        remove { updatedCallbacksIsKinematic.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode>> updatedCallbacksInterpolation =
        new global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode>>();
    public event System.Action<Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode> InterpolationUpdated
    {
        add
        {
            updatedCallbacksInterpolation.Add(value);
            value(Data.Interpolation);
        }
        remove { updatedCallbacksInterpolation.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode>> updatedCallbacksCollisionDetection =
        new global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode>>();
    public event System.Action<Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode> CollisionDetectionUpdated
    {
        add
        {
            updatedCallbacksCollisionDetection.Add(value);
            value(Data.CollisionDetection);
        }
        remove { updatedCallbacksCollisionDetection.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.Impulse>> eventCallbacksImpulse =
        new global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.Impulse>>();
    public event System.Action<Improbable.Entity.Physical.Impulse> Impulse
    {
        add { eventCallbacksImpulse.Add(value); }
        remove { eventCallbacksImpulse.Remove(value); }
    }

    private readonly global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.SetVelocity>> eventCallbacksSetVelocity =
        new global::System.Collections.Generic.List<System.Action<Improbable.Entity.Physical.SetVelocity>>();
    public event System.Action<Improbable.Entity.Physical.SetVelocity> SetVelocity
    {
        add { eventCallbacksSetVelocity.Add(value); }
        remove { eventCallbacksSetVelocity.Remove(value); }
    }

    override protected void UnsubscribeEventHandlersInternal(object visualizer)
    {
        UnsubscribeEventHandler(visualizer, updatedCallbacksMass);
        UnsubscribeEventHandler(visualizer, updatedCallbacksForce);
        UnsubscribeEventHandler(visualizer, updatedCallbacksTorque);
        UnsubscribeEventHandler(visualizer, updatedCallbacksDrag);
        UnsubscribeEventHandler(visualizer, updatedCallbacksAngularDrag);
        UnsubscribeEventHandler(visualizer, updatedCallbacksFreezePosition);
        UnsubscribeEventHandler(visualizer, updatedCallbacksFreezeRotation);
        UnsubscribeEventHandler(visualizer, updatedCallbacksUseGravity);
        UnsubscribeEventHandler(visualizer, updatedCallbacksIsKinematic);
        UnsubscribeEventHandler(visualizer, updatedCallbacksInterpolation);
        UnsubscribeEventHandler(visualizer, updatedCallbacksCollisionDetection);
        UnsubscribeEventHandler(visualizer, eventCallbacksImpulse);
        UnsubscribeEventHandler(visualizer, eventCallbacksSetVelocity);
    }

    public IRigidbodyDataUpdater Update
    {
        get
        {
            if (Updating)
            {
                if (ShouldLogUpdateNoFinishAndSend)
                {
                    ShouldLogUpdateNoFinishAndSend = false;
                    LOGGER.ErrorFormat("Multiple state updates of entity {0} are in flight, which has undefined semantics. " +
                        "Each call to Update has to be followed by a FinishAndSend() before another call is made on the same state. (Logged only once.)", EntityId);
                }
            }
            else
            {
                Updating = true;
                Updater = new RigidbodyDataUpdate(EntityId, new bool[0], new Schema.Improbable.Entity.Physical.RigidbodyDataData());
            }
            return this;
        }
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.Mass(float newValue)
    {
        if (Updater.Proto.MassSpecified || !Mass.Equals(newValue))
        {
            Updater.Proto.Mass = newValue;
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.Force(Improbable.Math.Vector3d newValue)
    {
        if (Updater.Proto.Force != null || !Force.Equals(newValue))
        {
            Updater.Proto.Force = Improbable.Math.Vector3dHelper.Instance.ToProto(newValue);
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.Torque(Improbable.Math.Vector3d newValue)
    {
        if (Updater.Proto.Torque != null || !Torque.Equals(newValue))
        {
            Updater.Proto.Torque = Improbable.Math.Vector3dHelper.Instance.ToProto(newValue);
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.Drag(float newValue)
    {
        if (Updater.Proto.DragSpecified || !Drag.Equals(newValue))
        {
            Updater.Proto.Drag = newValue;
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.AngularDrag(float newValue)
    {
        if (Updater.Proto.AngularDragSpecified || !AngularDrag.Equals(newValue))
        {
            Updater.Proto.AngularDrag = newValue;
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.FreezePosition(Improbable.Entity.Physical.FreezeConstraints newValue)
    {
        if (Updater.Proto.FreezePosition != null || !FreezePosition.Equals(newValue))
        {
            Updater.Proto.FreezePosition = Improbable.Entity.Physical.FreezeConstraintsHelper.Instance.ToProto(newValue);
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.FreezeRotation(Improbable.Entity.Physical.FreezeConstraints newValue)
    {
        if (Updater.Proto.FreezeRotation != null || !FreezeRotation.Equals(newValue))
        {
            Updater.Proto.FreezeRotation = Improbable.Entity.Physical.FreezeConstraintsHelper.Instance.ToProto(newValue);
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.UseGravity(bool newValue)
    {
        if (Updater.Proto.UseGravitySpecified || !UseGravity.Equals(newValue))
        {
            Updater.Proto.UseGravity = newValue;
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.IsKinematic(bool newValue)
    {
        if (Updater.Proto.IsKinematicSpecified || !IsKinematic.Equals(newValue))
        {
            Updater.Proto.IsKinematic = newValue;
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.Interpolation(Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode newValue)
    {
        if (Updater.Proto.InterpolationSpecified || !Interpolation.Equals(newValue))
        {
            Updater.Proto.Interpolation = (Schema.Improbable.Entity.Physical.RigidbodyDataData.InterpolationMode) newValue;
        }
        return this;
    }

    IRigidbodyDataUpdater IRigidbodyDataUpdater.CollisionDetection(Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode newValue)
    {
        if (Updater.Proto.CollisionDetectionSpecified || !CollisionDetection.Equals(newValue))
        {
            Updater.Proto.CollisionDetection = (Schema.Improbable.Entity.Physical.RigidbodyDataData.CollisionDetectionMode) newValue;
        }
        return this;
    }

    global::System.Collections.Generic.IList<Improbable.Entity.Physical.Impulse> triggeredImpulse = new global::System.Collections.Generic.List<Improbable.Entity.Physical.Impulse>();
    IRigidbodyDataUpdater IRigidbodyDataUpdater.TriggerImpulse(
        Improbable.Math.Vector3d value)
    {
        var eventData = new Improbable.Entity.Physical.Impulse(
            value);
        triggeredImpulse.Add(eventData);
        Updater.Proto.Impulse.Add(Improbable.Entity.Physical.ImpulseHelper.Instance.ToProto(eventData));
        return this;
    }

    global::System.Collections.Generic.IList<Improbable.Entity.Physical.SetVelocity> triggeredSetVelocity = new global::System.Collections.Generic.List<Improbable.Entity.Physical.SetVelocity>();
    IRigidbodyDataUpdater IRigidbodyDataUpdater.TriggerSetVelocity(
        Improbable.Math.Vector3d value)
    {
        var eventData = new Improbable.Entity.Physical.SetVelocity(
            value);
        triggeredSetVelocity.Add(eventData);
        Updater.Proto.SetVelocity.Add(Improbable.Entity.Physical.SetVelocityHelper.Instance.ToProto(eventData));
        return this;
    }

    override protected bool TriggerUpdatedEvents(Schema.Improbable.Entity.Physical.RigidbodyDataData update, bool[] statesToClear)
    {
        bool anythingUpdated = false;
        bool updatedMass = update.MassSpecified;
        anythingUpdated |= updatedMass;
        if (updatedMass) TriggerCallbacks(updatedCallbacksMass, Data.Mass);

        bool updatedForce = update.Force != null;
        anythingUpdated |= updatedForce;
        if (updatedForce) TriggerCallbacks(updatedCallbacksForce, Data.Force);

        bool updatedTorque = update.Torque != null;
        anythingUpdated |= updatedTorque;
        if (updatedTorque) TriggerCallbacks(updatedCallbacksTorque, Data.Torque);

        bool updatedDrag = update.DragSpecified;
        anythingUpdated |= updatedDrag;
        if (updatedDrag) TriggerCallbacks(updatedCallbacksDrag, Data.Drag);

        bool updatedAngularDrag = update.AngularDragSpecified;
        anythingUpdated |= updatedAngularDrag;
        if (updatedAngularDrag) TriggerCallbacks(updatedCallbacksAngularDrag, Data.AngularDrag);

        bool updatedFreezePosition = update.FreezePosition != null;
        anythingUpdated |= updatedFreezePosition;
        if (updatedFreezePosition) TriggerCallbacks(updatedCallbacksFreezePosition, Data.FreezePosition);

        bool updatedFreezeRotation = update.FreezeRotation != null;
        anythingUpdated |= updatedFreezeRotation;
        if (updatedFreezeRotation) TriggerCallbacks(updatedCallbacksFreezeRotation, Data.FreezeRotation);

        bool updatedUseGravity = update.UseGravitySpecified;
        anythingUpdated |= updatedUseGravity;
        if (updatedUseGravity) TriggerCallbacks(updatedCallbacksUseGravity, Data.UseGravity);

        bool updatedIsKinematic = update.IsKinematicSpecified;
        anythingUpdated |= updatedIsKinematic;
        if (updatedIsKinematic) TriggerCallbacks(updatedCallbacksIsKinematic, Data.IsKinematic);

        bool updatedInterpolation = update.InterpolationSpecified;
        anythingUpdated |= updatedInterpolation;
        if (updatedInterpolation) TriggerCallbacks(updatedCallbacksInterpolation, Data.Interpolation);

        bool updatedCollisionDetection = update.CollisionDetectionSpecified;
        anythingUpdated |= updatedCollisionDetection;
        if (updatedCollisionDetection) TriggerCallbacks(updatedCallbacksCollisionDetection, Data.CollisionDetection);

        if (anythingUpdated) TriggerPropertyUpdated();
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents(Schema.Improbable.Entity.Physical.RigidbodyDataData stateUpdate)
    {
        bool anythingUpdated = false;
        bool updatedImpulse = stateUpdate.Impulse.Count > 0;
        TriggerEventCallbacks(eventCallbacksImpulse, stateUpdate.Impulse, Improbable.Entity.Physical.ImpulseHelper.Instance);
        anythingUpdated |= updatedImpulse;
        bool updatedSetVelocity = stateUpdate.SetVelocity.Count > 0;
        TriggerEventCallbacks(eventCallbacksSetVelocity, stateUpdate.SetVelocity, Improbable.Entity.Physical.SetVelocityHelper.Instance);
        anythingUpdated |= updatedSetVelocity;
        return anythingUpdated;
    }

    override protected bool TriggerAllStateEvents()
    {
        bool anythingUpdated = false;
        bool updatedImpulse = triggeredImpulse.Count > 0;
        TriggerEventCallbacks(eventCallbacksImpulse, triggeredImpulse);
        if (triggeredImpulse != null) triggeredImpulse.Clear();
        anythingUpdated |= updatedImpulse;
        bool updatedSetVelocity = triggeredSetVelocity.Count > 0;
        TriggerEventCallbacks(eventCallbacksSetVelocity, triggeredSetVelocity);
        if (triggeredSetVelocity != null) triggeredSetVelocity.Clear();
        anythingUpdated |= updatedSetVelocity;
        return anythingUpdated;
    }
}

public class RigidbodyDataUpdate : global::Improbable.Entity.State.StateUpdate<Improbable.Entity.Physical.RigidbodyDataData, Schema.Improbable.Entity.Physical.RigidbodyDataData>
{
    public const int STATE_UPDATE_FIELD_ID = 190122;
    public RigidbodyDataUpdate(global::Improbable.EntityId entityId, bool[] statesToClear, Schema.Improbable.Entity.Physical.RigidbodyDataData proto)
        : base(entityId, statesToClear, Improbable.Entity.Physical.RigidbodyDataDataHelper.Instance, proto, STATE_UPDATE_FIELD_ID) { }

    public override IReadWriteEntityState CreateState(global::Improbable.EntityId entityId, IStateSender stateSender)
    {
        return new RigidbodyData(entityId, GetData(), stateSender);
    }

    public static RigidbodyDataUpdate ExtractFrom(global::Improbable.Protocol.StateUpdate proto)
    {
        var protoState = ProtoBuf.Extensible.GetValue<Schema.Improbable.Entity.Physical.RigidbodyDataData>(proto.EntityState, STATE_UPDATE_FIELD_ID);
        return new RigidbodyDataUpdate(global::Improbable.EntityIdHelper.Instance.FromProto(proto.EntityId), null, protoState);
    }

    override protected int SeqToId(int seqId) { return seqToId[seqId]; }
    private static int[] seqToId = {};
}
}
