using System.Collections.Generic;
using Common;
using TeamRNA.DefensiveRoles;
using TeamRNA.ForReview;

namespace TeamRNA
{
    public class Squad : ITeam
    {
        private static readonly List<IRole> assignedRoles = new List<IRole>();

        public Squad()
        {
            assignedRoles.Add(new Defender(PlayerType.CenterForward, PlayerType.CenterForward));
            assignedRoles.Add(new Defender(PlayerType.LeftForward, PlayerType.RightForward));
            assignedRoles.Add(new Defender(PlayerType.RightForward, PlayerType.LeftForward));
            assignedRoles.Add(new Defender(PlayerType.LeftDefender, PlayerType.RightDefender));
            assignedRoles.Add(new Defender(PlayerType.RightDefender, PlayerType.LeftDefender));
            assignedRoles.Add(new GoalKeeper(PlayerType.Keeper));
            
        }

        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            Pitch.Assign(myTeam, enemyTeam, ball, matchInfo);

            if (matchInfo.EnemyTeamScored || matchInfo.MyTeamScored)
            {
                Pitch.Log("=======================================");
                Pitch.Log("Someone scored, game stage set to reset");
                Pitch.Log("=======================================");
                Pitch.Stage = GameStage.Reset;
            }

            SetGameState();

            foreach (var assignedRole in assignedRoles)
                assignedRole.DoAction();
        }


        private static void SetGameState()
        {
            if (Pitch.Stage == GameStage.Reset)
            {
                Pitch.Stage = GameStage.Start;
                return;
            }
            
            if (Pitch.Stage == GameStage.Start && Pitch.Ball.Owner == null)
                return;

            if (Pitch.Ball.Owner == null)
            {
                //loose ball or pass
                var closest = Pitch.ClosestToBall;

                if (closest == null && Pitch.Stage != GameStage.GetTheBall)
                {
                    Pitch.Log("Going to get the ball - nobody near");
                    Pitch.Stage = GameStage.GetTheBall; 
                    return;
                }

                if(closest == null)
                    return;

                if (closest.Team == Pitch.My && Pitch.Stage != GameStage.GetTheBall)
                {
                    Pitch.Stage = GameStage.GetTheBall;
                    Pitch.Log("Going to get the ball");
                }
                if (closest.Team != Pitch.My && Pitch.Stage != GameStage.Defence)
                {
                    Pitch.Stage = GameStage.Defence;
                    Pitch.Log("Going to defence because not closest to ball");
                }
            }
            else if (Pitch.Ball.Owner.Team == Pitch.My && Pitch.Stage != GameStage.Attack)
            {
                Pitch.Stage = GameStage.Attack;
                Pitch.Log("Going to attack because owned ball");
            }
            else if (Pitch.Ball.Owner.Team == Pitch.Enemy && Pitch.Stage != GameStage.Defence)
            {
                Pitch.Stage = GameStage.Defence;
                Pitch.Log("Going to defence because enemy owns ball");
            }
        }
    }
}