using Common; //Common contains all the classes you need to play CloudBall.

namespace TeamOne
{
    /// <summary>
    /// This is the example team TeamOne.
    /// 
    /// It is a very basic team, all the players act the same, and they allways go for the ball.
    /// </summary>
    public class TeamOne : ITeam                                                            //All teams submitted has to inherit from ITeam
    {
        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            foreach (Player player in myTeam.Players)                                       //Loop over all players in my team.
            {
                Player closestEnemy = player.GetClosest(enemyTeam);                         //Gets the closest enemy player.

                if (ball.Owner == player)                                                   //If this player has the ball.
                    player.ActionShootGoal();                                               //Tell this player to shoot towards the goal, at maximum strength!
                else if (player.CanPickUpBall(ball))
                    player.ActionPickUpBall();
                else player.ActionGo(ball.Position);                                                 //Worst case just go for the ball
            }                                                           //Return myTeam with the teams actions.
        }
    }
}