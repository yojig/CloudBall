using System;
using GentleWare.CloudBall.Wolkenhondjes1;

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
            //for team ice there are two dlls
            using (var game = new Client.Client(new TeamRNA.Squad(), new TeamIce.TeamIce()))

            //using (var game = new Client.Client(new TeamRNA.Squad(), new TeamRNA.TestSquad()))
            //using (var game = new Client.Client(new TeamRNA.Squad(), new TeamYojig.First()))
            {
                game.Run();
            }
        }
    }
#endif
}

