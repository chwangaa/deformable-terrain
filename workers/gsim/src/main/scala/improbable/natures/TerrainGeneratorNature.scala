package improbable.natures


import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.terrainchunk.Terrainseed
import improbable.util.EntityPrefabs._
import improbable.behaviours.TerrainGeneratorBehaviour

/**
  * Created by chihang on 21/03/2016.
  */
object TerrainGeneratorNature extends NatureDescription {
  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = {
    Set(descriptorOf[TerrainGeneratorBehaviour])
  }

  def apply(initialPosition: Coordinates, seed:Long, terrain_length:Int = 100): NatureApplication = {
    application(
      states = Seq(Terrainseed(seed, terrain_length)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TERRAINGENERATOR, initialPosition)
      )
    )
  }
}
