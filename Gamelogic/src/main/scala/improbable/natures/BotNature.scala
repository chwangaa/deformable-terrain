package improbable.natures

import improbable.corelib.natures.{NatureApplication, NatureDescription, RigidbodyEntity}
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.util.EntityPrefabs.BOT

object BotNature extends NatureDescription {

  override val dependencies = Set[NatureDescription](RigidbodyEntity, ColoredNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set.empty
  }

  def apply(initialPosition: Coordinates, onFire: Boolean = false): NatureApplication = {
    application(
      natures = Seq(
        RigidbodyEntity(prefab = BOT, position = initialPosition, drag = 0.2f),
        ColoredNature(color = java.awt.Color.red))
    )
  }

}
