package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.behaviours.bot.MovementEvent
import improbable.math.Coordinates
import improbable.natures.TerrainNature
import improbable.papi.EntityId
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.physical.{Extinguish, Ignite, RaycastResponse}
import scala.Tuple2
import scala.concurrent.duration._
/**
  * Created by chihang on 21/03/2016.
  */
class TerrainGeneratorBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {

  val radius = 100
  val terrain_length = 100
  val garbage_collector_period = 0.5 // period to which to garbage collect old terrain, in second
  var generatedTerrain = Map[Coordinates, EntityId]()
  var garbageTerrain = Set[Coordinates]()

  override def onReady(): Unit = {
    logger.info("Terrain Generator ready")
    // add tag to the entity so movement event can be received here
    entity.addTag("TerrainGenerator")
    // reconstructing the map upon failure
    reconstruction_code()

    // listen to the movement events
    world.messaging.onReceive {
      case MovementEvent(position) =>
        checkIfNewObjectPositionRequireMoreTerrains(position)
    }

    // start garbage collector
     initializeGarbageCollector()
  }

  def initializeGarbageCollector() = {
    world.timing.every(garbage_collector_period.minute){
      garbageCollection()
    }
  }

  def reconstruction_code():Unit = {
    val terrain_entities = world.entities.find(Coordinates(0,0,0), 10000000, Set("Terrain"))
    terrain_entities.foreach(terrain =>
            generatedTerrain += terrain.position -> terrain.entityId
    )
  }

  def checkIfNewObjectPositionRequireMoreTerrains(position: Coordinates): Unit ={
    val terrain_coordinates = findCoordinatesOfTerrainsThatNeedToBeGenerated(position)

    val new_coordinates = terrain_coordinates.diff(generatedTerrain.keySet)
    val old_coordinates = generatedTerrain.keySet.diff(terrain_coordinates)
    updateGarbageCollector(terrain_coordinates, old_coordinates)

    new_coordinates.foreach(
      position => {
        logger.info(position.toString() + " is new terrain coordinate")
        generateNewTerrain(position)
      }
    )
  }

  def updateGarbageCollector(new_terrain:Set[Coordinates], old_terrain:Set[Coordinates]):Unit = {
    new_terrain.foreach(terrain =>
      garbageTerrain -= terrain
    )
    old_terrain.foreach(terrain =>
      garbageTerrain += terrain
    )
  }

  // destroy the terrain entities as indexed by garbageTerrain set
  def garbageCollection():Unit = {
    logger.info("calling garbage collector")
    garbageTerrain.foreach(terrain => {
        logger.info(terrain.toString() + " can be garbage collected now")
        generatedTerrain.get(terrain) match{
           case Some(id) => {
             world.entities.destroyEntity(id)
             garbageTerrain -= terrain
             generatedTerrain -= terrain
           }
           case None => logger.info("error: the id in the cached set does not exist!")
          }
    }
    )
  }

  // generate a new terrain entity at given coordinate
  def generateNewTerrain(position:Coordinates): Unit ={
    if(!generatedTerrain.contains(position)){
      val id = world.entities.spawnEntity(TerrainNature(position))
      generatedTerrain += position -> id
      logger.info("new terrain generated at position " + position.toString())
    }
  }


  def getTerrainCoordinateForObjectPosition(x : Double, z : Double): Coordinates = {
    val terrain_x:Int = math.floor(x / terrain_length).toInt * terrain_length
    val terrain_z:Int = math.floor(z / terrain_length).toInt * terrain_length
    return new Coordinates(terrain_x, 0, terrain_z)
  }

  def findCoordinatesOfTerrainsThatNeedToBeGenerated(position: Coordinates): Set[Coordinates] = {


    val x:Double = position.x
    val z:Double = position.z

    var neighbour_terrains = Set[Coordinates]()

    for{
      i <- x-radius to x+radius by 100
      j <- z-radius to z+radius by 100
    }{
      val new_terrain = getTerrainCoordinateForObjectPosition(i, j)
      neighbour_terrains += new_terrain
    }

//    neighbour_terrains.foreach(x =>
//      logger.info("this terrain is required " + x.toString())
//    )

    return neighbour_terrains
  }

}