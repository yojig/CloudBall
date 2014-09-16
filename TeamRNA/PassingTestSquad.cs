using System.Linq;
using Common;

namespace TeamRNA
{
    public class PassingTestSquad : ITeam
    {
        private PlayerType shooter = PlayerType.CenterForward;

        private int currentIter;
        private int counter;

        private bool setupDone;

        public PassingTestSquad()
        {
            currentIter = 8;
        }

        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            var setup = myTeam.Players.First(pl => pl.PlayerType == PlayerType.CenterForward);
            var pl1 = myTeam.Players.First(pl => pl.PlayerType == PlayerType.RightForward);
            var pl2 = myTeam.Players.First(pl => pl.PlayerType == PlayerType.LeftForward);

            if (!setupDone)
            {
                if (setup.CanPickUpBall(ball))
                {
                    setup.ActionPickUpBall();
                    return;
                }
                if (ball.Owner == setup)
                {
                    setup.ActionShoot(pl1, 8);
                    setupDone = true;
                    return;
                }
                setup.ActionGo(ball);
                return;
            }


            if (counter >= 9)
            {
                ++currentIter;
                counter = 0;
            }
            
            var currentPower = Constants.PlayerMaxShootStr*currentIter/10;

            if (ball.Owner == pl1)
            {
                pl1.ActionShoot(pl2, currentPower);
                shooter = pl1.PlayerType;
                myTeam.DevMessage += string.Format("\r\nshooting with power {0}", currentPower);
                ++counter;
                return;
            }
            if (ball.Owner == pl2)
            {
                pl2.ActionShoot(pl1, currentPower);
                shooter = pl2.PlayerType;
                myTeam.DevMessage += string.Format("\r\nshooting with power {0}", currentPower);
                ++counter;
                return;
            }

            if (ball.Velocity.Length < 20)
            {
                if (shooter != pl1.PlayerType && pl1.CanPickUpBall(ball))
                {
                    pl1.ActionPickUpBall();
                    return;
                }
                if (shooter != pl2.PlayerType && pl2.CanPickUpBall(ball))
                {
                    pl2.ActionPickUpBall();
                    return;
                }

                if (pl1.GetDistanceTo(ball) < pl2.GetDistanceTo(ball))
                {
                    if (shooter != pl1.PlayerType)
                    {
                        pl1.ActionGo(ball);
                        return;
                    }
                }
                else
                {
                    if (shooter != pl2.PlayerType)
                    {
                        pl2.ActionGo(ball);
                        return;
                    } 
                }
            }


        }
    }
}