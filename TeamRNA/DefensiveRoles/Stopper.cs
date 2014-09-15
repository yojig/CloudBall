using Common;

namespace TeamRNA.DefensiveRoles
{
    public class Stopper : IRole
    {
        public void DoAction(Player self, Pitch pitch)
        {
            if (self.CanPickUpBall(pitch.Ball))
            {
                self.ActionPickUpBall();
                return;
            }
            
            self.GoForBall(pitch.Ball);
        }
    }
}