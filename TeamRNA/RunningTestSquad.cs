using System.Linq;
using Common;

namespace TeamRNA
{
    public class RunningTestSquad : ITeam
    {
        private bool maxReached = true;
        private double prevVelocity;
        private IPosition point;

        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            var pl1 = myTeam.Players.First(pl => pl.PlayerType == PlayerType.RightDefender);

            myTeam.DevMessage = string.Format("speed {0}", pl1.Velocity.Length);

            if (maxReached)
            {
                if (pl1.Velocity.Length > 0.05)
                {
                    pl1.ActionWait();
                    return;
                }

                myTeam.DevMessage += "\r\n0.05 minReached";
                maxReached = false;
                point = pl1.Position.X < Field.Borders.Center.X ? Field.EnemyGoal : Field.MyGoal;
            }

            if (pl1.Velocity.Length > 3 * 0.95)
            {
                myTeam.DevMessage += "\r\n0.95 maxReached";
                maxReached = true;
                prevVelocity = 0;
                pl1.ActionWait();
            }
            else
            {
                pl1.ActionGo(point);
                prevVelocity = pl1.Velocity.Length;
            }
        }
    }
}