using Common;

namespace TeamRNA.DefensiveRoles
{
    public class Berserker : IRole
    {
        public void DoAction(Player self, Pitch pitch)
        {
            if (pitch.Ball.Owner == self)
            {
                self.ActionShootGoal(10);
                return;
            }

            if (self.CanPickUpBall(pitch.Ball))
            {
                self.ActionPickUpBall();
                return;
            }

            self.GoForBall(pitch.Ball);
        }
    }
}