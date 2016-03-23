package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.behaviours.bot.MovementEvent
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import improbable.physical.{Generatorreport, GeneratorReportData}
import scala.concurrent.duration._

/**
  * Created by chihang on 22/03/2016.
  */
class ReportToTerrainGeneratorNature(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {

  var checkout_radius: Int = 100
  var report_period: Int = 1000
  var do_report: Boolean = false



  override def onReady(): Unit = {
    checkout_radius = entity.watch[Generatorreport].checkoutRadius.get
    report_period = entity.watch[Generatorreport].reportPeriod.get
    do_report = entity.watch[Generatorreport].report.get
    initializeReport()
  }

  def initializeReport(): Unit = {
    if(do_report){
      world.timing.every(report_period.milliseconds){
        world.entities.find(EntityFindByTag("TerrainGenerator")).foreach(
          x =>
            world.messaging.sendToEntity(x.entityId, MovementEvent(entity.position, checkout_radius))
        )
      }
    }
  }
}