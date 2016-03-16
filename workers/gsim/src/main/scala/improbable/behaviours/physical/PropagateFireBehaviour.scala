package improbable.physical

import improbable.Cancellable
import improbable.papi.world.World
import improbable.papi.entity.{EntityBehaviour, Entity}
import improbable.papi.world.messaging.CustomMsg

import scala.concurrent.duration._

case object Ignite extends CustomMsg
case object Extinguish extends CustomMsg

class PropagateFireBehaviour(fire: FireWriter, world: World, entity: Entity) extends EntityBehaviour{

  var cancellable: Cancellable = null

  if (fire.onFire) {
    ignite()
  }

  world.messaging.onReceive {
    case Ignite =>
      if (!fire.onFire) {
        ignite()
      }
    case Extinguish =>
      if (fire.onFire) {
        extinguish()
        cancellable.cancel()
      }
  }

  def ignite(): Unit = {
    fire.update.onFire(true).finishAndSend()
    cancellable = world.timing.every(1000.milliseconds) {
      spreadFire()
    }
    world.timing.after(20000.milliseconds){
      entity.destroy()
    }
  }

  def extinguish(): Unit = {
    fire.update.onFire(false).finishAndSend()
  }

  def spreadFire(): Unit = {
    world.entities.find(entity.position, 5.0f).foreach {
      otherEntity =>
        world.messaging.sendToEntity(otherEntity.entityId, Ignite)
    }
  }
}