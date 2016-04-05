package improbable.behaviours

import com.typesafe.scalalogging.Logger
import improbable.math.Coordinates
import improbable.natures.BuildingNature
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.World
import improbable.terrainchunk.{BuildingRequest, TerrainUsageState, TerrainUsage}
import improbable.unity.fabric.PhysicsEngineConstraint
import improbable.util.TerrainGeneratorSetting.TERRAIN_LENGTH
import scala.util.Random

class TerrainUsageInitializerBehaviour(entity : Entity, logger : Logger, world: World) extends EntityBehaviour {
  override def onReady(): Unit = {

    // delegate building request to the physics engine
    entity.delegateState[BuildingRequest](PhysicsEngineConstraint)
    initializeTerrainUsage()


    entity.watch[BuildingRequest].onBuildingRequested {
      request => {
        // we want to avoid the tree generated from hitting the player, so add some random disturbance
        val width = request.width
        val height = request.height
        val length = request.length
        val position = request.position
        world.entities.spawnEntity(BuildingNature(initialPosition = position, width= width, height=height, length=length))
        logger.info(s"a building is spawned at position $position")
      }
    }
  }

  def initializeTerrainUsage():Unit = {
    val terrain_usage_type = entity.watch[TerrainUsage].usage.get

    terrain_usage_type match{
      case TerrainUsageState.TerrainUsageType.Residence =>
        generateResidence()
      case _ =>
        {}
    }
  }

  def generateResidence(): Unit = {
    generateHorizontalResidenceArea()
  }

  def generateHorizontalResidenceArea(): Unit = {
    var z: Int = (getRandomInRange(0, 20) + entity.position.z).toInt

    while(z < entity.position.z + TERRAIN_LENGTH - 20){
      val x: Int = (getRandomInRange(0, 10) + entity.position.x).toInt
      val width = getRandomInRange(20, 40)
      generateAHorizontalBlock(x, z, width)
      val street_width = getRandomInRange(15, 35)
      z += width + street_width
    }
  }

  def generateAHorizontalBlock(x_coordinate: Int, z_coordinate: Int, width: Int): Unit = {
    var start_x = x_coordinate
    val end_position = x_coordinate + getRandomInRange(TERRAIN_LENGTH - 20, TERRAIN_LENGTH)

    while(start_x < end_position){
      val height = getRandomInRange(30, 100)
      val length = getRandomInRange(10, 30)
      val position = Coordinates(start_x, 20, z_coordinate)
      world.entities.spawnEntity(BuildingNature(initialPosition = position, width= width, height=height, length=length))
      start_x += length
    }
  }

  def generateAVerticalBlock(x_coordinate: Int, z_coordinate: Int, length: Int): Unit = {
    var start_z = z_coordinate
    val end_position = Math.min(z_coordinate + getRandomInRange(TERRAIN_LENGTH - 20, TERRAIN_LENGTH), entity.position.z + TERRAIN_LENGTH)

    while(start_z < end_position){
      val height = getRandomInRange(30, 100)
      val width = getRandomInRange(10, 30)
      val position = Coordinates(x_coordinate, 20, start_z)
      world.entities.spawnEntity(BuildingNature(initialPosition = position, width= width, height=height, length=length))
      start_z += width
    }
  }

  def getRandomInRange(min: Int, max: Int): Int = {
    val num = Random.nextInt(max-min)
    return num + min;
  }


}