package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.math.Coordinates
import improbable.papi._
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.entities.EntityFindByTag
import improbable.physical.{Generatorreport, GeneratorReportData}
import scala.concurrent.duration._
import improbable.util.TerrainGeneratorSetting
import improbable.util.TerrainCoordinateMapping._

/**
  * Created by chihang on 22/03/2016.
  */
class ReportToTerrainGeneratorBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {

  var checkout_radius: Int = 100
  var report_period: Int = TerrainGeneratorSetting.GARBAGE_COLLECTION_UPDATE_PERIOD
  var do_report: Boolean = false
  var terrain_length: Int = TerrainGeneratorSetting.TERRAIN_LENGTH
  var previous_position: Coordinates = entity.position

  var generatedTerrain = Set[Coordinates]()
  var oldTerrain = Set[Coordinates]()

  var generator_id: EntityId = findGenerator()

  def findGenerator():EntityId = {
    // busy wait until a terrain generator is available
    while(world.entities.find(EntityFindByTag("TerrainGenerator")).isEmpty){
      world.timing.after(5.seconds){
        logger.info("terrain generator is not found")
      }
    }
    return world.entities.find(EntityFindByTag("TerrainGenerator")).last.entityId

  }

  override def onReady(): Unit = {
    /*
      first, update all class level elements
     */
    checkout_radius = entity.watch[Generatorreport].checkoutRadius.get
    report_period = TerrainGeneratorSetting.GARBAGE_COLLECTION_UPDATE_PERIOD
    do_report = entity.watch[Generatorreport].report.get
    previous_position = entity.position
    generator_id = findGenerator()

    initializeReporter() // initialize the reporter, which performs the periodic check on positions
  }

  def initializeReporter(): Unit = {

    generateNeighbourhood()


    if(do_report){        // if the object is dynamic, then we track its motion, and whenever it drifts by a certain amount, we do a check to ensure its neighbour terrains are generated
      world.timing.every(100.milliseconds){ // TODO this should be relative to the velocity
        ensureAllNeighbourTerrainsAreGenerated()
      }
    }
    else{                // if the object is static, such as a tree, we only generate the terrain which holds it
      val terrain_position = getTerrainCoordinateForObjectPosition(entity.position.x, entity.position.z)
      askGeneratorForNewTerrain(terrain_position)
    }
    // both dynamic and static objects, we periodically send a message to the generator to make sure the neighbourhood of interest is not being garbage collected
    world.timing.every(report_period.seconds){
      tellGeneratorTerrainsStillRequired()
    }
  }

  def ensureAllNeighbourTerrainsAreGenerated(): Unit = {
    if(positionDriftedFarEnough()){
      generateNeighbourhood()
    }
  }

  /** periodically call this function to ensure the generator does not garbage collect the terrains still in our field of intertest */
  def tellGeneratorTerrainsStillRequired(): Unit = {
    generatedTerrain.foreach(position =>
      world.messaging.sendToEntity(generator_id, RequireTerrainAt(position))
    )
  }

  /**
    * ask generator to generate all the terrains in the neighbourhood
    */
  def generateNeighbourhood(): Unit = {
    val neighbour_terrains = findCoordinatesOfTerrainsThatNeedToBeGenerated(entity.position, checkout_radius)
    val new_coordinates = neighbour_terrains.diff(generatedTerrain)
    val old_coordinates = generatedTerrain.diff(neighbour_terrains)
    removeOldTerrains(old_coordinates)
    askGeneratorForTheseTerrains(new_coordinates)
  }

  /**
    * ask generator to generate terrains
    * @param new_coordinates set of terrains of which to ask the generator to generate
    */
  def askGeneratorForTheseTerrains(new_coordinates:Set[Coordinates]):Unit = {
    new_coordinates.foreach(
      position => {
        askGeneratorForNewTerrain(position)
      }
    )
  }

  def positionDriftedFarEnough(): Boolean ={
    val current_position = entity.position
    if(current_position.distanceTo(previous_position) >= terrain_length*0.9){ // TODO we potentially want make this relative to the object velocity
      previous_position = current_position
      return true
    }
    else{
      return false
    }
  }

  /**
    * ask generator to generate a particular terrain specified by the position
    * @param position position of the new terrain
    */
  def askGeneratorForNewTerrain(position:Coordinates): Unit = {

    world.messaging.sendToEntity(generator_id, RequireTerrainAt(position))
    generatedTerrain += position
  }

  /**
    * remove the old terrain from the active list
    * @param old_terrains old terrain set
    */
  def removeOldTerrains(old_terrains:Set[Coordinates]): Unit = {
    old_terrains.foreach(position =>{
      generatedTerrain -= position
    })
  }

}