package improbable.apps

import com.typesafe.scalalogging.Logger
import improbable.math.Vector3d
import improbable.natures.BotNature
import improbable.papi.world.AppWorld
import improbable.papi.worldapp.{WorldAppLifecycle, WorldApp}

class CubeSpawner(val world: AppWorld,
                  override val logger: Logger,
                  val lifecycle: WorldAppLifecycle) extends WorldApp {

  spawnCubes()

  private def spawnCubes(): Unit = {
    Range(1, 50).foreach {
      _ =>
        Thread.sleep(200)
        world.entities.spawnEntity(BotNature((Vector3d.unitY * 20.0f + Vector3d.unitX * 10.0f).toCoordinates))
    }
  }
}
