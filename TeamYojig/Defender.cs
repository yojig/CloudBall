using System.Linq;
using Common;

namespace TeamYojig
{
    public class Defender : IPlayerRole
    {
        public void DoAction(Game game, Player self)
        {
            var ball = game.Ball;
            var goal = Field.EnemyGoal.Center;

            if (ball.Owner == self)
            {
                var closestToGoal = goal.GetClosest(game.Enemy);
                if (closestToGoal.GetDistanceTo(goal) > Field.EnemyGoal.Height)
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

                var pos = self.GetCenterShootout(game.Enemy);
                if (self.Position.X < Field.Borders.Center.X * 0.75)
                {
                    self.ActionShoot(pos);
                    return;
                }

                self.ActionGo(pos);
                return;
            }

            if (Utility.TryField(game, self))
                return;

            self.ActionGo(Field.MyGoal.GetClosest(game.Enemy));
        }
    }
}