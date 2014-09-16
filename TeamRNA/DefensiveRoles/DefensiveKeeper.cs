using System;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.DefensiveRoles
{
    public class DefensiveKeeper : BaseRole, IEquatable<DefensiveKeeper>
    {
        public DefensiveKeeper(Player self) : base(self)
        {
        }

        public override void DoAction()
        {
            Self.ActionGo(Field.MyGoal);
        }

        public bool Equals(DefensiveKeeper other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((DefensiveKeeper) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}