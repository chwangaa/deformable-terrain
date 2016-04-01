package improbable.behaviours.bot

import com.typesafe.scalalogging.Logger
import improbable.colorState.ColorState
import improbable.damage.{TeamStateData, TeamState}
import improbable.entity.physical.RigidbodyInterface
import improbable.math.{Coordinates, Vector3d}
import improbable.natures.BulletNature
import improbable.papi.world.World
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.util.FiringGameSetting._
import scala.concurrent.duration._
import scala.util.Random

class BotCombatBehaviour(logger:Logger, world: World, entity: Entity, rigidBodyInterface: RigidbodyInterface) extends EntityBehaviour{



  override def onReady(): Unit = {
    world.timing.every(BOT_FIRING_RATE.millisecond) {
      Combat()
    }
  }

  /**
    * implementation body of the firing, search a bot entity within a given region, and fire a bullet on top of it
    */
  def Combat(): Unit = {
    val my_team: TeamStateData.Team.Value = entity.watch[TeamState].side.get
    val my_team_tag = getTeamTag(my_team)

    // if opponent within firing radius, then fire
    val targets = world.entities.find(entity.position, BOT_FIRE_RADIUS, tags=Set("BOT")).filterNot(bot => bot.tags.contains(my_team_tag))
    if(!targets.isEmpty){
      val target = targets.last
      val target_position = target.position

      shootBulletToPosition(target_position, my_team)
    }
    else{
      // if opponent within view radius, move towards it
      val nearby_opponents = world.entities.find(entity.position, BOT_VIEW_RADIUS, tags=Set("BOT")).filterNot(bot => bot.tags.contains(my_team_tag))
      if(!nearby_opponents.isEmpty){
        val target = nearby_opponents.last
        val target_position = target.position
        moveTowardsPosition(target_position)
      }
      else{
        val x = Random.nextDouble() + 5
        val z = Random.nextDouble() + 5
        rigidBodyInterface.setForce(Vector3d(x, 0, z).normalised * BOT_MOVE_SPEED)
      }
    }

  }

  def moveTowardsPosition(destination: Coordinates):Unit = {
    val direction = destination.-(entity.position)
    val remove_y_component = Vector3d(direction.x, 0, direction.z)
    rigidBodyInterface.setForce(remove_y_component.normalised * BOT_MOVE_SPEED)
  }

  def shootBulletToPosition(target: Coordinates, team: TeamStateData.Team.Value): Unit = {
    val direction = target.-(entity.position).normalised
    val initial_position = entity.position.+(direction)

    world.entities.spawnEntity(BulletNature(initial_position, target=target, color=getColorFromTeam(team)))
  }
}