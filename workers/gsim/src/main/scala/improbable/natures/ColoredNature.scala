package improbable.natures

import improbable.behaviours.color.ColorBehaviour
import improbable.behaviours.color.ColorConversions._
import improbable.colorState.ColorState
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor

object ColoredNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set.empty

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(descriptorOf[ColorBehaviour])
  }

  def apply(color: java.awt.Color = java.awt.Color.white): NatureApplication = {
    application(
      states = Seq(
        ColorState(value = color)
      )
    )
  }

}

