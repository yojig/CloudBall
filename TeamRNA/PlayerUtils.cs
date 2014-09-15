using Common;

namespace TeamRNA
{
    public static class PlayerUtils
    {
        public static bool CanGetBall(this Player player, Ball ball)
        {
            if (ball.Owner != null)
                return false;

            return true;
        }

        public static void GoForBall(this Player player, Ball ball)
        {
            player.ActionGo(ball.GetFuturePosition());
        }
    }
}