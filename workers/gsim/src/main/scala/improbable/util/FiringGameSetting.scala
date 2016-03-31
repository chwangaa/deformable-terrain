package improbable.util

import improbable.damage.TeamStateData

/**
  * Created by chihang on 31/03/2016.
  */
object FiringGameSetting {
  def getColorFromTeam(team:TeamStateData.Team.Value ):java.awt.Color = team match{
    case TeamStateData.Team.BLUE =>
      java.awt.Color.BLUE
    case TeamStateData.Team.RED =>
      java.awt.Color.RED
    case TeamStateData.Team.GREEN =>
      java.awt.Color.GREEN
    case _ =>
      java.awt.Color.WHITE
  }

  def getTeamTag(team:TeamStateData.Team.Value ):String = team match{
    case TeamStateData.Team.BLUE =>
      "BLUE"
    case TeamStateData.Team.RED =>
      "RED"
    case TeamStateData.Team.GREEN =>
      "GREEN"
    case _ =>
      "WHITE"
  }

  val PLAYER_BULLET_RADIUS = 20
  val INITIAL_HEALTH = 25
  val BULLET_DAMAGE_VALUE = 25
  val NUMBER_OF_BOTS = 50
}
