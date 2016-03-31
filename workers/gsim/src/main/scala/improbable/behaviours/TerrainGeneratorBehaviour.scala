package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.math.Coordinates
import improbable.natures.TerrainChunkNature
import improbable.papi.EntityId
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.papi.world.messaging.CustomMsg
import improbable.terrainchunk.{TerrainSeedData, Terrainseed}
import improbable.util.TerrainGeneratorSetting
import scala.concurrent.duration._
import improbable.util.TerrainCoordinateMapping._
/**
  * Created by chihang on 21/03/2016.
  */
case class GenerateNeighbouringTerrainAt(position: Coordinates, radius: Int) extends CustomMsg
case class RequireTerrainAt(position:Coordinates) extends CustomMsg
case class DamageTerrainIdAtPosition(position: Coordinates, damage_position: Coordinates, radius: Int) extends CustomMsg


class TerrainGeneratorBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {

  var terrain_length = TerrainGeneratorSetting.TERRAIN_LENGTH
  val garbage_collector_period = TerrainGeneratorSetting.GARBAGE_COLLECTION_EXECUTION_PERIOD
  var generatedTerrain = Map[Coordinates, EntityId]()
  var markedTerrain = Set[Coordinates]()
  var seed : Long = entity.watch[Terrainseed].seed.get
  val nature: TerrainSeedData.TerrainType.Value = entity.watch[Terrainseed].nature.get

  override def onReady(): Unit = {
    seed = entity.watch[Terrainseed].seed.get
    terrain_length = TerrainGeneratorSetting.TERRAIN_LENGTH

    entity.addTag("TerrainGenerator")     // add tag to the entity so movement event can be received here

    reconstruction_code()     // reconstructing the map upon   failure


    // listen to communication events
    world.messaging.onReceive {
      case GenerateNeighbouringTerrainAt(position, radius) => // this event generates all the terrains within the radius
        generateTerrainsInNeighbourhood(position, radius)
      case RequireTerrainAt(position) =>          // this event generate the terrain at the specified position
        generateNewTerrainAtPosition(position)
      case DamageTerrainIdAtPosition(position, damage_center, radius) =>
        forwardTerrainDamageToTerrain(position, damage_center, radius)
    }

    initializeGarbageCollector() // start garbage collector

    logger.info(s"Terrain Generator with seed value $seed and size $terrain_length ready")

  }

  /**
    * start the periodic garbage collector
    */
  def initializeGarbageCollector():Unit = {
    world.timing.every(garbage_collector_period.second){
      garbageCollection()
    }
  }

  /**
    * call this code upon every onReady upon failure to reconstruct the generatedTerrain list
    */
  def reconstruction_code():Unit = {
    val terrain_entities = world.entities.find(Coordinates(0,0,0), 10000000, Set("Terrain"))
    terrain_entities.foreach(terrain =>
            generatedTerrain += terrain.position -> terrain.entityId
    )
  }

  /**
    * find and generate the required terrains in the neighborhood
 *
    * @param position center from which to generate the neighbouring terrains
    * @param radius radius from which to generate the inclusive terrains
    */
  def generateTerrainsInNeighbourhood(position: Coordinates, radius: Int): Unit ={
    val terrain_coordinates = findCoordinatesOfTerrainsThatNeedToBeGenerated(position, radius)

    val new_coordinates = terrain_coordinates.diff(generatedTerrain.keySet)
    val old_coordinates = generatedTerrain.keySet.diff(terrain_coordinates)
    // this is the mark stage of garbage collection
    markTerrainAsRequired(terrain_coordinates)
    new_coordinates.foreach(
      position => {
        generateNewTerrainAtPosition(position)
      }
    )
  }

  /** mark the given set of terrains so they do not get garbage collected */
  def markTerrainAsRequired(required_terrain_set:Set[Coordinates]):Unit = {
    required_terrain_set.foreach(terrain =>
      markedTerrain += terrain
    )
  }

  /** marked the given terrain so it does not get garbage collected */
  def markTerrainAsRequired(required_terrain:Coordinates):Unit = {
    markedTerrain += required_terrain
  }

  /* destroy the terrains that are not being marked */
  def garbageCollection():Unit = {
    logger.info("calling garbage collector")


    val terrains_to_sweep = generatedTerrain.keySet.diff(markedTerrain)     // the terrains that can be deleted are those that are not mapped, i.e. GeneratedTerrainSet - MarkedTerrainSet
    markedTerrain = Set[Coordinates]() // reset the MarkedTerrainSet

    // marked_stage
    terrains_to_sweep.foreach(terrain => {
        logger.info(terrain.toString() + " is being garbage collected now")
        generatedTerrain.get(terrain) match{
           case Some(id) => {
             if(!markedTerrain.contains(terrain)) { // this extra check is required to make sure no terrain generation request received in the period of this function call
               destroyTerrain(id)
               generatedTerrain -= terrain
             }

           }
           case None => logger.info("error: the id in the cached set does not exist!")
          }
      }
    )
  }

  /**
    * destroy the terrain entity
 *
    * @param id destroy the terrain specified by the entity Id
    */
  def destroyTerrain(id: EntityId):Unit = {
      world.entities.destroyEntity(id)
      world.messaging.sendToEntity(id, TerrainDestroyRequest())
  }


  /**
    * generate a new terrain entity at given coordinate
 *
    * @param position position of the terrain entity
    */
  def generateNewTerrainAtPosition(position:Coordinates): Unit ={
    markTerrainAsRequired(position)      // remove the position from the garbage collector since we still want it
    if(!generatedTerrain.contains(position)){ // check if it already exists
       val id = world.entities.spawnEntity(TerrainChunkNature(position, seed, terrain_length, nature))  // generate the terrain
       generatedTerrain += position -> id
    }
  }

  /**
    * forward the terrain damage request to the corresponding terrain
 *
    * @param position position of the terrain entity, to which the damage is applying to
    * @param damage_center position where the damage takes place
    * @param radius radius of the damage
    */
  def forwardTerrainDamageToTerrain(position: Coordinates, damage_center: Coordinates, radius:Int): Unit = {
    val terrain_id:EntityId = generatedTerrain(position) // find the terrain ID from the map
    world.messaging.sendToEntity(terrain_id, TerrainDamage(damage_center, radius))

  }

}