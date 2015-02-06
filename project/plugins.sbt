resolvers := Seq(
  "Improbable Nexus External Releases" at s"http://172.16.2.101:8081/content/repositories/releases/",
  "Improbable Nexus External Snapshots" at s"http://172.16.2.101:8081/content/repositories/snapshots/",
  "Spray Repository" at "http://repo.spray.io/"
) ++ resolvers.value

addSbtPlugin("improbable" % "core-library-build-plugin" % IO.read(file("project/coreLibrary.version")).trim)

addSbtPlugin("improbable" % "scala-sdk-build-plugin" % "0.0.10")

addSbtPlugin("improbable" % "unity-sdk-build-plugin" % "0.0.9")

addSbtPlugin("improbable" % "build-plugin" % "0.0.5")