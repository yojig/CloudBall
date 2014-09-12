using Common;

namespace TeamYojig
{
    public class Game
    {
        public Team My;
        public Team Enemy;
        public Ball Ball;
        public MatchInfo Info;
        public bool InAttack;

        public Game(MatchInfo info, Ball ball, Team enemy, Team my, bool inAttack)
        {
            Info = info;
            Ball = ball;
            Enemy = enemy;
            My = my;
            InAttack = inAttack;
        }
    }
}