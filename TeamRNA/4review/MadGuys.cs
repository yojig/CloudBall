//Common contains all the classes you need to play CloudBall.
using System;
using System.Linq;
using System.Diagnostics;
using Common;
using TeamRNA.ForReview;

//We need some maths for more advanced teams.

namespace SuperTeam
{
    public class MadGuys : ITeam
    {
        public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
        {
            foreach (Player player in myTeam.Players)
            {
                Player closestEnemy = player.GetClosest(enemyTeam);
                switch (player.PlayerType)
                {
                    case PlayerType.Keeper:
                        if (ball.Owner == player)
                        {
                            var partner = player.GetClosestUncovered(myTeam, enemyTeam);
                            if (partner != null)
                            {
                                var power = player.GetDistanceTo(partner) / 10;
                                player.ActionShoot(partner, power);
                                continue;
                            }
                            else
                            {
                                // todo: shoot just to enemy side
                                player.ActionShootGoal();
                                continue;
                            }
                        }
                        if (player.GetDistanceTo(ball) < 50 
                            && player.CanPickUpBall(ball))
                        {
                            player.ActionPickUpBall();
                            continue;
                        }
                        if (ball.GetClosest(myTeam) == player
                            && ball.GetClosest(enemyTeam).GetDistanceTo(ball) < player.GetDistanceTo(ball)
                            && ball.GetDistanceTo(Field.MyGoal.Center) < 400)
                        {
                            player.ActionGo(ball.Position);
                            continue;
                        }

                        var direction = GoalKeeper.ChooseDirection(player, ball);
                        player.ActionGo(direction);
                        continue;

                    default:
                        if (ball.Owner == player)
                            if (player.GetDistanceTo(Field.EnemyGoal.Center) > 400)
                            {
                                var inters = player.GetClosestInterceptors(enemyTeam, 400);
                                if (!inters.Any())
                                {
                                    player.ActionGo(Field.EnemyGoal.Center);
                                    continue;
                                }
                                else if (inters.Count() >= 3)
                                {
                                    var partner = player.GetClosestUncovered(myTeam, enemyTeam);
                                    if (partner != null) player.ActionShoot(partner, 100);
                                }
                                var goalDirect = (Field.EnemyGoal.Center - player.Position).GetDirection();
                                var direct = (from i in inters
                                              select (i.Velocity) + (player.Position - i.Position).GetDirection() * (1 - i.GetDistanceTo(player) / 500))
                                              .Aggregate(Vector.Zero, (acc, v) => acc + v).GetDirection() + goalDirect;
                                var dest = player.Position + direct;
                                //if (direct.X < -50 && partner != null)
                                //{
                                //    player.ActionShoot(partner, 100);
                                //    continue;
                                //}
                                //else 
                                //    if (dest.Y < Field.Borders.Top.Y + 200
                                //    || dest.Y > Field.Borders.Bottom.Y - 200)
                                //{
                                //    player.ActionGo(dest + goalDirect * 2);
                                //    continue;
                                //}
                                //else 
                                //        if (dest.X > Field.Borders.Right.X - 100)
                                //{
                                //    player.ActionGo(Field.Borders.Center);
                                //    continue;
                                //}
                                //else 
                                //if (Field.Borders.Contains(dest))
                                //{
                                    player.ActionGo(dest);
                                    continue;
                                //}
                                //else
                                //{
                                //    player.ActionGo(Field.EnemyGoal.Center);
                                //    continue;
                                //}
                            }
                            else
                            {
                                //var goalkepers = from ep in enemyTeam.InFrontOf(player)
                                //                 ;

                                player.ActionShootGoal();
                            }
                        break;
                }

                if (ball.Owner == player) //Allways shoots for the enemy goal.
                { }
                else if (player.CanPickUpBall(ball)) //Picks up the ball if posible.
                    player.ActionPickUpBall();

                else if (player.CanTackle(closestEnemy)
                         && ball.Owner == closestEnemy) //Tackles any enemy that is close.
                    player.ActionTackle(closestEnemy);

                else if (player == ball.GetClosest(myTeam)) //If the player is closest to the ball, go for it.
                {
                    var balldirect = ball.Velocity.GetDirection();
                    var ndirect = new Vector(-balldirect.Y, balldirect.X);
                    var direct = (ball.Position - player.Position).GetDirection() * 10;
                    if (direct.Length < 50)
                        player.ActionGo(ball);
                    else
                        player.ActionGo(direct + ndirect);
                }
                else if (player.CanTackle(closestEnemy)) //Tackles any enemy that is close.
                    player.ActionTackle(closestEnemy);
                else //If the player cannot do anything urgently usefull, move to a good position.
                {
                    if (player.PlayerType == PlayerType.Keeper) //The keeper protects the goal.
                    { }
                    //The keeper positions himself 50 units out from the goal                                                                                                            //at the same height as the ball, although never leaving the goal
                    else if (player.PlayerType == PlayerType.LeftDefender)
                        player.ActionGo(new Vector(Field.Borders.Width * 0.2, ball.Position.Y));
                    //The left defender helps protect the goal
                    else if (player.PlayerType == PlayerType.RightDefender)
                        player.ActionGo(Field.MyGoal.GetClosest(enemyTeam));
                    //The right defender chases the enemy closest to myGoal
                    else if (player.PlayerType == PlayerType.RightForward)
                        player.ActionGo((Field.Borders.Center + Field.Borders.Bottom + ball.Position) / 3);
                    //Right forward stays in position on the midline, untill the ball comes close.
                    else if (player.PlayerType == PlayerType.LeftForward)
                        player.ActionGo((Field.Borders.Center + Field.Borders.Top + ball.Position) / 3);
                    //Left forward stays in position on the midline, untill the ball comes close.
                    else if (player.PlayerType == PlayerType.CenterForward)
                        player.ActionGo((Field.Borders.Center + Field.EnemyGoal.Center + ball.Position) / 3);
                    //Center forward stays in position on the enemy side of the field.
                }
            }
        }
    }
}