package improbable.game

import improbable.entitytemplates.Player
import improbable.papi._
import improbable.papi.engine.EngineId
import improbable.papi.entity.EntityPrefab
import improbable.papi.world.messaging.{EngineConnected, EngineDisconnected}
import improbable.papi.worldapp.WorldApp


trait PlayerLifeCycleManager extends WorldApp {
  import improbable.game.PlayerLifeCycleManager._

  private var userIdToEntityIdMap = Map[EngineId, EntityId]()

  world.messaging.subscribe {
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
      case EngineConnected(userId, JAVASCRIPT, _) =>
        addEntity(userId)
      case _ =>
    }
  }

  private def addEntity(userId: String): Unit = {
    val playerEntityId = Player(engineId = userId).spawn(world, new EntityPrefab("PlayerCube"))
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
        world.entities.destroyEntity(id)
        logger.info(s"Destroying player: $userId with entityId $id")
      case None =>
        logger.warn(s"User disconnected but could not find entity id for player: $userId")
    }
  }
}

object PlayerLifeCycleManager {
  private val UNITY_CLIENT = "UnityClient"
  private val JAVASCRIPT = "JavaScript"
}