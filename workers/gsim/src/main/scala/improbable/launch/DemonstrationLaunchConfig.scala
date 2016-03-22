package improbable.launch

import improbable.apps._
import improbable.corelib.launcher.DefaultConstraintEngineDescriptorResolver
import improbable.dapi.LaunchConfig
import improbable.launch.bridgesettings.DemonstrationBridgeSettingsResolver

class DemonstrationLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends {} with LaunchConfig(
  DemonstrationWorldAppList.apps,
  dynamicallySpoolUpEngines,
  DemonstrationBridgeSettingsResolver,
  DefaultConstraintEngineDescriptorResolver
)

object ManualEngineStartupLaunchConfig extends DemonstrationLaunchConfig(dynamicallySpoolUpEngines = false)

object AutomaticEngineStartupLaunchConfig extends DemonstrationLaunchConfig(dynamicallySpoolUpEngines = true)
