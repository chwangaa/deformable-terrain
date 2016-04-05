package improbable.natures

import improbable.behaviours.{TerrainUsageInitializerBehaviour, TerrainInstanceBehaviour}
import improbable.behaviours.bot.MoveRandomlyBehaviour
import improbable.behaviours.color.SetColorFromFireBehaviour
import improbable.corelib.natures.base.BaseComposedTransformNature
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.terrainchunk._
import improbable.util.EntityPrefabs.BUILDING
import improbable.util.FiringGameSetting._

object BuildingNature extends NatureDescription {

  override def dependencies: Set[NatureDescription] = Set(RigidbodyComposedTransformNature, ColoredNature, HealthNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    //descriptorOf[BuildingInitializerBehaviour]
  )

  def apply(initialPosition: Coordinates, length: Int, width:Int, height:Int): NatureApplication = {

    application(
      states = Seq(
        Building(length=length, height=height, width=width)),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BUILDING, initialPosition, mass = 100, isKinematic = true, positionConstraints = FreezeConstraints(x = true, y = true, z = true), rotationConstraints = FreezeConstraints(x = true, y = true, z = true)),
        ColoredNature(java.awt.Color.GRAY),
        HealthNature()

      )
    )
  }
}