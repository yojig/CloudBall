using System.Linq;
using Common;

namespace TeamRNA.SpecialRoles
{
    public abstract class BaseRole : IRole
    {
        public PlayerType Type { get; set; }
        public Player Self { get { return Pitch.My.Players.First(pl => pl.PlayerType == Type); } }
        public abstract void DoAction();

        protected BaseRole(Player self)
        {
            Type = self.PlayerType;
            
        }
    }
}