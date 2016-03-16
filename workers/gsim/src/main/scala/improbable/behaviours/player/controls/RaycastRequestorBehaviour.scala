package improbable.behaviours.player.controls

import com.typesafe.scalalogging.Logger
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.physical.RaycastRequestWriter

/**
  * Created by chihang on 15/03/2016.
  */
class RaycastRequestorBehaviour(raycastRequestWriter: RaycastRequestWriter, logger: Logger) extends EntityBehaviour with RaycastRequestorInterface{

  def performRaycast(): Unit = {
    logger.info("going to send the perform rayCast request")
    raycastRequestWriter.update.triggerRaycastRequested().finishAndSend()
    logger.info("perform rayCast request sent")
  }
}
