using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace TeamRNA.ForReview
{
    public static class MadForward
    {
        public static Vector InterceptAttack(Player player, Ball ball)//, Team friends, Team enemies)
        {
            Team enemies = ball.Owner.Team;
            return ball.Position;
        }

        public static Vector PickupBall(Player player, Ball ball)//, Team friends, Team enemies)
        {
            return ball.Position;
        }

        public static Vector CoverAttacker(Player player, Team enemies)
        {
            Team friends = player.Team;

            switch (player.PlayerType)
            {
                case PlayerType.LeftForward:
                    return Field.EnemyGoal.Left;
                case PlayerType.CenterForward:
                    return Field.EnemyGoal.Center;
                case PlayerType.RightForward:
                    return Field.EnemyGoal.Right;
                default:
                    // todo: implement
                    return player.Position;
            }
        }
        public static Vector AttackGoal(Ball ball, Team enemies)
        {
            Player player = ball.Owner;
            Team friends = player.Team;
            return Field.EnemyGoal.Center;
        }
        public static Vector ShootGoal(Ball ball, Team enemies)
        {
            Player player = ball.Owner;
            Team friends = player.Team;
            double[] kks = (from ep in enemies.Players
                            let ktop = Line.K0(player.Position, Field.EnemyGoal.Top)
                            let kbot = Line.K0(player.Position, Field.EnemyGoal.Bottom)
                            let kep = Line.K0(player.Position, ep.Position)
                            where kep >= Math.Min(ktop, kbot) && kep <= Math.Max(ktop, kbot)
                            where ep.GetDistanceTo(Field.EnemyGoal.Center) < player.GetDistanceTo(Field.EnemyGoal.Center)
                            orderby kep
                            select kep).ToArray();
            return Field.EnemyGoal.Center;
        }
    }
}
