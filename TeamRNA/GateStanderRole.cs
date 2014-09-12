using Common;

namespace TeamRNA
{
    public class GateStanderRole : IRole
    {
        private readonly IPosition position;

        public GateStanderRole(IPosition position)
        {
            this.position = position;
        }

        public void DoAction(Player self, Pitch pitch)
        {
            self.ActionGo(position);
        }
    }
}