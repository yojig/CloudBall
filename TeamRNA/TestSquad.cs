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
            var pitch = new Pitch(myTeam, enemyTeam, ball, matchInfo);

            var height = (int)Field.MyGoal.Height;
            var step = height/(myTeam.Players.Count + 1);

            var roles = Enumerable.Range(1, myTeam.Players.Count)
                                  .Select(p => new GateStander(new Vector(0, Field.MyGoal.Y + p*step)))
                                  .Zip(myTeam.Players, Tuple.Create);

            foreach (var role in roles)
                role.Item1.DoAction(role.Item2, pitch);
        }
    }
}