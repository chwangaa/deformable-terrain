package improbable.apps

import improbable.math.Vector3d
import improbable.natures.BotNature
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.WorldApp
import improbable.math.Coordinates

import scala.concurrent.duration._
import scala.language.postfixOps

class CubeSpawner(appWorld: AppWorld) extends WorldApp {

  spawnCubes()

  private def spawnCubes(): Unit = {

    appWorld.entities.spawnEntity(BotNature(Coordinates(5, 1, 5), onFire = true))

    Range.inclusive(1, 100).foreach {
      i =>
        appWorld.timing.after((20 * i) millis) {
          appWorld.entities.spawnEntity(BotNature((Vector3d.unitY * 20.0f + Vector3d.unitX * 10.0f).toCoordinates))
        }
    }
  }
}
