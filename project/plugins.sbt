resolvers := Seq(
  "Improbable Nexus External Releases" at s"http://172.16.2.101:8081/content/repositories/releases/",
  "Improbable Nexus External Snapshots" at s"http://172.16.2.101:8081/content/repositories/snapshots/",
  "Spray Repository" at "http://repo.spray.io/"
) ++ resolvers.value

val everythingVersion = IO.read(file("project/everything.version")).trim

addSbtPlugin("improbable" % "fabric-sdk-build-plugin" % everythingVersion)
addSbtPlugin("improbable" % "unity-sdk-build-plugin" % everythingVersion)
