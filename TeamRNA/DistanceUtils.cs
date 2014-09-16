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

        public static double GetEstimatedDistance(this Player pos1, IPosition pos2)
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
    }
}