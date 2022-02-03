using System;
using Newtonsoft.Json;
using System.IO;

namespace Tetris
{
    static class FileClass
    {
        // Loads saved game control keys from the file
        // If there is no such file, creates a new one with default keys
        // Can be change later with GameControl.Update() method
        public static GameControl ReadControlKeys(string path)
        {
            if (!File.Exists(path))
            {
                var product = new GameControl(ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.C, ConsoleKey.Z, ConsoleKey.X, ConsoleKey.DownArrow, ConsoleKey.Spacebar, ConsoleKey.P);
                string output = JsonConvert.SerializeObject(product);
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(output.ToString());
                    sw.Close();
                }
                return product;
            }
            // If the file already exists then just read it
            else
            {
                GameControl obj;
                using (var file = File.OpenText(path))
                {
                    var serializer = new JsonSerializer();
                    obj = (GameControl)serializer.Deserialize(file, typeof(GameControl));
                }
                return obj;
            }
        }

        // Rewrite the file with control keys
        // If the file doesn't exist than creates a new one from gameControl object
        public static void WriteControlKeys(string path)
        {
            string output = JsonConvert.SerializeObject(Program.controlKeys);
            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine(output.ToString());
                sw.Close();
            }
        }

        /// <summary>
        /// Loads leaderboard
        /// </summary>
        /// <param name="path"></param>
        public static void ReadHighScore(string path)
        {
            if (!File.Exists(path))
                using (var sw = new StreamWriter(path)) { }
            using (var sr = new StreamReader(path))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    var line = str.Split(';');
                    Program.leaderboard.Add(new Leaderboard(line[0], int.Parse(line[1])));
                }
            }
        }

        /// <summary>
        /// Saves leaderboard
        /// </summary>
        /// <param name="path"></param>
        public static void WriteHighScore(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                foreach (var item in Program.leaderboard)
                {
                    sw.WriteLine(item.Name + ";" + item.Score);
                }
                sw.Close();
            }
        }

        /// <summary>
        /// Saves game
        /// </summary>
        /// <param name="path"></param>
        /// <param name="tetris"></param>
        public static void SaveGame(string path, Tetris tetris)
        {
            Console.Clear();
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine($"{(tetris.heldBlock != null ? tetris.heldBlock.Type : 'N')};{tetris.score};{tetris.lineCount};{Constants.MAX_TIMER_COUNT}");
                tetris.currentBlock.RemoveM(tetris.Board);
                for (int i = 0; i < tetris.Board.board.GetLength(0); i++)
                {
                    var str = "";
                    for (int j = 0; j < tetris.Board.board.GetLength(1); j++)
                    {
                        str += tetris.Board.board[i, j];
                    }
                    sw.WriteLine(str);
                }
                tetris.currentBlock.AddToBoardM(tetris.Board);
                sw.Close();
            }
            Console.WriteLine("Saving game...");
            Console.WriteLine("Dont turn of the program...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Click any key to return back...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Loads game and stats
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Tetris LoadGame(string path)
        {
            Console.Clear();
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found");
                Console.WriteLine("Click any key to return back");
                Console.ReadKey(true);
                return null;
            }
            var tetris = new Tetris();
            char[,] charArray;
            using (var sr = new StreamReader(path))
            {
                try
                {
                    // Load stats
                    var stats = sr.ReadLine().Split(';');
                    if (stats[0] == "N")
                        tetris.heldBlock = null;
                    else
                    {
                        foreach (var item in Collections.LIST_OF_PIECES)
                        {
                            if (item.Contains(stats[0]))
                            {
                                tetris.heldBlock = new Block(item);
                                break;
                            }
                        }
                    }
                    tetris.score = int.Parse(stats[1]);
                    tetris.lineCount = int.Parse(stats[2]);
                    tetris.level = tetris.lineCount / 10 + 1;
                    tetris.timer_cap = int.Parse(stats[3]);
                    // Load board
                    charArray = new char[Constants.BOARD_HEIGHT, Constants.BOARD_WIDTH];
                    for (int i = 0; i < charArray.GetLength(0); i++)
                    {
                        var line = sr.ReadLine().ToCharArray();
                        for (int j = 0; j < line.Length; j++)
                        {
                            charArray[i, j] = line[j];
                        }
                    }
                    tetris.Board.board = charArray;
                }
                catch
                {
                    Console.WriteLine("File is corrupted!");
                    Console.WriteLine("Can't load a game");
                    tetris = null;
                    Console.WriteLine();
                    Console.WriteLine("Click any key to return back...");
                    Console.ReadKey(true);
                }
            }
            return tetris;
        }
    }
}