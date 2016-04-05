package improbable.util

import improbable.terrainchunk.TerrainUsageState

import scala.util.Random

/**
  * Created by chihang on 05/04/2016.
  */


object TerrainUsageTypeUtil{
  def getRandomTerrainType(): TerrainUsageState.TerrainUsageType.Value = {
    val type_ = Random.nextInt(3)
    type_ match{
      case 0 =>
        return TerrainUsageState.TerrainUsageType.Residence
      case _ =>
        return TerrainUsageState.TerrainUsageType.None
    }
  }

}