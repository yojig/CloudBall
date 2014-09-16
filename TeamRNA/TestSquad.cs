using System;
using System.Linq;
using Common;
using TeamRNA.TestRoles;

namespace TeamRNA
{
    public class TestSquad : ITeam
    {
        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            Pitch.Assign(myTeam, enemyTeam, ball, matchInfo);

            var height = (int)Field.MyGoal.Height;
            var step = height/(myTeam.Players.Count + 1);

            var roles = Enumerable.Range(1, myTeam.Players.Count)
                                  .Zip(myTeam.Players, Tuple.Create)
                                  .Select(p => new GateStander(p.Item2, new Vector(0, Field.MyGoal.Y + p.Item1*step)));
                                  
            foreach (var role in roles)
                role.DoAction();
        }
    }
}