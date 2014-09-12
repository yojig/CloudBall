using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace TeamYojig
{
    public static class Utility
    {
        public static IEnumerable<Player> FieldPlayers(this Team team)
        {
            return team.Players.Where(pl => pl.PlayerType != PlayerType.Keeper);
        }

        public static T ClosestTo<T>(this IPosition pos, IEnumerable<T> list)
            where T:IPosition
        {
            return list.OrderBy(item => pos.GetDistanceTo(item)).First();
        }

        public static Player Sweeper(this Team team)
        {
            return team.Players.First(pl => pl.PlayerType == PlayerType.LeftDefender);
        }

        public static Player Defender(this Team team)
        {
            return team.Players.First(pl => pl.PlayerType == PlayerType.CenterForward);
        }

        public static bool IsSafe(this Player player, Game game, Player passer)
        {
            if (game.Enemy.Players.Any(p => p.IsInZone(passer, player)))
                return false;

            return true;
            //return player.GetClosest(game.Enemy).GetDistanceTo(player) > 150;
        }

        public const int SafetyDist = 35;

        public static bool IsInZone(this Player pl, IPosition start, IPosition end)
        {
            var vect = start.Position - end.Position;
            var dist = vect.Length;

            var distToStart = pl.GetDistanceTo(start);
            var distToEnd = pl.GetDistanceTo(end);

            if (dist > distToEnd - SafetyDist || dist > distToStart - SafetyDist)
            {
                return true;
            }

            return false;
        }

        public static IPosition GetCenterShootout(this Player player, Team enemy)
        {
            return new Vector(Field.Borders.Center.X, Field.Borders.Center.Y + (Field.Borders.Center.Y * Dice()));
        }

        public static Vector GetBallNextPosition(this Ball ball)
        {
            var scaledVelocity = ball.Velocity * ball.Velocity.Length * 1.2;
            return ball.Position + scaledVelocity;
        }

        public static IPosition GetSmartShoot(this Game game, Player self)
        {
            var closest = Field.EnemyGoal.GetClosest(game.Enemy);

            float targetY;
            if (closest.Position.Y > Field.EnemyGoal.Center.Y)
            {
                targetY = (closest.Position.Y + Field.EnemyGoal.Top.Y) / 2;
            }
            else
            {
                targetY = (closest.Position.Y + Field.EnemyGoal.Bottom.Y) / 2;
            }

            return new Vector(Field.EnemyGoal.X, targetY);
        }

        public static bool TryGeneral(Game game, Player self)
        {
            if (self.FallenTimer > 0)
            {
                self.ActionWait();
                return true;
            }

            var ball = game.Ball;
            var closestEnemy = self.GetClosest(game.Enemy);

            if (self.CanPickUpBall(ball))
            {
                self.ActionPickUpBall();
                return true;
            }

            if (self.CanTackle(closestEnemy))
            {
                self.ActionTackle(closestEnemy);
                return true;
            }

            return false;
        }

        public static bool TryField(Game game, Player self)
        {
            if (TryGeneral(game, self))
                return true;

            var ball = game.Ball.GetBallNextPosition();

            if (ball.ClosestTo(game.My.FieldPlayers()) == self)
            {
                self.ActionGo(ball);
                return true;
            }

            return false;
        }

        public static bool TryKeeper(Game game, Player self)
        {
            if (TryGeneral(game, self))
                return true;

            var ball = game.Ball.GetBallNextPosition();

            if (self == ball.GetClosest(game.My))
            {
                var fieldClosest = ball.ClosestTo(game.My.FieldPlayers());

                if (fieldClosest.GetDistanceTo(ball) > ball.GetClosest(game.Enemy).GetDistanceTo(ball))
                {
                    self.ActionGo(ball);
//                    self.ActionGo(game.GetNormalToBall(self));
                    return true;
                }
            }

            return false;
        }

        public static Vector GetNormalToBall(this Game game, Player self)
        {
            var scaledVelocity = game.Ball.Velocity*game.Ball.Velocity.Length;

            Vector normalToBall;
            if (game.Ball.Position.Y < self.Position.Y)
            {
                normalToBall = new Vector(scaledVelocity.Y, -scaledVelocity.X);
            }
            else
            {
                normalToBall = new Vector(-scaledVelocity.Y, scaledVelocity.X);
            }

            var normalPosition = self.Position + normalToBall;
            return normalPosition;
        }

        private static readonly Random Rand = new Random();

        public static int Dice()
        {
            return Rand.Next(2) == 0 ? 1 : -1;
        }
    }
}