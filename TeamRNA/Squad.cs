using Common;

namespace TeamRNA
{
    public class Squad : ITeam
    {
        private bool inAttack;

        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            var pitch = new Pitch(myTeam, enemyTeam, ball, matchInfo);

            if (!inAttack && ball.Owner != null && ball.Owner.Team == myTeam)
            {
                inAttack = true;
            }
            if (inAttack && ball.Owner != null && ball.Owner.Team == enemyTeam)
            {
                inAttack = false;
            }
            if (inAttack && ball.Owner == null)
            {
                //get closest to ball
            }
        }
    }
}
