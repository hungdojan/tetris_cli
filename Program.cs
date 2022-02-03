using System;
using System.Collections.Generic;

namespace Tetris
{
    class Program
    {
        public static List<Leaderboard> leaderboard = new List<Leaderboard>();
        public static GameControl controlKeys = new GameControl();
        private static void Main(string[] args)
        {
            controlKeys = FileClass.ReadControlKeys(Constants.PATH_CONTROL_KEYS);
            FileClass.ReadHighScore(Constants.PATH_LEADERBOARD);
            if (Environment.OSVersion.VersionString.Contains("Windows"))
                Console.SetWindowSize(Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT);
            else
                WindowSizeCheck();
            var tetris = new Tetris();
            string preWindow = "Menu";
            string window = "Menu";
            do
            {
                switch (window)
                {
                    case "Menu":
                        window = Window.Menu(tetris, out tetris);
                        preWindow = "Menu";
                        break;
                    case "Game":
                        window = tetris.Game();
                        break;
                    case "Leaderboard":
                        Window.Leaderboard();
                        window = "Menu";
                        break;
                    case "Pause":
                        window = Window.Pause(tetris);
                        preWindow = "Pause";
                        break;
                    case "Settings":
                        window = Window.Settings(preWindow);
                        break;
                }
            } while (window != "Exit");

            FileClass.WriteControlKeys(Constants.PATH_CONTROL_KEYS);
            FileClass.WriteHighScore(Constants.PATH_LEADERBOARD);
        }

        /// <summary>
        /// Checks for window size
        /// </summary>
        static void WindowSizeCheck()
        {
            while (Console.WindowHeight < Constants.WINDOW_HEIGHT || Console.WindowWidth < Constants.WINDOW_WIDTH)
            {
                Console.Clear();
                Console.WriteLine("Your window is too small");
                Console.WriteLine("To continue please resize your window");
                Console.WriteLine();
                Console.WriteLine("Your current window size: {0}x{1}", Console.WindowWidth, Console.WindowHeight);
                Console.WriteLine("Minimum size required: {0}x{1}", Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT);
                Console.WriteLine("To update and check your window size click any key...");
                Console.ReadKey(true);
            }
        }
    }
}
