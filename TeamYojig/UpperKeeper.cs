using System;
using Common;

namespace TeamYojig
{
    public class UpperKeeper : IPlayerRole
    {
        public void DoAction(Game game, Player self)
        {
            var ball = game.Ball;

            if (ball.Owner == self)
            {
                var sweeper = game.My.Sweeper();
                var defender = game.My.Defender();

                if (sweeper.IsSafe(game, self))
                {
                    self.ActionShoot(sweeper, self.GetDistanceTo(sweeper) * 1.5f);
                }
                else if (defender.IsSafe(game, self))
                {
                    self.ActionShoot(defender, self.GetDistanceTo(defender) * 1.5f);
                }
                else
                {
                    var pos = self.GetCenterShootout(game.Enemy);
                    self.ActionShoot(pos);
                }

                return;
            }

            if (Utility.TryKeeper(game, self))
                return;

            var ballPos = game.Ball.GetBallNextPosition();

            var ballToGoalVector = ballPos - Field.MyGoal.Center;
            var keeperVectorScale = Field.MyGoal.Height/2/ballToGoalVector.Length;
            var keeperToGoalVector = ballToGoalVector * keeperVectorScale;
            var keeperPosition = Field.MyGoal.Center + keeperToGoalVector;

            self.ActionGo(keeperPosition);
        }
    }
}