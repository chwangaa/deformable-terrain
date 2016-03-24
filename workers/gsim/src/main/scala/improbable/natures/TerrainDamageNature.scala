package improbable.natures

import improbable.behaviours.ReportToTerrainGeneratorNature
import improbable.behaviours.bot.MoveRandomlyBehaviour
import improbable.behaviours.color.SetColorFromFireBehaviour
import improbable.corelib.natures.base.BaseComposedTransformNature
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{Generatorreport, Fire}
import improbable.util.EntityPrefabs._

object TerrainDamageNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[ReportToTerrainGeneratorNature])

  def apply(initialPosition: Coordinates, onFire:Boolean = false): NatureApplication = {
    application(
      states = Seq(Fire(onFire),
        Generatorreport(false, 100, 100)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TERRAINDAMAGE, initialPosition = initialPosition, drag = 0.2f, mass = 100, rotationConstraints = FreezeConstraints(x = true, y = true, z = true))
      )
    )
  }
}
