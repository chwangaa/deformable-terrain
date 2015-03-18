import improbable.corelib.launcher.DefaultLaunchConfig
import improbable.game.{PlayerLifeCycleManagerDescriptor, TreeSpawnerDescriptor}

object ManualEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(false)

object AutomaticEngineStartupLaunchConfig extends SuperSeedlingLaunchConfig(true)

class SuperSeedlingLaunchConfig(dynamicallySpoolUpEngines: Boolean) extends DefaultLaunchConfig(
  Seq(PlayerLifeCycleManagerDescriptor, TreeSpawnerDescriptor),
  dynamicallySpoolUpEngines
)
