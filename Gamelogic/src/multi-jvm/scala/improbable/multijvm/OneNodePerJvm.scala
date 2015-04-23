package improbable.multijvm

import improbable.configuration.DeploymentConfigurationProperties
import improbable.core.deployment.ModuleProps
import improbable.core.terrain.Terrains
import improbable.deployment.config.{DeploymentSettings, EngineSettings, GameSetupSettings}
import improbable.multijvm.OneNodePerJvm._
import improbable.papi.world.{Terrain, WorldSettings}
import improbable.pinnacle.worldapps.spawning.BoxStackAppDescriptor
import improbable.terrain.DefaultTerrains
import scala.concurrent.duration._

/**
 * Copyright (c) 2014 All Right Reserved, Improbable Worlds Ltd.
 * Date: 07/10/2014
 * Summary:
 */

object DefaultWorldSettings extends WorldSettings(
  List(PlayerLifeCycleManagerDescriptor, BoxStackAppDescriptor),
  Terrains.largeFlatWorld
)


abstract class OneNodePerJvm(nodeName: String, withRouter: Boolean = false) extends App {
  System.setProperty(DeploymentConfigurationProperties.PROPERTY_UNIQUE_NODE_NAME, nodeName)
  protected val enginesToStart = Nil

  protected val gameSetupSettings = GameSetupSettings(
    WorldSettings(DefaultWorldSettings.apps, DefaultTerrains.VISUAL_TEST_WORLD_TERRAIN),
    deploymentSettings = DeploymentSettings(dynamicallySpoolUpEngines = false, enginesToStart)
  )

  private val gameDefinitionLoader = new GameDefinitionLoader()
  protected val availableGames = gameDefinitionLoader.load()
  protected val terrain = gameSetupSettings.worldSettings.terrain
  protected val constraintToEngineMapping = if (gameSetupSettings.deploymentSettings.dynamicallySpoolUpEngines) {
    gameSetupSettings.engineSettings.constraintToEngineMapping
  } else {
    EngineSettings.NO_CONSTRAINT_TO_ENGINE_MAPPING
  }
  protected val availableApps = availableGames.flatMap(_.definedApps)

  private val moduleLauncher = new SingleMachineMultipleJvmModuleLauncher(createRouter = withRouter, MODULE_LIFE_DURATION)


  protected def startModules(moduleProps: ModuleProps*): Unit = {
    moduleLauncher.startModules(moduleProps: _*)
  }
}

object OneNodePerJvm {
  private val MODULE_LIFE_DURATION = 2.minute
}