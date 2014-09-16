using System;
using System.Linq;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.DefensiveRoles
{
    public class Defender : BaseRole, IEquatable<Defender>
    {
        public readonly PlayerType MarkTarget;

        public Defender(Player self, Player mark) : base(self)
        {
            this.MarkTarget = mark.PlayerType;
        }

        public override void DoAction()
        {
            if (Self.CanPickUpBall(Pitch.Ball))
            {
                Self.ActionPickUpBall();
                return;
            }

            //calculate target pos
            //if target is moving, then set point farther using diff in speed vectors
            //if target is stale set point closer

            var target = Pitch.Enemy.Players.First(pl => pl.PlayerType == MarkTarget);
            Self.ActionGo(target.GetFuturePosition());
        }

        public bool Equals(Defender other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(MarkTarget, other.MarkTarget);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Defender) obj);
        }

        public override int GetHashCode()
        {
            return (MarkTarget != null ? MarkTarget.GetHashCode() : 0);
        }
    }
}