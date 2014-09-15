using Common;

namespace TeamRNA.SpecialRoles
{
    public class BallGrabber : IRole
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