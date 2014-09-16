using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using TeamRNA.AttackingRoles;
using TeamRNA.DefensiveRoles;
using TeamRNA.SpecialRoles;

namespace TeamRNA
{
    public class Squad : ITeam
    {
        private static bool inAttack;
        private static readonly Dictionary<PlayerType, IRole> assignedRoles = new Dictionary<PlayerType, IRole>();

        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            Pitch.Assign(myTeam, enemyTeam, ball, matchInfo);

            if (matchInfo.EnemyTeamScored || matchInfo.MyTeamScored)
            {
                Pitch.Log("==============================");
                Pitch.Log("Someone scored, flushing roles");
                Pitch.Log("==============================");
                assignedRoles.Clear();
            }

            SetAttackState();

            if (inAttack)
                AssignAttackRoles();
            else
                AssignDefenceRoles();


            foreach (var assignedRole in assignedRoles)
                assignedRole.Value.DoAction();
        }

        private static void AssignAttackRoles()
        {
            if (Pitch.Ball.Owner != null && Pitch.Ball.Owner.Team.Equals(Pitch.My))
            {
                AssignRole(Pitch.Ball.Owner, new Berserker(Pitch.Ball.Owner));
            }
            else
            {
                var enemyClosest = Pitch.EnemyClosestToBall;
                var enemyClosestDistToBall = double.PositiveInfinity;

                if (enemyClosest != null)
                    enemyClosestDistToBall = enemyClosest.GetEstimatedBallDistance() / 1.2;

                var myOrdered = Pitch.My.Players
                                     .OrderBy(pl => pl.GetEstimatedBallDistance());

                var myOrderedClosest = myOrdered
                    .Where(pl => pl.GetEstimatedBallDistance() < enemyClosestDistToBall)
                    .ToList();

                //grab the ball
                if (myOrderedClosest.Any(pl => pl.PlayerType != PlayerType.Keeper))
                {
                    //closest is field player
                    var closestFieldPlayer = myOrdered.First(pl => pl.PlayerType != PlayerType.Keeper);
                    if (closestFieldPlayer.CanGetBall())
                    {
                        AssignRole(closestFieldPlayer, new BallGrabber(closestFieldPlayer));
                        ClearExclusiveRole(closestFieldPlayer, typeof(BallGrabber));
                    }

                    var keeper = Pitch.MyKeeper;

                    if (keeper != null)
                    {
                        AssignRole(keeper, new DefensiveKeeper(keeper));
                        ClearExclusiveRole(keeper, typeof(DefensiveKeeper));
                    }
                }
                else
                {
                    //closest is keeper - assign him grab ball and substitute keeper to closest to goal field player
                    var closestKeeper = myOrdered.First();
                    if (closestKeeper.CanGetBall())
                    {
                        AssignRole(closestKeeper, new BallGrabber(closestKeeper));
                        ClearExclusiveRole(closestKeeper, typeof(BallGrabber));
                    }

                    var newKeeper = myOrdered.Skip(1).FirstOrDefault();
                    if (newKeeper != null)
                    {
                        AssignRole(newKeeper, new DefensiveKeeper(newKeeper));
                        ClearExclusiveRole(newKeeper, typeof(DefensiveKeeper));
                    }
                }
            }

            var unassigned = Pitch.My.Players
                                  .Where(pl => !assignedRoles.ContainsKey(pl.PlayerType))
                                  .ToList();

            unassigned.ForEach(pl => AssignRole(pl, new Berserker(pl)));
        }

        private static void AssignDefenceRoles()
        {
            //todo not remove fallen players from this list, but add them extra distance to ball basing on timer
            var keeper = Pitch.MyKeeper;

            if (keeper != null)
            {
                AssignRole(keeper, new DefensiveKeeper(keeper));
                ClearExclusiveRole(keeper, typeof(DefensiveKeeper));
            }

            //todo if there is someone closer to my goal than keeper and enemy is closer than keeper - assign second keeper



            var closestToBall = Pitch.MyFieldClosestToBall;
            if (closestToBall != null)
            {
                AssignRole(closestToBall, new Stopper(closestToBall));
                ClearExclusiveRole(closestToBall, typeof(Stopper));
            }


            var ballMarkArea = (Pitch.Ball.Position - Field.MyGoal.Position).X*1.2;
            var markDistance = Math.Max(DistanceUtils.DefenceDistance, ballMarkArea);

            var assignedDefenders = assignedRoles
                .Where(pl => pl.Value.GetType() == typeof (Defender))
                .Select(pl => new KeyValuePair<PlayerType, Defender>(pl.Key, (Defender)pl.Value))
                .ToList();

            var playersToMark = Pitch.Enemy.Players
                                     .Where(pl => Pitch.Ball.Owner != pl)
                                     .Where(pl => pl.Position.X < markDistance)
                                     .ToList();

            var nonMarkedPlayersToMark = playersToMark
                .Where(enemy => assignedDefenders.All(def => def.Value.MarkTarget != enemy.PlayerType))
                .ToList();

            var defendersLostRole = assignedDefenders
                .Where(def => playersToMark.All(enemy => enemy.PlayerType != def.Value.MarkTarget))
                .ToList();

            var freePlayers = Pitch.My.Players
                .Where(pl => !assignedRoles.ContainsKey(pl.PlayerType))
                .Concat(defendersLostRole.Select(def => def.Value.Self))
                .ToList();

            foreach (var enemy in nonMarkedPlayersToMark.OrderBy(pl => pl.GetDistanceTo(Field.MyGoal)))
            {
                if (!freePlayers.Any())
                    break;

                var defender = freePlayers.First();
                AssignRole(defender, new Defender(defender, enemy));
                freePlayers.Remove(defender);
            }

            //zip free with enemies, disregards enemy half players
            //assign roles to free players - strip ball and go forward
        }

        private static void ClearExclusiveRole(Player exclusive, Type roleType)
        {
            var roles = assignedRoles
                .Where(pl => pl.Value.GetType() == roleType)
                .ToList();
            if (roles.Count() > 1)
            {
                var clearRoles = roles.Where(pl => pl.Key != exclusive.PlayerType);
                foreach (var clear in clearRoles)
                {
                    assignedRoles.Remove(clear.Key);
                    Pitch.Log("Clearing {0} from {1} as it is exclusive", clear.Value.GetType().Name, clear.Key);
                }
            }
        }

        private static void AssignRole(Player player, IRole role)
        {
            if (assignedRoles.ContainsKey(player.PlayerType))
            {
                if(assignedRoles[player.PlayerType].Equals(role))
                    return;
            }

            assignedRoles[player.PlayerType] = role;
            Pitch.Log("Assigned {0} to {1}", role.GetType().Name, player.PlayerType);
        }


        private static void SetAttackState()
        {
            if (!inAttack && Pitch.Ball.Owner != null && Pitch.Ball.Owner.Team == Pitch.My)
            {
                inAttack = true;
                Pitch.Log("Going to attack because owned ball, flushing roles");
                assignedRoles.Clear();
            }
            if (inAttack && Pitch.Ball.Owner != null && Pitch.Ball.Owner.Team == Pitch.Enemy)
            {
                inAttack = false;
                Pitch.Log("Going to defence because enemy owns ball, flushing roles");
                assignedRoles.Clear();
            }
            if (Pitch.Ball.Owner == null)
            {
                var closest = Pitch.ClosestToBall;

                if (closest.Team == Pitch.My && !inAttack)
                {
                    inAttack = true;
                    Pitch.Log("Going to attack because closest to ball, flushing roles");
                    assignedRoles.Clear();
                }
                if(closest.Team != Pitch.My && inAttack)
                {
                    inAttack = false;
                    Pitch.Log("Going to defence because not closest to ball, flushing roles");
                    assignedRoles.Clear();
                }
            }
        }
    }
}
