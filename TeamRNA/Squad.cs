using System.Linq;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA
{
    public class Squad : ITeam
    {
        private bool inAttack;

        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            var pitch = new Pitch(myTeam, enemyTeam, ball, matchInfo);

            SetAttackState(pitch);

            if (inAttack)
                AssignAttackRoles(pitch);
            else
                AssignDefenceRoles(pitch);
        }

        private static void AssignAttackRoles(Pitch pitch)
        {
            var freePlayers = pitch.My.Players.ToList();
            var enemyClosest = pitch.Enemy.Players
                                    .OrderBy(pl => pl.GetEstimatedDistance(pitch.Ball))
                                    .FirstOrDefault();
            var enemyClosestDistToBall = double.PositiveInfinity;

            if (enemyClosest != null)
                enemyClosestDistToBall = (enemyClosest.GetEstimatedDistance(pitch.Ball)/1.2);

            var myOrdered = pitch.My.Players
                                 .OrderBy(pl => pl.GetEstimatedDistance(pitch.Ball));

            var myOrderedClosest = myOrdered
                .Where(pl => pl.GetEstimatedDistance(pitch.Ball) < enemyClosestDistToBall)
                .ToList();

            //grab the ball
            if (myOrderedClosest.Any(pl => pl.PlayerType != PlayerType.Keeper))
            {
                //closest is field player
                var closestFieldPlayer = myOrdered.First(pl => pl.PlayerType != PlayerType.Keeper);
                if (closestFieldPlayer.CanGetBall(pitch.Ball))
                {
                    new BallGrabber().DoAction(closestFieldPlayer, pitch);
                    freePlayers.Remove(closestFieldPlayer);
                }
            }
            else
            {
                //closest is keeper - assign him grab ball and substitute keeper to closest to goal field player
                var closestKeeper = myOrdered.First();
                if (closestKeeper.CanGetBall(pitch.Ball))
                {
                    new BallGrabber().DoAction(closestKeeper, pitch);
                    freePlayers.Remove(closestKeeper);
                }

                var newKeeper = pitch.My.Players
                                     .Where(pl => pl.PlayerType != PlayerType.Keeper)
                                     .OrderBy(pl => pl.GetEstimatedDistance(Field.MyGoal))
                                     .FirstOrDefault();
                if (newKeeper != null)
                {
                    new DefensiveKeeper().DoAction(newKeeper, pitch);
                    freePlayers.Remove(newKeeper);
                }
            }
        }

        private static void AssignDefenceRoles(Pitch pitch)
        {
            //todo not remove fallen players from this list, but add them extra distance to ball basing on timer
            var freePlayers = pitch.My.Players.ToList();
            var keeper = pitch.My.Players.FirstOrDefault(pl => pl.PlayerType == PlayerType.Keeper);

            if (keeper != null)
            {
                new DefensiveKeeper().DoAction(keeper, pitch);
                freePlayers.Remove(keeper);
            }

            var closestToBall = pitch.My.Players
                                    .OrderBy(pl => pl.GetEstimatedDistance(pitch.Ball))
                                    .FirstOrDefault();
            if (closestToBall != null)
            {
                new Stopper().DoAction(closestToBall, pitch);
                freePlayers.Remove(closestToBall);
            }

            //zip free with enemies, disregards enemy half players
            //assign roles to free players - strip ball and go forward
        }


        private void SetAttackState(Pitch pitch)
        {
            if (!inAttack && pitch.Ball.Owner != null && pitch.Ball.Owner.Team == pitch.My)
            {
                inAttack = true;
            }
            if (inAttack && pitch.Ball.Owner != null && pitch.Ball.Owner.Team == pitch.Enemy)
            {
                inAttack = false;
            }
            if (inAttack && pitch.Ball.Owner == null)
            {
                var closest = pitch.ClosestToBall;

                inAttack = closest.Team == pitch.My;
            }
        }
    }
}
