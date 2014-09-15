using Common;

namespace TeamRNA.DefensiveRoles
{
    public class DefensiveKeeper : IRole
    {
        public void DoAction(Player self, Pitch pitch)
        {
            self.ActionGo(Field.MyGoal);
        }
    }
}