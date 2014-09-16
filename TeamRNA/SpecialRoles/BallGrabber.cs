using System;
using Common;

namespace TeamRNA.SpecialRoles
{
    public class BallGrabber : BaseRole, IEquatable<BallGrabber>
    {
        public BallGrabber(Player self) : base(self)
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

        public bool Equals(BallGrabber other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((BallGrabber) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}