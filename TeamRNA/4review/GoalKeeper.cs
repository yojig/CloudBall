using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.ForReview
{
    public class GoalKeeper : BaseRole
    {
        public GoalKeeper(PlayerType self) : base(self)
        { }

        public override void DoAction()
        {
            var myTeam = Pitch.My;
            var enemyTeam = Pitch.Enemy;
            var ball = Pitch.Ball;

            if (BallOwner)
            {
                var partner = Self.GetClosestUncovered(myTeam, enemyTeam);
                if (partner != null)
                {
                    var power = Self.GetDistanceTo(partner) / 10;
                    Self.ActionShoot(partner, power);
                    return;
                }
                else
                {
                    // todo: shoot just to enemy side
                    Self.ActionShootGoal();
                    return;
                }
            }

            if (Self.CanPickUpBall(ball))
            {
                Self.ActionPickUpBall();
                return;
            }

            
            if (Pitch.ClosestToBall != null 
                && Pitch.ClosestToBall.PlayerType == Type 
                && Pitch.ClosestToBall.Team == Pitch.My
                && ball.GetDistanceTo(Field.MyGoal.Center) < 400)
            {
                Self.ActionGo(ball.Position);
                return;
            }

            var dir = ChooseDirection(Self, ball);
            Self.ActionGo(dir);
        }

        public static Vector ChooseDirection(Player player, Ball ball)
        {
            var k1 = Line.K0(ball.Position, Field.MyGoal.Top);
            var k2 = Line.K0(ball.Position, Field.MyGoal.Bottom);
            var kball = ball.Velocity.Y / ball.Velocity.X;
            var lineball = Line.One(ball.Position, ball.Position + ball.Velocity);

            if ( ball.Owner == null
                && lineball.K >= Math.Min(k1, k2)
                && lineball.K <= Math.Max(k1, k2))
            {
                var nball = Line.Normal(lineball, player.Position);
                var intpos = Line.Cross(lineball, nball);
                if (Field.Borders.Contains(intpos)) return intpos;
                return Line.Cross(lineball, Line.One(Field.MyGoal.Top, Field.MyGoal.Bottom));
            }

            var line = Line.One(ball.Position, Field.MyGoal.Center);

            var n1 = Line.Normal(line, Field.MyGoal.Top);
            var n2 = Line.Normal(line, Field.MyGoal.Bottom);
            var a = Line.Cross(line, n1);
            var b = Line.Cross(line, n2);

            if (ball.GetDistanceTo(a) <= ball.GetDistanceTo(b))
                return a;
            return b;
        }
    }
}
