package improbable.launch

import improbable.dapi.{LaunchConfig, Launcher}

class DemonstrationLauncher(launchConfig: LaunchConfig) {

  val options = Seq(
    "--engine_startup_retries=3",
    "--game_world_edge_length=5000",
    "--entity_activator=improbable.corelib.entity.CoreLibraryEntityActivator",
    "--use_spatial_build_workflow=true",
    "--resource_based_config_name=one-gsim-one-jvm"
  )
  Launcher.startGame(launchConfig, options: _*)

}

object ManualEngineSpoolUpDemonstrationLauncher extends DemonstrationLauncher(ManualEngineStartupLaunchConfig) with App

object AutoEngineSpoolUpDemonstrationLauncher extends DemonstrationLauncher(AutomaticEngineStartupLaunchConfig) with App
