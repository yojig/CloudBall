using Common;

namespace TeamRNA.DefensiveRoles
{
    public class Defender : IRole
    {
        private readonly Player mark;

        public Defender(Player mark)
        {
            this.mark = mark;
        }

        public void DoAction(Player self, Pitch pitch)
        {
            if (self.CanPickUpBall(pitch.Ball))
            {
                self.ActionPickUpBall();
                return;
            }

            self.ActionGo(mark.GetFuturePosition());
        }
    }
}