import improbable.build._
import improbable.build.unity._
import improbable.build.fabric._
import sbt._

object BuildConfiguration extends improbable.build.ImprobableBuild(
  projectName = "demonstration",
  organisation = "improbable",
  version = "1",
  buildSettings = Seq(FabricBuildSettings(), UnityPlayerProject()),
  gameDependencies = List(CoreLibrary)
)

object Versions {
  val coreLibraryVersion = readVersionFrom("project/coreLibrary.version")
}
