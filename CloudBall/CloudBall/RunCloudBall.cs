using System;

namespace CloudBall
{
#if WINDOWS || XBOX
    static class RunCloudBall
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //using (var game = new Client.Client(new TeamRNA.Squad(), new TeamRNA.TestSquad()))
            //using (var game = new Client.Client(new TeamRNA.Squad(), new TeamYojig.First()))
            //using (var game = new Client.Client(new TeamRNA.Squad(), new TeamIce.TeamIce()))
            using (var game = new Client.Client(new TeamRNA.PassingTestSquad(), new TeamRNA.TestSquad()))
            {
                game.Run();
            }
        }
    }
#endif
}

