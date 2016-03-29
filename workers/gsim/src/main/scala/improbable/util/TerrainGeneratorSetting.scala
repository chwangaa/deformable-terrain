package improbable.util

import improbable.papi.entity.EntityPrefab

object TerrainGeneratorSetting {

  val TERRAIN_LENGTH = 100
  val GARBAGE_COLLECTION_UPDATE_PERIOD = 30 // the period in which the object send its current location to the garbage collector
  val GARBAGE_COLLECTION_EXECUTION_PERIOD = 60 // the period in which the garbage collector executes
  val PLAYER_VIEW_RADIUS = 200
}
