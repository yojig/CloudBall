using Common;

namespace TeamYojig
{
    public class Sweeper : IPlayerRole
    {
        public void DoAction(Game game, Player self)
        {
            var ball = game.Ball;

            if (ball.Owner == self)
            {
                var defender = game.My.Defender();

                if (defender.IsSafe(game, self))
                {
                    self.ActionShoot(defender, self.GetDistanceTo(defender) * 1.5f);
                }
                else
                {
                    if (self.Position.X < Field.Borders.Center.X*0.2)
                    {
                        float yPos;
                        var center = Field.Borders.Center;
                        if (self.Position.Y > center.Y)
                        {
                            yPos = center.Y + center.Y/2;
                        }
                        else
                        {
                            yPos = center.Y - center.Y/2;
                        }

                        self.ActionShoot(new Vector(0, yPos));
                        return;
                    }


                    if (self.Position.X < Field.Borders.Center.X * 0.75)
                    {
                        var pos = self.GetCenterShootout(game.Enemy);
                        self.ActionShoot(pos);
                        return;
                    }

                    self.ActionShootGoal();
                    return;
                }

                return;
            }

            if (Utility.TryField(game, self))
                return;

            var closestToGoal = Field.MyGoal.GetClosest(game.Enemy);
            IPosition target;
            if (ball.GetBallNextPosition().GetDistanceTo(Field.MyGoal) > closestToGoal.GetDistanceTo(Field.MyGoal))
            {
                target = closestToGoal;
            }
            else
            {
                target = ball;
            }

            target = new Vector(target.Position.X*0.8, target.Position.Y);
            self.ActionGo(target);
        }
    }
}