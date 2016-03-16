package improbable.natures

import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.behaviours.color.SetColorFromFireBehaviour
import improbable.physical.{Fire, PropagateFireBehaviour}

object FlammableNature extends NatureDescription {
  override val dependencies = Set[NatureDescription]()

  override def activeBehaviours = {
    Set(
      descriptorOf[PropagateFireBehaviour],
      descriptorOf[SetColorFromFireBehaviour]
    )
  }

  def apply(onFire: Boolean = false): NatureApplication = {
    application(
      states = Seq(Fire(onFire))
    )
  }
}