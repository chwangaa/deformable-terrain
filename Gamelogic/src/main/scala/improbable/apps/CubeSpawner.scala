package improbable.apps

import com.typesafe.scalalogging.Logger
import improbable.math.Vector3d
import improbable.natures.BotNature
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.{WorldAppLifecycle, WorldApp}
import scala.concurrent.duration._
import scala.language.postfixOps

class CubeSpawner(world: AppWorld,
                  logger: Logger,
                  lifecycle: WorldAppLifecycle) extends WorldApp {

  spawnCubes()

  private def spawnCubes(): Unit = {
    Range.inclusive(1, 50).foreach {
      i =>
        world.timing.after((200 * i) millis) {
          world.entities.spawnEntity(BotNature((Vector3d.unitY * 20.0f + Vector3d.unitX * 10.0f).toCoordinates))
        }
    }
  }
}
