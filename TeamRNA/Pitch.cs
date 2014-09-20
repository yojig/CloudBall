﻿using Common;
using System.Linq;

namespace TeamRNA
{
    public class Pitch
    {
        public static Team My { get; private set; }
        public static Team Enemy { get; private set; }
        public static Ball Ball { get; private set; }
        public static MatchInfo Info { get; private set; }

        public static GameStage Stage { get; set; }

        public static void Assign(Team my, Team enemy, Ball ball, MatchInfo info)
        {
            My = my;
            Enemy = enemy;
            Ball = ball;
            Info = info;
        }

        private Pitch()
        {
        }

        public static Player ClosestToBall
        {
            get
            {
                var myClosest = My.Players
                                  .OrderBy(pl => pl.GetNextTurnDistance(Ball))
                                  .FirstOrDefault();
                var enemyClosest = Enemy.Players
                                  .OrderBy(pl => pl.GetNextTurnDistance(Ball))
                                  .FirstOrDefault();

                if (myClosest != null && enemyClosest != null)
                {
                    if (myClosest.GetNextTurnDistance(Ball) * 1.2 < enemyClosest.GetNextTurnDistance(Ball))
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

        public static Player MyFieldClosestToBall
        {
            get
            {
                return My.Players
                    .Where(pl => pl.PlayerType != PlayerType.Keeper)
                    .OrderBy(pl => pl.GetNextTurnDistance(Ball))
                    .FirstOrDefault();

            }
        }

        public static Player EnemyClosestToBall
        {
            get
            {
                return Enemy.Players
                    .OrderBy(pl => pl.GetNextTurnDistance(Ball))
                    .FirstOrDefault();

            }
        }

        public static void Log(string msg)
        {
            if (string.IsNullOrEmpty(My.DevMessage))
                My.DevMessage = "[" + Pitch.Info.CurrentTimeStep + "] " + msg;
            else
                My.DevMessage += "\r\n" + msg;
        }

        public static void Log(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }


        public static Player MyKeeper
        {
            get
            {
                return My.Players.FirstOrDefault(pl => pl.PlayerType == PlayerType.Keeper);
            }
        }

        public static Player MyPlayer(PlayerType type)
        {
            return My.Players.FirstOrDefault(pl => pl.PlayerType == type);
        }
    }
}