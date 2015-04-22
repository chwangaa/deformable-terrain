import improbable.configuration.DeploymentConfigurationProperties
import improbable.corelib.launcher.ShutdownAfterInput
import improbable.dapi.Launcher
import improbable.unity.fabric.engine.DownloadableClientEngineDescriptor

object ManualEngineSpoolUpGameLauncher extends SuperSeedlingGameLauncher(ManualEngineStartupLaunchConfig) with App with ShutdownAfterInput

object AutoEngineSpoolUpGameLauncher extends SuperSeedlingGameLauncher(AutomaticEngineStartupLaunchConfig) with App with ShutdownAfterInput

object VisibleClient extends DownloadableClientEngineDescriptor(withGui = true)

class SuperSeedlingGameLauncher(gameSetupSettings: SuperSeedlingLaunchConfig) {
  System.setProperty(DeploymentConfigurationProperties.PROPERTY_IS_PRODUCTION, true.toString)
  System.setProperty("game_engines_to_start", "VisibleClient@0,0,0")
  Launcher.startGame(gameSetupSettings)
}