using System.Collections.Generic;
using Common;
using System.Linq;

namespace TeamRNA
{
    public class Pitch
    {
        public Team My { get; private set; }
        public Team Enemy { get; private set; }
        public Ball Ball { get; private set; }
        public MatchInfo Info { get; private set; }

        public Pitch(Team my, Team enemy, Ball ball, MatchInfo info)
        {
            My = my;
            Enemy = enemy;
            Ball = ball;
            Info = info;
        }

        public Player ClosestToBall
        {
            get
            {
                var myClosest = My.Players
                                  .OrderBy(pl => pl.GetEstimatedDistance(Ball))
                                  .FirstOrDefault();
                var enemyClosest = Enemy.Players
                                  .OrderBy(pl => pl.GetEstimatedDistance(Ball))
                                  .FirstOrDefault();

                if (myClosest != null && enemyClosest != null)
                {
                    if (myClosest.GetEstimatedDistance(Ball)*1.2 < enemyClosest.GetEstimatedDistance(Ball))
                        return myClosest;

                    return enemyClosest;
                }

                if (myClosest != null)
                    return myClosest;

                if (enemyClosest != null)
                    return enemyClosest;

                return null;
            }
        }


        
    }
}