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
            using (var game = new Client.Client(new TeamYojig.First(), new TeamRNA.TestSquad()))
            {
                game.Run();
            }
        }
    }
#endif
}

