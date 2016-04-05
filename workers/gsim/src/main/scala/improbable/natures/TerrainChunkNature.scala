package improbable.natures

import improbable.behaviours.{TerrainUsageInitializerBehaviour, TerrainInstanceBehaviour}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.terrainchunk._
import improbable.util.EntityPrefabs._
import improbable.util.TerrainUsageTypeUtil.getRandomTerrainType

object TerrainChunkNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
                        descriptorOf[TerrainInstanceBehaviour],
                        descriptorOf[TerrainUsageInitializerBehaviour]
  )

  def apply(initialPosition: Coordinates, seed: Long, terrain_length:Int, terrain_type:TerrainSeedData.TerrainType.Value, terrain_usage: TerrainUsageState.TerrainUsageType.Value): NatureApplication = {

    application(
      states = Seq(Terrainseed(seed, terrain_length, nature = terrain_type),
                   TerrainUsage(usage = terrain_usage),
                   TerrainDamage(List[Damage]()),
                   DamageRequest(),
                   BuildingRequest()
        ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TERRAINCHUNK, initialPosition, mass = 100, drag = 100,
          positionConstraints = FreezeConstraints(x = true, y = true, z = true),
          rotationConstraints = FreezeConstraints(x = true, y = true, z = true),
          tags = List(terrain_usage.toString))
      )
    )
  }
}