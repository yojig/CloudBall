using Common;

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
    }
}