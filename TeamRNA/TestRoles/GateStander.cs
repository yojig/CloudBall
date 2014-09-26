using System;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.TestRoles
{
    public class GateStander : BaseRole, IEquatable<GateStander>
    {
        private readonly IPosition position;

        public GateStander(PlayerType self, IPosition position)
            : base(self)
        {
            this.position = position;
        }

        public override void DoAction()
        {
            Self.ActionGo(position);
        }

        public bool Equals(GateStander other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GateStander) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}