using System;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.DefensiveRoles
{
    public class Stopper : BaseRole, IEquatable<Stopper>
    {
        public Stopper(Player self) : base(self)
        {
        }

        public override void DoAction()
        {
            if (Self.CanPickUpBall(Pitch.Ball))
            {
                Self.ActionPickUpBall();
                return;
            }

            Self.GoForBall();
        }

        public bool Equals(Stopper other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((Stopper)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}