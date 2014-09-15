using Common;

namespace TeamRNA.SpecialRoles
{
    public class DefensiveKeeper : IRole
    {
        public void DoAction(Player self, Pitch pitch)
        {
            self.ActionGo(Field.MyGoal);
        }
    }
}