import improbable.apps.{CubeSpawnerDescriptor, PlayerLifeCycleManagerDescriptor}
import improbable.corelib.launcher.DefaultLaunchConfig

class SuperSeedlingLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(PlayerLifeCycleManagerDescriptor, CubeSpawnerDescriptor),
  dynamicallySpoolUpEngines
)

object ManualEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(false)

object AutomaticEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(true)
