using System;
using System.Linq;
using Common;

namespace TeamYojig
{
    public class TargetMan : IPlayerRole
    {
        public void DoAction(Game game, Player self)
        {
            var ball = game.Ball;
            var goal = Field.EnemyGoal.Center;

            if (ball.Owner == self)
            {
                var closestToGoal = goal.GetClosest(game.Enemy);
                if (closestToGoal.GetDistanceTo(goal) > Field.EnemyGoal.Height*0.7)
                {
                    self.ActionShootGoal();
                    return;
                }

                var enemyToGoal = game.Enemy.Players
                    .Where(pl => pl.Position.X > self.Position.X - 40)
                    .Select(pl => new { pl, dist = pl.GetDistanceTo(self) })
                    .OrderBy(tp => tp.dist)
                    .FirstOrDefault();

                if (enemyToGoal != null && enemyToGoal.dist < 120)
                {
                    self.ActionShoot(game.GetSmartShoot(self));
                    return;
                }

                var closestEnemy = self.GetClosest(game.Enemy);
                if (self.GetDistanceTo(closestEnemy) < 50)
                {
                    self.ActionShoot(game.GetSmartShoot(self));
                    return;
                }

                self.ActionGo(goal);
                return;
            }

            if (Utility.TryField(game, self))
                return;

            var centerX = goal.X*0.8;

            IPosition target;
            if (ball.Position.X > goal.X * 0.6)
            {
                var targetX = Math.Max(centerX, ball.Position.X * 0.95);
                var mul = ball.Position.Y < goal.Y ? 1 : -1;
                var targetY = goal.Y + Field.EnemyGoal.Height * 0.6 * mul;

                target = new Vector(targetX, targetY);
            }
            else
            {
                target = new Vector(centerX, goal.Y);
            }


            self.ActionGo(target);
        }
    }
}