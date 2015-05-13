import improbable.apps.{TreeSpawnerDescriptor, CubeSpawnerDescriptor, PlayerLifeCycleManagerDescriptor}
import improbable.corelib.launcher.DefaultLaunchConfig

class DemonstrationLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(PlayerLifeCycleManagerDescriptor, CubeSpawnerDescriptor, TreeSpawnerDescriptor),
  dynamicallySpoolUpEngines
)

object ManualEngineStartupLaunchConfig extends DemonstrationLaunchConfig(false)

object AutomaticEngineStartupLaunchConfig extends DemonstrationLaunchConfig(true)
