package improbable.apps

import improbable.math.Vector3d
import improbable.natures.BotNature
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.WorldApp

import scala.concurrent.duration._
import scala.language.postfixOps

class CubeSpawner(appWorld: AppWorld) extends WorldApp {

  spawnCubes()

  private def spawnCubes(): Unit = {
    Range.inclusive(1, 50).foreach {
      i =>
        appWorld.timing.after((200 * i) millis) {
          appWorld.entities.spawnEntity(BotNature((Vector3d.unitY * 20.0f + Vector3d.unitX * 10.0f).toCoordinates))
        }
    }
  }
}
