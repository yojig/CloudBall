using System;
using System.Linq;
using Common;

namespace TeamYojig
{
    public class LowerWinger : IPlayerRole
    {
        public void DoAction(Game game, Player self)
        {
            var ball = game.Ball;
            var goal = Field.EnemyGoal.Center;

            if (ball.Owner == self)
            {
                var closestToGoal = goal.GetClosest(game.Enemy);
                if (closestToGoal.GetDistanceTo(goal) > Field.EnemyGoal.Height * 0.8)
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

                self.ActionGo(Field.EnemyGoal.Bottom);
                return;
            }

            if (Utility.TryField(game, self))
                return;

            IPosition target;
            if (game.InAttack)
            {
                if (ball.Position.X > goal.X*0.6)
                {
                    var centerX = goal.X * 0.82;
                    var targetX = Math.Max(centerX, ball.Position.X * 0.95);
                    var targetY = goal.Y + Field.EnemyGoal.Height * 0.75;

                    target = new Vector(targetX, targetY);
                }
                else
                {
                    target = new Vector(Field.Borders.Bottom.X, Field.Borders.Bottom.Y - Field.MyGoal.Height * 0.7);
                }
            }
            else
            {
                var closestLowerPlayers = game.Enemy.Players
                                              .Where(pl => pl.Position.Y > Field.Borders.Center.Y)
                                              .Select(pl => new { pl, dist = pl.GetDistanceTo(Field.MyGoal.Bottom) })
                                              .OrderBy(tp => tp.dist);

                if (closestLowerPlayers.Count() > 1)
                {
                    target = closestLowerPlayers.Skip(1).First().pl;
                }
                else
                {
                    target = new Vector(Field.Borders.Bottom.X, Field.Borders.Bottom.Y - Field.MyGoal.Height * 0.7);
                }
            }

            self.ActionGo(target);
        }
    }
}