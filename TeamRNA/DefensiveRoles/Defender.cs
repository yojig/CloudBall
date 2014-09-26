using System;
using System.Linq;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.DefensiveRoles
{
    public class Defender : BaseRole
    {
        public readonly PlayerType MarkTarget;

        public Defender(PlayerType self, PlayerType mark)
            : base(self)
        {
            this.MarkTarget = mark;
        }

        public float PickedUpBallOnTurn = -1;

        public override void DoAction()
        {
            if (Self.FallenTimer > 0)
            {
                //Debug("is fallen - skip me");
                return;
            }

            if (BallOwner)
            {
                if (PickedUpBallOnTurn < 0)
                    PickedUpBallOnTurn = Pitch.Info.CurrentTimeStep;

                WithBall();
                return;
            }

            PickedUpBallOnTurn = -1;

            WithoutBall();
        }

        private void WithBall()
        {
            var isTackeImmune = Pitch.Info.CurrentTimeStep - PickedUpBallOnTurn < Constants.BallPickUpImmunityTimer - 1;
            var isSafe = isTackeImmune;

            var enemyPlayersCanTackle = Pitch.Enemy.Players
                    .Where(pl => pl.FallenTimer < 2)
                    .Where(pl => pl.TackleTimer < 2)
                    .Where(pl => pl.GetDistanceTo(Self) < Constants.PlayerMaxTackleDistance + 5)
                    .ToList();

            if (!isSafe)
            {
                
                if (!enemyPlayersCanTackle.Any())
                    isSafe = true;
            }

            if (isSafe)
            {
                Self.ActionGo(Field.EnemyGoal);
                return;
            }
            else
            {
                var enemiesInGoalDirection = enemyPlayersCanTackle
                                                  .Where(pl => pl.Position.X > Self.Position.X)
                                                  .ToList();

                if (enemiesInGoalDirection.Count == 0 && Self.Position.GetDistanceTo(Field.EnemyGoal) < Field.EnemyGoal.Height * 3)
                {
                    Info("shooting empty goal");
                    Self.ActionShoot(Field.EnemyGoal, (float)(Constants.PlayerMaxShootStr * 0.85));
                    
                    return;
                }

                var close = enemiesInGoalDirection
                    .Where(pl => pl.GetDistanceTo(Self.Position) < Constants.PlayerMaxTackleDistance*2)
                    .ToList();
                switch (close.Count)
                {
                    case 0:
                        Self.ActionGo(Field.EnemyGoal);
                        return;
                    case 1:
                        if (close[0].Position.GetDistanceTo(Self.Position) < Constants.PlayerMaxTackleDistance + 1)
                        {
                            Info("shooting with max power");
                            Self.ActionShoot(Field.EnemyGoal, (float)(Constants.PlayerMaxShootStr));
                        }
                        else
                        {
                            var target = (close[0].Position - Self.Position);
                            if (target.Y > 0)
                                target.Rotate((float)-(Math.PI / 4));
                            else
                                target.Rotate((float)(Math.PI / 4));
                            Self.ActionGo(target);
                        }

                        return; 
                    default:
                        //todo scale velocity
                        Info("shooting in the middle");
                        var dir = close[0].Position - close[1].Position;
                        var shootTarget = close[1].Position + dir / 2;

                        Info("action shoot");
                        Self.ActionShoot(shootTarget, (float)(Constants.PlayerMaxShootStr));
                        return;
                }
            }
        }

        private void WithoutBall()
        {
            var target = Pitch.Enemy.Players.First(pl => pl.PlayerType == MarkTarget);

            if (Self.CanPickUpBall(Pitch.Ball))
            {
                Self.ActionPickUpBall();
                Info("picking up the ball");
                return;
            }

            if (Self.CanTackle(target))
            {
                Self.ActionTackle(target);
                Info("tackling enemy");
                return;
            }

            if (Self.CanTackle(Pitch.Ball.Owner))
            {
                Self.ActionTackle(Pitch.Ball.Owner);
                Info("tackling enemy with ball");
                return;    
            }

            MarkPlayer(target);
        }

        private void MarkPlayer(Player target)
        {
            //add velocity shift
            var velocityDiff = target.Velocity - Self.Velocity;
            var velocityShift = velocityDiff.Length * velocityDiff.Length * 2.5;

            var targetPosition = target.Position + target.Velocity;
            var targetVector = Field.MyGoal.Position - targetPosition;
            targetVector = targetVector/targetVector.Length*(Constants.PlayerMaxTackleDistance + 5 + velocityShift);

            var goPos = target.Position + targetVector;

            //if target position is behind the ball than chase ball
            if (goPos.X*1.1 > Pitch.Ball.Position.X)
            {
                velocityDiff = Pitch.Ball.Velocity - Self.Velocity;
                velocityShift = velocityDiff.Length * velocityDiff.Length;

                targetPosition = Pitch.Ball.Position + Pitch.Ball.Velocity;
                targetVector = Field.MyGoal.Position - targetPosition;
                targetVector = targetVector / targetVector.Length * (Constants.BallMaxPickUpDistance - 5 + velocityShift);

                goPos = Pitch.Ball.Position + targetVector;
            }

            Self.ActionGo(goPos);
            //Debug("marking player");
        }
    }
}