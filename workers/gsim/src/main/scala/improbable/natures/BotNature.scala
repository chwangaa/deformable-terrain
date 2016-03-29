package improbable.natures

import improbable.behaviours.ReportToTerrainGeneratorBehaviour
import improbable.behaviours.color.SetColorFromFireBehaviour
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{Generatorreport, Fire}
import improbable.util.EntityPrefabs.BOT
import improbable.behaviours.bot.MoveRandomlyBehaviour


object BotNature extends NatureDescription {

  override def dependencies = Set[NatureDescription](RigidbodyComposedTransformNature, ColoredNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[MoveRandomlyBehaviour],
    descriptorOf[ReportToTerrainGeneratorBehaviour])

  def apply(initialPosition: Coordinates, onFire:Boolean = false, checkout_radius:Int = 100, report_period:Int = 500): NatureApplication = {
    application(
      states = Seq(Fire(onFire), Generatorreport(true, report_period, checkout_radius)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BOT, initialPosition = initialPosition, drag = 0.2f),
        ColoredNature(color = java.awt.Color.white)
        )
    )
  }

}
