import improbable.configuration.DeploymentConfigurationProperties
import improbable.corelib.launcher.ShutdownAfterInput
import improbable.dapi.Launcher
import improbable.unity.fabric.engine.DownloadableClientEngineDescriptor

import scala.io.StdIn

object ManualEngineSpoolUpGameLauncher extends DemonstrationGameLauncher(ManualEngineStartupLaunchConfig) with App with ShutdownAfterInput

object AutoEngineSpoolUpGameLauncher extends DemonstrationGameLauncher(AutomaticEngineStartupLaunchConfig) with App with ShutdownAfterInput

object Demonstration extends DemonstrationGameLauncher(AutomaticEngineStartupLaunchConfig) with App with ShutdownAfterInput

object VisibleClient extends DownloadableClientEngineDescriptor(withGui = true)

class DemonstrationGameLauncher(gameSetupSettings: DemonstrationLaunchConfig) {
  Launcher.startGame(gameSetupSettings,
    "--engine_startup_retries=3",
    "--game_engines_to_start=VisibleClient@0,0,0")
}