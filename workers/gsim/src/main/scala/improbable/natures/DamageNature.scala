package improbable.natures

import improbable.behaviours.{ReportToTerrainGeneratorBehaviour}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.damage.{DamagedState, DamagedStateData, TeamStateData, TeamState}
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical._
import improbable.util.EntityPrefabs.TERRAINDEMAGE
import improbable.behaviours.bot.{BotCombatBehaviour, RandomFiringBehaviour, MoveRandomlyBehaviour}
import improbable.util.FiringGameSetting._

object DamageNature extends NatureDescription {

  override def dependencies = Set[NatureDescription](RigidbodyComposedTransformNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
  )


  def apply(initialPosition: Coordinates, damaged_position: Coordinates, radius: Int): NatureApplication = {

    application(
      states = Seq(
        DamagedState(position=damaged_position, radius= radius)
      ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = TERRAINDEMAGE, initialPosition = initialPosition)
      )
    )
  }

}
