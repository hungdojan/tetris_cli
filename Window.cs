using System;

namespace Tetris
{
    static class Window
    {
        /// <summary>
        /// Opens menu window
        /// </summary>
        /// <param name="tetris"></param>
        /// <param name="newTetris"></param>
        /// <returns>Name of the next window</returns>
        public static string Menu(Tetris tetris, out Tetris newTetris)
        {
            if (tetris == null)
                tetris = new Tetris();
            newTetris = tetris;
            PrintLabel("Tetris", Collections.MENU_WINDOW_LINES, out int cursorTop);
            Console.SetCursorPosition(0, cursorTop);
            CursorPosition(cursorTop, cursorTop + Collections.MENU_WINDOW_LINES.Length - 1);
            string result = "Menu";
            switch (Console.CursorTop - 1)
            {
                case 1:
                    if (System.IO.File.Exists(Constants.PATH_SAVE_FILE))
                    {
                        if (YesNo("Are you sure? Your save file will be deleted"))
                        {
                            System.IO.File.Delete(Constants.PATH_SAVE_FILE);
                            tetris.NewGame();
                            result = "Game";
                        }
                        else
                            result = "Menu";
                    }
                    else
                    {
                        tetris.NewGame();
                        result = "Game";
                    }
                    break;
                case 2:
                    newTetris = FileClass.LoadGame(Constants.PATH_SAVE_FILE);
                    if (newTetris != null)
                    {
                        result = "Game";
                        newTetris.gameRunning = true;
                        newTetris.result = "Game";
                    }
                    break;
                case 3:
                    result = "Leaderboard";
                    break;
                case 4:
                    result = Settings(result);
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine(Program.controlKeys);
                    Console.WriteLine();
                    Console.WriteLine("Click any key to return back...");
                    Console.ReadKey(true);
                    break;
                case 6:
                    result = "Exit";
                    Console.SetCursorPosition(0, 2 + Collections.MENU_WINDOW_LINES.Length);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Opens settings window
        /// </summary>
        /// <param name="preWindow">Previous window</param>
        /// <returns>Name of the next window</returns>
        public static string Settings(string preWindow)
        {
            PrintLabel("Settings", Collections.SETTINGS_WINDOW_LINES, out int cursorTop);
            Console.SetCursorPosition(0, cursorTop);
            CursorPosition(cursorTop, cursorTop + Collections.SETTINGS_WINDOW_LINES.Length - 1);
            string result = "Settings";
            switch (Console.CursorTop - 1)
            {
                case 1:
                    Program.controlKeys.UpdateKeys();
                    break;
                case 2:
                    if (YesNo("Are you sure you want to delete all data?"))
                    {
                        Program.controlKeys.SetDefaultKeys();
                        Program.leaderboard.Clear();
                        if (System.IO.File.Exists(Constants.PATH_SAVE_FILE))
                            System.IO.File.Delete(Constants.PATH_SAVE_FILE);
                        Console.SetCursorPosition(0, 2 + Collections.YES_NO_LINES.Length);
                        Console.WriteLine("Done");
                    }
                    else
                        Console.SetCursorPosition(0, 2 + Collections.YES_NO_LINES.Length);
                    Console.WriteLine();
                    Console.WriteLine("Click any key to return back...");
                    Console.ReadKey(true);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Author: {0}", Constants.AUTHOR);
                    Console.WriteLine("Version: {0}", Constants.VERSION);
                    Console.WriteLine("Release date: {0}", Constants.RELEASE_DATE);
                    Console.WriteLine();
                    Console.WriteLine("Click any key to return back...");
                    Console.ReadKey(true);
                    break;
                case 4:
                    result = preWindow;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Asks player to confirm his/her action
        /// </summary>
        /// <param name="text">Massage for player</param>
        /// <returns>True if agree</returns>
        public static bool YesNo(string text)
        {
            PrintLabel(text, Collections.YES_NO_LINES, out int cursorTop);
            Console.SetCursorPosition(0, cursorTop);
            CursorPosition(cursorTop, cursorTop + Collections.YES_NO_LINES.Length - 1);
            switch (Console.CursorTop - 1)
            {
                case 1:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Opens pause window 
        /// </summary>
        /// <param name="tetris"></param>
        /// <returns>Name of the next window</returns>
        public static string Pause(Tetris tetris)
        {
            PrintLabel("You paused the game", Collections.PAUSE_WINDOW_LINES, out int cursorTop);
            Console.SetCursorPosition(0, cursorTop);
            CursorPosition(cursorTop, cursorTop + Collections.PAUSE_WINDOW_LINES.Length - 1);

            string result = "Pause";
            switch (Console.CursorTop - 1)
            {
                case 1:
                    result = "Game";
                    tetris.gameRunning = true;
                    break;
                case 2:
                    FileClass.SaveGame(Constants.PATH_SAVE_FILE, tetris);
                    break;
                case 3:
                    result = Settings(result);
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine(Program.controlKeys);
                    Console.WriteLine();
                    Console.WriteLine("Click any key to return back...");
                    Console.ReadKey(true);
                    break;
                case 5:
                    result = "Menu";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Opens leaderboard window
        /// </summary>
        public static void Leaderboard()
        {
            Console.Clear();
            Console.WriteLine($"****** Leaderboard ******");
            Console.WriteLine();
            Console.WriteLine("    {0} {1, 15}", "Name", "Score");
            int index = 1;
            // Prints ranking
            foreach (var item in Program.leaderboard)
            {
                Console.Write("{0}.", index);
                Console.SetCursorPosition(4, Console.CursorTop);
                Console.WriteLine("{0} {1, 10}", item.Name, item.Score);
                index++;
            }
            Console.WriteLine();
            Console.WriteLine("Click any key to return back...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Printing template
        /// </summary>
        /// <param name="title">Title of the window</param>
        /// <param name="options">Array of options</param>
        private static void PrintLabel(string title, string[] options, out int cursorTop)
        {
            Console.Clear();
            Console.WriteLine($"****** {title} ******");
            Console.WriteLine();
            cursorTop = Console.CursorTop;
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"  {(i + 1)}. {options[i]}");
            }
        }

        /// <summary>
        /// Set the range where cursor can move
        /// </summary>
        /// <param name="min">Min line</param>
        /// <param name="max">Max line</param>
        private static void CursorPosition(int min, int max)
        {
            ConsoleKey consoleKey;
            do
            {
                consoleKey = Console.ReadKey(true).Key;
                switch (consoleKey)
                {
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop > min)
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        if (Console.CursorTop < max)
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
                        break;
                }
            } while (consoleKey != ConsoleKey.Enter);
        }
    }
}