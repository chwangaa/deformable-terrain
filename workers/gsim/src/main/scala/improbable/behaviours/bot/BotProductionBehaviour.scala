package improbable.behaviours.bot

import com.typesafe.scalalogging.Logger
import improbable.damage.{TeamState}
import improbable.math.{Coordinates, Vector3d}
import improbable.natures.BotNature
import improbable.papi.entity.{Entity, EntityBehaviour}
import improbable.papi.world.World

import improbable.util.FiringGameSetting.BOT_PRODUCTION_PERIOD
import scala.concurrent.duration._

class BotProductionBehaviour(world: World,
                            entity: Entity,
                            logger: Logger
                           ) extends EntityBehaviour {


  override def onReady(): Unit = {
    val my_team = entity.watch[TeamState].side.get
    val start_position = Coordinates(0, 60, 0)

    world.timing.every(BOT_PRODUCTION_PERIOD.milliseconds) {
      world.entities.spawnEntity(BotNature(start_position, team=my_team))
    }

  }


}
