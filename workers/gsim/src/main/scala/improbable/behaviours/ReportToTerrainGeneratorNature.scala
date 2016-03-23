package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.behaviours.bot.MovementEvent
import improbable.papi.EntityId
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import improbable.physical.{Generatorreport}
import scala.concurrent.duration._

/**
  * Created by chihang on 22/03/2016.
  */
class ReportToTerrainGeneratorNature(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {

  var checkout_radius: Int = 100
  var report_period: Int = 1000
  var do_report: Boolean = false
  var is_persistent:Boolean = false
  var generator_id:EntityId = 0



  override def onReady(): Unit = {
    checkout_radius = entity.watch[Generatorreport].checkoutRadius.get
    report_period = entity.watch[Generatorreport].reportPeriod.get
    do_report = entity.watch[Generatorreport].report.get
    is_persistent = entity.watch[Generatorreport].isPersistent.get
    findGeneratorId()
    if(is_persistent){
      logger.info("a persistent entity is reporting")
      createPersistentTerrain()
    }
    else{
      logger.info("a dynamic entity is reporting")
      initializeReport()
    }


  }

  def findGeneratorId(): Unit = {
    generator_id = world.entities.find(EntityFindByTag("TerrainGenerator")).last.entityId
  }

  def initializeReport(): Unit = {
    if(do_report){
      world.timing.every(report_period.milliseconds){
//        world.entities.find(EntityFindByTag("TerrainGenerator")).foreach(
//          x =>
            world.messaging.sendToEntity(generator_id, MovementEvent(entity.position, checkout_radius))
//        )
      }
    }
  }

  def createPersistentTerrain():Unit = {
    world.entities.find(EntityFindByTag("TerrainGenerator")).foreach(
      x =>
        world.messaging.sendToEntity(x.entityId, CreatePersistentTerrainEvent(entity.position))
    )
  }
}