using Common;

namespace TeamRNA.TestRoles
{
    public class GateStander : IRole
    {
        private readonly IPosition position;

        public GateStander(IPosition position)
        {
            this.position = position;
        }

        public void DoAction(Player self, Pitch pitch)
        {
            self.ActionGo(position);
        }
    }
}