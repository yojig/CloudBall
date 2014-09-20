using System.Linq;
using Common;
using TeamRNA.SpecialRoles;

namespace TeamRNA.AttackingRoles
{
    public class CenterForward : BaseRole
    {
        public float PickedUpBallOnTurn = -1;

        public CenterForward(Player self)
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
                        .Where(pl => pl.GetNextTurnDistance(Self) > Constants.PlayerMaxTackleDistance);
                    if (!enemyPlayersCanTackle.Any())
                        isSafe = true;
                }
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
                Pitch.Log("{0} tackling enemy", Type);
                return;
            }
            
            if (Self.CanPickUpBall(Pitch.Ball))
            {
                Self.ActionPickUpBall();
                Pitch.Log("{0} picking up the ball", Type);
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