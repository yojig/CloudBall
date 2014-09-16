using System;
using Common;
using System.Linq;
using TeamRNA.SpecialRoles;

namespace TeamRNA.AttackingRoles
{
    public class Berserker : BaseRole, IEquatable<Berserker>
    {
        public Berserker(Player self):base(self)
        {
        }

        public override void DoAction()
        {
            var closest = Pitch.Enemy.Players
                               .OrderBy(pl => pl.GetEstimatedDistance(Self))
                               .FirstOrDefault();

            if (closest != null && closest.GetEstimatedDistance(Self) > DistanceUtils.SafeDistance)
            {
                Self.ActionGo(Field.EnemyGoal);
                return;
            }

            if (Pitch.Ball.Owner == Self)
            {
                Self.ActionShootGoal(Constants.PlayerMaxShootStr);
                return;
            }

            if (Self.CanPickUpBall(Pitch.Ball))
            {
                Self.ActionPickUpBall();
                return;
            }

            Self.GoForBall();
        }

        public bool Equals(Berserker other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((Berserker)obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}