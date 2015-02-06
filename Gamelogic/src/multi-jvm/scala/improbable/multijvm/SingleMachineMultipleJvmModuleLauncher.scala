package improbable.multijvm

import improbable.IpResolver
import improbable.core.deployment.{ModuleNode, ModuleProps}

import scala.concurrent.duration.Duration

/**
 * Copyright (c) 2014 All Right Reserved, Improbable Worlds Ltd.
 * Date: 07/10/2014
 * Summary: 
 */
class SingleMachineMultipleJvmModuleLauncher(createRouter: Boolean, shutdownWithin: Duration) {

  private val moduleNode = {
    if (createRouter) {
      new ModuleNode(None)
    } else {
      new ModuleNode(Some(IpResolver.getExternalIp))
    }
  }

  def startModules(moduleProps: ModuleProps*): Unit = {
    moduleProps.foreach(moduleNode.startModule)
    Thread.sleep(shutdownWithin.toMillis)
    System.exit(0)
  }
}
