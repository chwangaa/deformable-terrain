package improbable.entitytemplates

import improbable.corelib.entity.nature.definitions.{CoreLibPhysicalObject, CoreLibBotObject}
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.math._
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.NewMoveRandomlyBehaviour

object DemoBot extends NatureDescription {
  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(NewMoveRandomlyBehaviour())
  }

  override val dependencies = Set[NatureDescription](CoreLibPhysicalObject)

  def apply(position: Vector3d): NatureApplication = {
    application(
      states = Seq(),
      natures = Seq(CoreLibPhysicalObject(position, drag = 0.2f))
    )
  }
}