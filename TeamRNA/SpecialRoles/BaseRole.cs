using Common;

namespace TeamRNA.SpecialRoles
{
    public abstract class BaseRole : IRole
    {
        public PlayerType Type { get; set; }
        public Player Self { get { return Pitch.MyPlayer(Type); } }

        public abstract void DoAction();

        protected BaseRole(PlayerType self)
        {
            Type = self;
        }

        public bool BallOwner
        {
            get { return Pitch.Ball.Owner != null && Pitch.Ball.Owner.Team == Pitch.My && Pitch.Ball.Owner.PlayerType == Type; }
        }

        public Player ClosestEnemy
        {
            get { return Self.ClosestEnemy(); }
        }


        public void Info(string format, params string[] p)
        {
            if (Pitch.LogInfoEnabled)
                Pitch.Log("[" + Type + "] " + format, p);
        }
    }
}