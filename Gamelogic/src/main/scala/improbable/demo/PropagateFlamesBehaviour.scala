package improbable.demo

import improbable.papi.world.messaging.CustomMsg

import scala.concurrent.duration._

case object Ignite extends CustomMsg

trait PropagateFlamesBehaviour extends PropagateFlamesBehaviourBase {


  if(state.isOnFire) {
    ignite()
  }

  world.messaging.onReceive {
    case Ignite =>
      if (!state.isOnFire) {
        ignite()
      }
  }

  def ignite(): Unit = {
    state.update.isOnFire(true).finishAndSend()
    world.timing.every(500.milliseconds) {
      spreadFire()
    }
  }

  def spreadFire(): Unit = {
    world.entities.find(entity.position, 5.0f).foreach {
      otherEntity =>
        world.messaging.sendToEntity(otherEntity.entityId, Ignite)
    }
  }
}