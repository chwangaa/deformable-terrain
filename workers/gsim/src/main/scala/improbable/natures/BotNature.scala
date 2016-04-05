package improbable.natures

import improbable.behaviours.{ReportToTerrainGeneratorBehaviour}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.damage.{TeamStateData, TeamState}
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical._
import improbable.util.EntityPrefabs.BOT
import improbable.behaviours.bot.{BotCombatBehaviour, RandomFiringBehaviour, MoveRandomlyBehaviour}
import improbable.util.FiringGameSetting._

object BotNature extends NatureDescription {

  override def dependencies = Set[NatureDescription](RigidbodyComposedTransformNature, ColoredNature, HealthNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[ReportToTerrainGeneratorBehaviour]
    ,descriptorOf[MoveRandomlyBehaviour]
  )


  def apply(initialPosition: Coordinates, team:TeamStateData.Team.Value = TeamStateData.Team.RED, onFire:Boolean = false, checkout_radius:Int = 100, report_period:Int = 500): NatureApplication = {

    application(
      states = Seq(Generatorreport(true, report_period, checkout_radius),
        TeamState(team)
      ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BOT, initialPosition = initialPosition, drag = 0.2f, tags=List("BOT", getTeamTag(team))),
        ColoredNature(getColorFromTeam(team))
        ,HealthNature()
        )
    )
  }

}
