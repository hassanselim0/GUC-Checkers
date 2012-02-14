using System;

namespace Checkers
{
    static class Program
    {
        public static Main game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            game = new Main();
            game.Run();
        }
    }
}

