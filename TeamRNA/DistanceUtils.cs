using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace TeamRNA
{
    public static class DistanceUtils
    {
        public static double GetEstimatedBallDistance(this Player pos)
        {
            //straightly add velocity to get knowledge about movement of targets
            var estimatedPos1 = pos.Position + pos.Velocity;
            var estimatedPos2 = Pitch.Ball.Position + Pitch.Ball.Velocity;

            return estimatedPos1.GetDistanceTo(estimatedPos2);
        }

        public static double GetNextTurnDistance(this Player pos1, IPosition pos2)
        {
            //straightly add velocity to get knowledge about movement of targets
            var estimatedPos1 = pos1.Position + pos1.Velocity;

            return estimatedPos1.GetDistanceTo(pos2);
        }

        public static IPosition GetFuturePosition(this Ball ball)
        {
            var scaledVelocity = ball.Velocity * ball.Velocity.Length * 1.2;
            return ball.Position + scaledVelocity;
        }

        public static IPosition GetFuturePosition(this Player player)
        {
            var scaledVelocity = player.Velocity * player.Velocity.Length * 1.2;
            return player.Position + scaledVelocity;
        }

        public static double SafeDistance = 25;
        public static double DefenceDistance = Field.Borders.Width*0.65;

        public static Player ClosestEnemy(this Player self)
        {
            return self.GetClosest(Pitch.Enemy);
        }
        
        public static bool BallInPlayerDirection(this Player pl)
        {
            var sub = (pl.Position - Pitch.Ball.Position);
            var dir = Pitch.Ball.Velocity;

            return (sub.Unit() - dir.Unit()).Length < 1.414213;
        }

        public static Vector GetPlayerWithBallIntersection(this Player pl)
        {
            var vector = GetNormalToVector(pl.Position, Pitch.Ball.Position, Pitch.Ball.Velocity);

            return pl.Position + vector;
        }

        public static Vector GetNormalToVector(Vector pos, Vector ballPos, Vector ballDirection)
        {
            var hypotenuse = (pos - ballPos);
            var dotProduct = hypotenuse.Dot(ballDirection);
            var magnitudeH = Math.Sqrt(hypotenuse.X * hypotenuse.X + hypotenuse.Y * hypotenuse.Y);
            var magnituteD = Math.Sqrt(ballDirection.X * ballDirection.X + ballDirection.Y * ballDirection.Y);

            var cos = dotProduct / (magnitudeH * magnituteD);

            //var angle = Math.Acos(cos);
            var directionLength = hypotenuse.Length * cos;
            var scaledDirection = ballDirection.Unit() * directionLength;

            var result = (ballPos + scaledDirection) - pos;
            return result;
        }

        public static int TurnsToGetBall(this Player pl)
        {
            //if ball is moving from me calculate next player position towards ball
            //if ball is moving to me calc position to normal to know if we can get ball
            //if we can do it - calc towards ball pos in turn when we are reaching normal
            //if not - calc towards ball pos in turn when we are reaching normal

            return 0;
        }

        private static int TurnsToGetToPoint(this Player pl, Vector point)
        {
            var position = pl.Position;
            var velocity = pl.Velocity;

            var turn = 0;

            while ((point - position).Length < velocity.Length)
            {
                var direction = (position - point);
                direction.Normalize();

                velocity = (velocity + direction*Constants.PlayerAccelerationFactor*Constants.PlayerMaxVelocity)*
                           Constants.PlayerSlowDownFactor;
                position = position + velocity;
            }

            return turn;
        }

        public static Dictionary<int, Tuple<Vector, Vector>> BuildBallFuturePosition(int turns)
        {
            var result = new Dictionary<int, Tuple<Vector, Vector>>();
            result[0] = Tuple.Create(Pitch.Ball.Position, Pitch.Ball.Velocity);

            for(var i = 0; i < turns; i++)
                AddBallFuturePos(ref result);
                
            return result;
        }

        private static void AddBallFuturePos(ref Dictionary<int, Tuple<Vector, Vector>> result)
        {
            var last = result.Last();

            var velocity = last.Value.Item2 * Constants.BallSlowDownFactor;
            var position = last.Value.Item1 + velocity;
            //todo: if pos is in border then recalc next position and turn velocity vector

            result[last.Key + 1] = Tuple.Create(position, velocity);
        }
    }
}