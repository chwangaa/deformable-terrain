import improbable.apps.{PlayerLifeCycleManagerDescriptor, TreeSpawnerDescriptor}
import improbable.corelib.launcher.DefaultLaunchConfig

class SuperSeedlingLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(PlayerLifeCycleManagerDescriptor, TreeSpawnerDescriptor),
  dynamicallySpoolUpEngines
)

object ManualEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(false)

object AutomaticEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(true)
