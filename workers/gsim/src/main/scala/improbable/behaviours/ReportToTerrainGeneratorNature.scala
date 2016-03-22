package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.behaviours.bot.MovementEvent
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import scala.concurrent.duration._

/**
  * Created by chihang on 22/03/2016.
  */
class ReportToTerrainGeneratorNature(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {




  override def onReady(): Unit = {

    world.timing.every(1000.milliseconds) {
      world.entities.find(EntityFindByTag("TerrainGenerator")).foreach(
        x =>
          world.messaging.sendToEntity(x.entityId, MovementEvent(entity.position))
      )
    }
  }
}