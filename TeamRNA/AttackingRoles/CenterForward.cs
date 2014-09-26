using System.Linq;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.AttackingRoles
{
    public class CenterForward : BaseRole
    {
        public float PickedUpBallOnTurn = -1;

        public CenterForward(PlayerType self)
            : base(self)
        {
        }

        public override void DoAction()
        {
            if (Self.FallenTimer > 0)
            {
                //Pitch.Log("{0} is fallen - skip me", Type);
                return;
            }

            if (BallOwner)
            {
                if (PickedUpBallOnTurn < 0)
                    PickedUpBallOnTurn = Pitch.Info.CurrentTimeStep;

                var isSafe = Pitch.Info.CurrentTimeStep - PickedUpBallOnTurn < Constants.BallPickUpImmunityTimer;

                if (!isSafe)
                {
                    var enemyPlayersCanTackle = Pitch.Enemy.Players
                        .Where(pl => pl.TackleTimer == 0)
                        .Where(pl => pl.GetDistanceTo(Self) < Constants.PlayerMaxTackleDistance + 5);
                    if (!enemyPlayersCanTackle.Any())
                        isSafe = true;
                }

                if (isSafe)
                {
                    Self.ActionGo(Field.EnemyGoal);
                }
                else
                {
                    var myPlayer = Pitch.My.Players
                        .Where(pl => pl.PlayerType != Type)
                        .OrderBy(pl => pl.GetDistanceTo(Self))
                        .First();
                    var scaledPosition = myPlayer.Position + myPlayer.Velocity*30;
                    Self.ActionShoot(scaledPosition, (float)(Constants.PlayerMaxShootStr * 0.7));
                }

                return;
            }
            else
            {
                PickedUpBallOnTurn = -1;
            }

            //general actions

            if (Self.TackleTimer == 0 && ClosestEnemy.GetDistanceTo(Self) < Constants.PlayerMaxTackleDistance &&
                Self.CanTackle(ClosestEnemy))
            {
                Self.ActionTackle(ClosestEnemy);
                Info("tackling enemy");
                return;
            }
            
            if (Self.CanPickUpBall(Pitch.Ball))
            {
                Self.ActionPickUpBall();
                Info("picking up the ball");
                return;
            }

            switch (Pitch.Stage)
            {
                case GameStage.Start:
                default:
                    Self.ActionGo(Pitch.Ball);
                    return;
            }
        }
    }
}