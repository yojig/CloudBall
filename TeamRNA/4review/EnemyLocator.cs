using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Common;

namespace TeamRNA.ForReview
{
    public static class PlayersLocator
    {
        private static bool leftgoal = Field.MyGoal.X > Field.EnemyGoal.X;

        public static Player GetClosestInterceptor(this Player player, Team enemies)
        {
            var line = Line.One(player.Position, Field.EnemyGoal.Center);
            return (from ep in enemies.Players
                    let y1 = line.K * ep.Position.X + line.B
                    let dist = player.GetDistanceTo(ep.Position)
                    where Math.Abs(ep.Position.Y - y1) < 100
                    where ep.GetDistanceTo(Field.EnemyGoal.Center) < dist
                    //where dist < 300
                    orderby dist descending
                    select ep).FirstOrDefault();
        }

        public static Vector GetDirection(this Vector vector)
        {
            vector.Normalize();
            return vector;
        }

        public static IEnumerable<Player> GetClosestInterceptors(this Player player, Team enemies, int dist)
        {
            var line = Line.One(player.Position, Field.EnemyGoal.Center);
            return (from ep in enemies.Players
                    let eloc = ep.Position + ep.Velocity
                    let y1 = line.K * eloc.X + line.B
                    where Math.Abs(eloc.Y - y1) < 100
                    //where ep.GetDistanceTo(Field.EnemyGoal.Center) < dist
                    where player.GetDistanceTo(eloc) < dist
                    //orderby player.GetDistanceTo(ep) descending
                    select ep);
        }

        public static Player GetClosestUncovered(this Player player, Team friends, Team enemies)
        {
            var partner = (from p in friends.Players
                           where p != player
                           where p.PlayerType != PlayerType.Keeper
                           let curdist = p.GetDistanceTo(Field.EnemyGoal.Center)
                           let dist = player.GetDistanceTo(p)
                           //let plrdist = player.Position.GetDistanceTo(Field.EnemyGoal.Center)
                           //where curdist < plrdist
                           //where dist < 500
                           orderby curdist descending, dist descending
                           let line = Line.One(p, player)
                           let es = from ep in enemies.Players
                                    let y1 = line.K * ep.Position.X + line.B
                                    where Math.Abs(ep.Position.Y - y1) > 50
                                    where Math.Abs(ep.GetDistanceTo(p) - dist) > 50
                                    select ep
                           where !es.Any()
                           select p).FirstOrDefault();
            return partner;
        }

        public static IList<Player> InFrontOf(this Team enemies, Player player)
        {
            return (from p in enemies.Players
                     let k = Line.K0(p.Position, player.Position)
                     where k > -1d / 2d && k < 1d / 2d
                     where (!leftgoal && (p.Position.X > player.Position.X))
                        || (leftgoal && (p.Position.X < player.Position.X))
                     select p).ToArray();
        }
        public static IList<Player> BehindOf(this Team enemies, Player player)
        {
            return (from p in enemies.Players
                    let k = Line.K0(p.Position, player.Position)
                    where k < -1d / 2d || k > 1d / 2d
                       || (leftgoal && (p.Position.X > player.Position.X))
                       || (!leftgoal && (p.Position.X < player.Position.X))
                    select p).ToArray();
        }
    }
}
