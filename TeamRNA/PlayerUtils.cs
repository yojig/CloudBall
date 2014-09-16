using Common;

namespace TeamRNA
{
    public static class PlayerUtils
    {
        public static bool CanGetBall(this Player player)
        {
            if (Pitch.Ball.Owner != null)
                return false;

            return true;
        }

        public static void GoForBall(this Player player)
        {
            player.ActionGo(Pitch.Ball.GetFuturePosition());
        }
    }
}