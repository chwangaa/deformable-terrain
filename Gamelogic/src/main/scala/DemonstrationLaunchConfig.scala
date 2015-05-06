import improbable.apps.{CubeSpawnerDescriptor, PlayerLifeCycleManagerDescriptor}
import improbable.corelib.launcher.DefaultLaunchConfig

class DemonstrationLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(PlayerLifeCycleManagerDescriptor, CubeSpawnerDescriptor),
  dynamicallySpoolUpEngines
)

object ManualEngineStartupLaunchConfig extends DemonstrationLaunchConfig(false)

object AutomaticEngineStartupLaunchConfig extends DemonstrationLaunchConfig(true)
