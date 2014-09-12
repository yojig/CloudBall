using System.Collections.Generic;
using System.Linq;
using Common;

namespace TeamYojig
{
    public class First:ITeam
    {
        private readonly Dictionary<PlayerType, IPlayerRole> players = new Dictionary<PlayerType, IPlayerRole>();
        private bool inAttack;

        public First()
        {
            players.Add(PlayerType.Keeper, new UpperKeeper());
            players.Add(PlayerType.LeftDefender, new Sweeper());
            players.Add(PlayerType.CenterForward, new Defender());
            players.Add(PlayerType.RightDefender, new TargetMan());
            players.Add(PlayerType.LeftForward, new UpperWinger());
            players.Add(PlayerType.RightForward, new LowerWinger());
        }

        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            if (!inAttack && ball.Owner != null && ball.Owner.Team == myTeam)
            {
                inAttack = true;
            }
            if (inAttack && ball.Owner != null && ball.Owner.Team == enemyTeam)
            {
                inAttack = false;
            }

            var game = new Game(matchInfo, ball, enemyTeam, myTeam, inAttack);


            foreach (var kv in players)
            {
                kv.Value.DoAction(game, myTeam.Players.First(pl => pl.PlayerType == kv.Key));
            }
        }
    }
}
