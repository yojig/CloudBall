using System.Collections.Generic;
using Common;
using TeamRNA.AttackingRoles;

namespace TeamRNA
{
    public enum GameStage
    {
        Reset,
        Start,
        Defence,
        Attack
    }
    
    public class NewSquad : ITeam
    {
        private static readonly List<IRole> assignedRoles = new List<IRole>();

        public NewSquad()
        {
            assignedRoles.Add(new CenterForward(Pitch.MyPlayer(PlayerType.CenterForward)));
    
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

                //if (closest.Team == Pitch.My && !inAttack)
                //{
                //    inAttack = true;
                //    Pitch.Log("Going to attack because closest to ball, flushing roles");
                //    assignedRoles.Clear();
                //}
                //if (closest.Team != Pitch.My && inAttack)
                //{
                //    inAttack = false;
                //    Pitch.Log("Going to defence because not closest to ball, flushing roles");
                //    assignedRoles.Clear();
                //}
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