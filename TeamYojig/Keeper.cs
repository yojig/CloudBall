using System;
using Common;

namespace TeamYojig
{
    public class Keeper : IPlayerRole
    {
        public void DoAction(Game game, Player self)
        {
            var ball = game.Ball;
            var closestEnemy = self.GetClosest(game.Enemy);

            if (ball.Owner == self)
            {
                var sweeper = game.My.Sweeper();

                if (sweeper.IsSafe(game, self))
                {
                    self.ActionShoot(sweeper, 50);
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

            //if (ball.GetDistanceTo(Field.MyGoal) > 800)
            //{
            //    self.ActionGo(new Vector(Field.MyGoal.Height/2, Field.MyGoal.Center.Y));
            //}
            //else
            //{
                //get ball target
                var distanceCoeff = ball.Velocity.Length;// / ball.GetDistanceTo(self);
                var target = ball.Position.Y + ball.Velocity.Y * distanceCoeff;

                var y = Math.Max(Math.Min(target, Field.MyGoal.Bottom.Y), Field.MyGoal.Top.Y);

                var lenCoeff = ball.GetDistanceTo(Field.MyGoal.Center) * 2 / Field.MyGoal.Height;
                var x = Math.Min(ball.Position.X/lenCoeff, Field.MyGoal.Height / 2);

                self.ActionGo(new Vector(x, y));
            //}
        }
    }
}
