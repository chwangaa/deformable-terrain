package improbable.natures

import improbable.behaviours.physical.MoveRandomlyBehaviour
import improbable.corelib.entity.nature.definitions.RigidbodyEntity
import improbable.corelib.entity.nature.{NatureApplication, NatureDescription}
import improbable.math._
import improbable.util.EntityPrefabs.PLAYER

object BotNature extends NatureDescription {

  override val dependencies = Set[NatureDescription](RigidbodyEntity, ColoredNature)

  override def activeBehaviours = {
    Set(descriptorOf[MoveRandomlyBehaviour])
  }

  def apply(initialPosition: Coordinates, onFire: Boolean = false): NatureApplication = {
    application(
      natures = Seq(
        RigidbodyEntity(prefab = PLAYER, position = initialPosition, drag = 0.2f),
        ColoredNature(color = java.awt.Color.red))
    )
  }

}