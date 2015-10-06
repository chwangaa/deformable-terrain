import improbable.corelib.launcher.ShutdownAfterInput
import improbable.dapi.Launcher
import improbable.unity.fabric.engine.DownloadableClientEngineDescriptor

object ManualEngineSpoolUpGameLauncher extends DemonstrationGameLauncher(ManualEngineStartupLaunchConfig) with App with ShutdownAfterInput

object AutoEngineSpoolUpGameLauncher extends DemonstrationGameLauncher(AutomaticEngineStartupLaunchConfig) with App with ShutdownAfterInput

object VisibleClient extends DownloadableClientEngineDescriptor(withGui = true)

class DemonstrationGameLauncher(gameSetupSettings: DemonstrationLaunchConfig, options: String*) {
  val allOptions = options ++ Seq(
    "--engine_startup_retries=3",
    "--game_world_edge_length=5000",
    "--entity_activator=improbable.corelib.entity.CoreLibraryEntityActivator",
    "--resource_based_config_name=one-gsim-one-jvm"
  )
  Launcher.startGame(gameSetupSettings, allOptions: _*)
}
