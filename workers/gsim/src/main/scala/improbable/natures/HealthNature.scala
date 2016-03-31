package improbable.natures

import improbable.behaviours.HealthBehaviour
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.physical.Bla
import improbable.util.FiringGameSetting.INITIAL_HEALTH

object HealthNature extends NatureDescription {
  override val dependencies = Set[NatureDescription]()

  override def activeBehaviours = {
    Set(
      descriptorOf[HealthBehaviour]
    )
  }

  def apply(health: Int = INITIAL_HEALTH): NatureApplication = {
    application(
      states = Seq(Bla(life = health))
    )
  }
}
