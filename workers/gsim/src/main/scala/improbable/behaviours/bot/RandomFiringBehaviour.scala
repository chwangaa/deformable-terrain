package improbable.behaviours.bot

import com.typesafe.scalalogging.Logger
import improbable.colorState.ColorState
import improbable.damage.{TeamStateData, TeamState}
import improbable.math.{Vector3d}
import improbable.natures.BulletNature
import improbable.papi.world.World
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.util.FiringGameSetting._
import scala.concurrent.duration._
import scala.util.Random

class RandomFiringBehaviour(logger:Logger, world: World, entity: Entity) extends EntityBehaviour{

  /**
    * start the periodic random firing
    */
  val active_radius:Float = 5.0f

  override def onReady(): Unit = {
    world.timing.every(5.seconds) {
      randomFiring()
    }
  }

  /**
    * implementation body of the firing, search a bot entity within a given region, and fire a bullet on top of it
    */
  def randomFiring(): Unit = {
    val my_team: TeamStateData.Team.Value = entity.watch[TeamState].side.get
    val my_team_tag = getTeamTag(my_team)

    val targets = world.entities.find(entity.position, active_radius, tags=Set("BOT")).filterNot(bot => bot.tags.contains(my_team_tag))
    if(targets.isEmpty){
      //Do nothing
    }
    else{
      val target = targets.last
      val target_position = target.position.+(Vector3d(0, 10, 0)) // create a bullet at 10 unit above the target
      val radius = Random.nextInt(10)

      world.entities.spawnEntity(BulletNature(target_position, radius=radius, color=getColorFromTeam(my_team)))
    }


  }
}