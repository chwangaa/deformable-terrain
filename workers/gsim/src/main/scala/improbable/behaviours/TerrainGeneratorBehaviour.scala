package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.behaviours.bot.MovementEvent
import improbable.math.Coordinates
import improbable.natures.TerrainChunkNature
import improbable.papi.EntityId
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.physical.{Extinguish, Ignite, RaycastResponse}
import improbable.terrainchunk.{Terrainseed}
import scala.Tuple2
import scala.concurrent.duration._
/**
  * Created by chihang on 21/03/2016.
  */
class TerrainGeneratorBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {

  var terrain_length = 100
  val garbage_collector_period = 30 // period to which to garbage collect old terrain, in second
  var generatedTerrain = Map[Coordinates, EntityId]()
  var markedTerrain = Set[Coordinates]()
  var seed : Long = 0

  override def onReady(): Unit = {
    seed = entity.watch[Terrainseed].seed.get
    terrain_length = entity.watch[Terrainseed].terrainLength.get

    logger.info(s"Terrain Generator with seed value $seed and size $terrain_length ready")
    // add tag to the entity so movement event can be received here
    entity.addTag("TerrainGenerator")
    // reconstructing the map upon failure
    reconstruction_code()

    // listen to the movement events
    world.messaging.onReceive {
      case MovementEvent(position, radius) =>
        checkIfNewObjectPositionRequireMoreTerrains(position, radius)
    }

    // start garbage collector
     initializeGarbageCollector()
  }

  def initializeGarbageCollector() = {
    world.timing.every(garbage_collector_period.second){
      garbageCollection()
    }
  }

  def reconstruction_code():Unit = {
    val terrain_entities = world.entities.find(Coordinates(0,0,0), 10000000, Set("Terrain"))
    terrain_entities.foreach(terrain =>
            generatedTerrain += terrain.position -> terrain.entityId
    )
  }

  def checkIfNewObjectPositionRequireMoreTerrains(position: Coordinates, radius: Int): Unit ={
    val terrain_coordinates = findCoordinatesOfTerrainsThatNeedToBeGenerated(position, radius)

    val new_coordinates = terrain_coordinates.diff(generatedTerrain.keySet)
    val old_coordinates = generatedTerrain.keySet.diff(terrain_coordinates)
    // this is the mark stage of garbage collection
    updateGarbageCollector(terrain_coordinates)
    new_coordinates.foreach(
      position => {
        generateNewTerrain(position)
      }
    )
  }

  def updateGarbageCollector(required_terrain_set:Set[Coordinates]):Unit = {
    required_terrain_set.foreach(terrain =>
      markedTerrain += terrain
    )
  }

  // destroy the terrain entities as indexed by garbageTerrain set
  def garbageCollection():Unit = {
    logger.info("calling garbage collector")

    // the terrains that can be deleted are those that are not mapped, i.e. GeneratedTerrainSet - MarkedTerrainSet
    val terrains_to_sweep : Set[Coordinates] = generatedTerrain.keySet.diff(markedTerrain)
    markedTerrain = Set[Coordinates]() // reset the MarkedTerrainSet
    // marked_stage
    terrains_to_sweep.foreach(terrain => {
        logger.info(terrain.toString() + " is being garbage collected now")
        generatedTerrain.get(terrain) match{
           case Some(id) => {
             destroyTerrain(id)
             generatedTerrain -= terrain
           }
           case None => logger.info("error: the id in the cached set does not exist!")
          }
      }
    )
  }

  def destroyTerrain(id: EntityId):Unit = {
      world.entities.destroyEntity(id)
  }

  // generate a new terrain entity at given coordinate
  def generateNewTerrain(position:Coordinates): Unit ={
    if(!generatedTerrain.contains(position)){
       val id = world.entities.spawnEntity(TerrainChunkNature(position, seed, terrain_length))
       generatedTerrain += position -> id
       // logger.info("new terrain generated at position " + position.toString())
    }
  }


  def getTerrainCoordinateForObjectPosition(x : Double, z : Double): Coordinates = {
    val terrain_x:Int = math.floor(x / terrain_length).toInt * terrain_length
    val terrain_z:Int = math.floor(z / terrain_length).toInt * terrain_length
    return new Coordinates(terrain_x, 0, terrain_z)
  }

  def findCoordinatesOfTerrainsThatNeedToBeGenerated(position: Coordinates, radius: Int): Set[Coordinates] = {


    val x:Double = position.x
    val z:Double = position.z

    var neighbour_terrains = Set[Coordinates]()

    for{
      i <- x-radius to x+radius by terrain_length
      j <- z-radius to z+radius by terrain_length
    }{
      val new_terrain = getTerrainCoordinateForObjectPosition(i, j)
      neighbour_terrains += new_terrain
    }

    return neighbour_terrains
  }

}