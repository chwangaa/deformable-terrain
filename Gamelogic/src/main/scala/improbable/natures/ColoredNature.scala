package improbable.natures

import improbable.behaviours.color.ColorConversions._
import improbable.behaviours.color.ColorBehaviour
import improbable.color.ColorState
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}

object ColoredNature extends NatureDescription {

  override val dependencies = Set[NatureDescription]()

  override def activeBehaviours = {
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
