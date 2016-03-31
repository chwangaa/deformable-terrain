package improbable.apps

import com.typesafe.scalalogging.Logger
import improbable.behaviours.GenerateNeighbouringTerrainAt
import improbable.damage.TeamStateData
import improbable.math.{Vector3d, Coordinates}
import improbable.natures._
import improbable.papi._
import improbable.papi.engine._
import improbable.papi.world.AppWorld
import improbable.papi.world.entities.EntityFindByTag
import improbable.papi.world.messaging.{EngineDisconnected, EngineConnected}
import improbable.papi.worldapp.{WorldApp, WorldAppLifecycle}
import scala.util.Random
import scala.concurrent.duration._
import scala.language.postfixOps
import improbable.util.FiringGameSetting.NUMBER_OF_BOTS

class TerrainSpawner(appWorld: AppWorld,
                  logger: Logger,
                  lifecycle: WorldAppLifecycle) extends WorldApp {

  initializeTerrainGenerator()
  spawnCubes()

  private def initializeTerrainGenerator(): Unit = {
    val initial_position = Coordinates(0, 0, 0)
    val random_seed = Random.nextLong()
    appWorld.entities.spawnEntity(TerrainGeneratorNature(initial_position, random_seed, 100))

    appWorld.entities.find(EntityFindByTag("TerrainGenerator")).foreach(
      x =>
        appWorld.messaging.sendToEntity(x.entityId, GenerateNeighbouringTerrainAt(initial_position, 500))
    )

  }

  private def spawnCubes(): Unit = {

    Range.inclusive(1, NUMBER_OF_BOTS).foreach {
      i =>
        appWorld.timing.after((200 * i) millis) {
          appWorld.entities.spawnEntity(BotNature(Coordinates(0, 60, 0), team=TeamStateData.Team.BLUE))
          appWorld.entities.spawnEntity(BotNature(Coordinates(0, 60, 0), team=TeamStateData.Team.GREEN))
        }
    }

  }



  // spawn the player
  val UNITY_CLIENT = "UnityClient"
  private var userIdToEntityIdMap = Map[EngineId, EntityId]()

  appWorld.messaging.subscribe {
    case engineConnectedMsg: EngineConnected =>
      engineConnected(engineConnectedMsg)

    case engineDisconnectedMsg: EngineDisconnected =>
      engineDisconnected(engineDisconnectedMsg)
  }

  private def engineConnected(msg: EngineConnected): Unit = {
    msg match {
      // For now use the engineName as the userId.
      case EngineConnected(userId, UNITY_CLIENT, _) =>
        addEntity(userId)
      case _ =>
    }
  }

  private def addEntity(userId: String): Unit = {
    val playerEntityId = appWorld.entities.spawnEntity(PlayerNature(engineId = userId))
    logger.info(s"Spawning Player with userId $userId and entityId $playerEntityId")
    userIdToEntityIdMap += userId -> playerEntityId
  }

  private def engineDisconnected(msg: EngineDisconnected): Unit = {
    msg match {
      case EngineDisconnected(userId, UNITY_CLIENT) =>
        removeUserIdToEntityIdEntry(userId)
      case _ =>
    }
  }

  private def removeUserIdToEntityIdEntry(userId: EngineId) = {
    userIdToEntityIdMap.get(userId) match {
      case Some(id) =>
        appWorld.entities.destroyEntity(id)
        logger.info(s"Destroying player: $userId with entityId $id")
      case None =>
        logger.warn(s"User disconnected but could not find entity id for player: $userId")
    }
  }

}
