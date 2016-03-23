package improbable.natures

import improbable.behaviours.TerrainInstanceBehaviour

import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.terrainchunk.Terrainseed
import improbable.util.EntityPrefabs._

object TerrainChunkNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] =
    Set(descriptorOf[TerrainInstanceBehaviour])


  def apply(initialPosition: Coordinates, seed: Long, terrain_length:Int, category:String = "Terrain"): NatureApplication = {
    application(
      states = Seq(Terrainseed(seed, terrain_length)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TERRAINCHUNK, initialPosition, mass = 100, drag = 100, positionConstraints = FreezeConstraints(x = true, y = true, z = true), rotationConstraints = FreezeConstraints(x = true, y = true, z = true), tags=List(category))
      )
    )
  }
}