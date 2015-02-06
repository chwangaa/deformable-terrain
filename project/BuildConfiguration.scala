import improbable.build._
import improbable.build.unity._
import improbable.build.fabric._
import sbt._

object BuildConfiguration extends improbable.build.ImprobableBuild(
  projectName = "superseedling",
  organisation = "improbable",
  version = Versions.coreLibraryVersion,
  buildSettings = Seq(FabricBuildSettings(), UnityPlayerProject()),
  gameDependencies = List(CoreLibrary)
)

object Versions {
  val coreLibraryVersion = readVersionFrom("project/coreLibrary.version")
}
