package improbable.natures

import improbable.behaviours.ReportToTerrainGeneratorNature
import improbable.behaviours.color.SetColorFromFireBehaviour
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{Fire}
import improbable.util.EntityPrefabs.BOT
import improbable.behaviours.bot.MoveRandomlyBehaviour


object BotNature extends NatureDescription {

  override def dependencies = Set[NatureDescription](RigidbodyComposedTransformNature, ColoredNature, FlammableNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[MoveRandomlyBehaviour],
    descriptorOf[SetColorFromFireBehaviour],
    descriptorOf[ReportToTerrainGeneratorNature])

  def apply(initialPosition: Coordinates, onFire:Boolean = false): NatureApplication = {
    application(
      states = Seq(Fire(onFire)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BOT, initialPosition = initialPosition, drag = 0.2f),
        ColoredNature(color = java.awt.Color.white),
        FlammableNature(onFire))
    )
  }

}
