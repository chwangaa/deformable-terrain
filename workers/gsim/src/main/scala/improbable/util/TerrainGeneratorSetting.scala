package improbable.util

import improbable.math.Coordinates
import improbable.papi.entity.EntityPrefab
import improbable.terrainchunk.TerrainSeedData

object TerrainGeneratorSetting {

  val TERRAIN_LENGTH = 100
  val GARBAGE_COLLECTION_UPDATE_PERIOD = 30 // the period in which the object send its current location to the garbage collector
  val GARBAGE_COLLECTION_EXECUTION_PERIOD = 60 // the period in which the garbage collector executes
  val PLAYER_VIEW_RADIUS = 400
  val TERRAIN_NATURE = TerrainSeedData.TerrainType.Perlin

}








object TerrainCoordinateMapping{
  def getTerrainCoordinateForObjectPosition(x : Double, z : Double): Coordinates = {
    val terrain_length = TerrainGeneratorSetting.TERRAIN_LENGTH
    val terrain_x:Int = math.floor(x / terrain_length).toInt * terrain_length
    val terrain_z:Int = math.floor(z / terrain_length).toInt * terrain_length
    return new Coordinates(terrain_x, 0, terrain_z)
  }

  def getTerrainCoordinateForObjectPosition(position: Coordinates): Coordinates = {
    val x = position.x
    val z = position.z
    return getTerrainCoordinateForObjectPosition(x, z)
  }

  def findCoordinatesOfTerrainsThatNeedToBeGenerated(center: Coordinates, radius: Int): Set[Coordinates] = {
    val terrain_length = TerrainGeneratorSetting.TERRAIN_LENGTH
    val x:Double = center.x
    val z:Double = center.z

    var neighbour_terrains = Set[Coordinates]()

    val start_position = getTerrainCoordinateForObjectPosition(x-radius, z-radius)
    val end_position = getTerrainCoordinateForObjectPosition(x+radius, z+radius) // this ensures the last position is considered by the loop, fixes the edge case

    for{
      i <- start_position.x to end_position.x by terrain_length
      j <- start_position.z to end_position.z by terrain_length
    }{
      neighbour_terrains += getTerrainCoordinateForObjectPosition(i, j)
    }

    return neighbour_terrains
  }
}