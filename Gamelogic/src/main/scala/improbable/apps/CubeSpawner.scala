package improbable.apps

import improbable.math.Vector3d
import improbable.natures.Bot
import improbable.papi.worldapp.WorldApp

trait CubeSpawner extends WorldApp {

  spawnCubes()

  private def spawnCubes(): Unit = {
    Range(1, 50).foreach {
      _ =>
        Thread.sleep(200)
        world.entities.spawnEntity(Bot((Vector3d.unitY * 20.0f + Vector3d.unitX * 10.0f).toCoordinates))
    }
  }
}