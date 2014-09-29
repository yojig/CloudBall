using System;
using System.Collections.Generic;
using Common;
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

        private static Dictionary<int, Tuple<Vector, Vector>> ballFuturePosition;
        public static Vector BallFuturePosition(int turn)
        {
            while (!ballFuturePosition.ContainsKey(turn))
                DistanceUtils.AddBallFuturePos(ref ballFuturePosition);

            return ballFuturePosition[turn].Item1;
        }

        public static void Assign(Team my, Team enemy, Ball ball, MatchInfo info)
        {
            My = my;
            Enemy = enemy;
            Ball = ball;
            Info = info;

            LogInfoEnabled = true;

            ballFuturePosition = DistanceUtils.BuildBallFuturePosition(50);
            closest = null;
        }

        private Pitch()
        {}

        private static Player closest;
        public static Player ClosestToBall
        {
            get
            {
                if (closest == null)
                {
                    var myClosest = My.Players
                                      .Where(pl => pl.FallenTimer > 0)
                                      .OrderBy(pl => pl.TurnsToGetBall())
                                      .FirstOrDefault();
                    var enemyClosest = Enemy.Players
                                            .Where(pl => pl.FallenTimer > 0)
                                            .OrderBy(pl => pl.TurnsToGetBall())
                                            .FirstOrDefault();

                    if (myClosest != null && enemyClosest != null)
                    {
                        if (myClosest.TurnsToGetBall()*1.01 < enemyClosest.TurnsToGetBall())
                            return myClosest;

                        closest = enemyClosest;
                    }

                    if (myClosest != null)
                        closest = myClosest;

                    if (enemyClosest != null)
                        closest = enemyClosest;
                }

                return closest;
            }
        }

        public static Player MyFieldClosestToBall
        {
            get
            {
                return My.Players
                    .Where(pl => pl.PlayerType != PlayerType.Keeper)
                    .OrderBy(pl => pl.TurnsToGetBall())
                    .FirstOrDefault(); 
            }
        }

        public static Player EnemyClosestToBall
        {
            get
            {
                return Enemy.Players
                    .OrderBy(pl => pl.TurnsToGetBall())
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

        public static bool LogInfoEnabled;
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