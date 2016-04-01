package improbable.natures

import improbable.behaviours.bot.BotProductionBehaviour
import improbable.behaviours.{ReportToTerrainGeneratorBehaviour}
import improbable.corelib.natures.rigidbody.RigidbodyComposedTransformNature
import improbable.corelib.natures.{NatureApplication, NatureDescription}
import improbable.damage.{TeamStateData, TeamState}
import improbable.entity.physical.FreezeConstraints
import improbable.math.Coordinates
import improbable.papi.entity.behaviour.EntityBehaviourDescriptor
import improbable.physical._
import improbable.util.EntityPrefabs.BOTWAREHOUSE
import improbable.util.FiringGameSetting._

object BotwarehouseNature extends NatureDescription {

  override def dependencies = Set[NatureDescription](RigidbodyComposedTransformNature, ColoredNature, HealthNature)

  override def activeBehaviours: Set[EntityBehaviourDescriptor] = Set[EntityBehaviourDescriptor](
    descriptorOf[ReportToTerrainGeneratorBehaviour],
    descriptorOf[BotProductionBehaviour]
  )


  def apply(initialPosition: Coordinates, team:TeamStateData.Team.Value = TeamStateData.Team.RED, onFire:Boolean = false, checkout_radius:Int = 100, report_period:Int = 500): NatureApplication = {

    application(
      states = Seq(Generatorreport(false, report_period, checkout_radius),
        TeamState(team)
      ),
      natures = Seq(
        RigidbodyComposedTransformNature(entityPrefab = BOTWAREHOUSE, initialPosition = initialPosition, drag = 0.2f, tags=List("BOT", getTeamTag(team)), rotationConstraints = FreezeConstraints(x = true, y = true, z = true), positionConstraints = FreezeConstraints(x = true, y = true, z = true)),
        ColoredNature(getColorFromTeam(team))
        ,HealthNature(health=BOT_WAREHOUSE_LIFE)
      )
    )
  }

}
