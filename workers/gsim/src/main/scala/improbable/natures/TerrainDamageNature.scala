package improbable.natures

import improbable.behaviours.ReportToTerrainGeneratorBehaviour
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.damage.DamageState
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical.{Generatorreport, Fire}
import improbable.util.EntityPrefabs._

object TerrainDamageNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[ReportToTerrainGeneratorBehaviour])

  def apply(initialPosition: Coordinates, terrain_coordinate:Coordinates): NatureApplication = {
    application(
      states = Seq(
        Generatorreport(false, 100, 100),
        DamageState(terrain_coordinate)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TERRAINDEMAGE, initialPosition = initialPosition, drag = 0.2f, mass = 100, rotationConstraints = FreezeConstraints(x = true, y = true, z = true))
      )
    )
  }
}
