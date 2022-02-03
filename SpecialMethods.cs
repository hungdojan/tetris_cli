using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    class SpecialMethods
    {
        /// <summary>
        /// Converts char to int
        /// </summary>
        /// <param name="c">Single digit</param>
        /// <returns>Converted digit</returns>
        public static int CharToInt(char c)
        {
            // if not a digit throw an exception
            if (!Char.IsDigit(c))
                throw new Exception("Input is not a digit!");
            return int.Parse(Convert.ToString(c));
        }

        /// <summary>
        /// Converts string of char into intArray
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] StringToIntArray(string str)
        {
            int[] arr = new int[str.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = CharToInt(str[i]);
            }
            return arr;
        }

        /// <summary>
        /// Updates single collision coords dictionary
        /// At first reset the dictionary, then use hashset to load rows/columns
        /// Search for max/min in each rows/columns and add back to dictionary
        /// </summary>
        /// <param name="arrOfPoints">Array of points of the block</param>
        /// <param name="topIndex">Index of array; top = true, left = false; true by default</param>
        /// <param name="max">To load max or min value; true by default</param>
        public static Point[] UpdateSingleCollisionCoords(Point[] arrOfPoints, bool topIndex = true, bool max = true)
        {
            SortedSet<int> hs = new SortedSet<int>();
            List<Point> list = new List<Point>();

            // Load index of rows/columns from arrOfPoints
            for (int i = 0; i < arrOfPoints.Length; i++)
            {
                hs.Add(!topIndex ? arrOfPoints[i].Top : arrOfPoints[i].Left);
            }

            // Add to dictionary with default value of 0
            foreach (var item in hs)
            {
                int value = max ? 0 : int.MaxValue; // Set default returning value
                for (int i = 0; i < arrOfPoints.GetLength(0); i++)
                {
                    // skips loop if item from hashset is not equal to given coords
                    if (item != (!topIndex ? arrOfPoints[i].Top : arrOfPoints[i].Left)) continue;
                    if (max)    // sets returning value
                    {
                        if ((topIndex ? arrOfPoints[i].Top : arrOfPoints[i].Left) > value)
                            value = (topIndex ? arrOfPoints[i].Top : arrOfPoints[i].Left);
                    }
                    else
                    {
                        if ((topIndex ? arrOfPoints[i].Top : arrOfPoints[i].Left) < value)
                            value = (topIndex ? arrOfPoints[i].Top : arrOfPoints[i].Left);
                    }
                }
                if (!topIndex)
                    list.Add(new Point(item, value));
                else
                    list.Add(new Point(value, item));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Checks if its possible to add block on the specific coords of topLeftCorner of the block 
        /// </summary>
        /// <param name="surface">Game board</param>
        /// <param name="block">Object Block</param>
        /// <param name="topLeftCorner">Coords of topLeftCorner</param>
        /// <returns>Returns false when blocks collides with other block/border of game board</returns>
        public static bool TryMove(Surface surface, Block block, Point topLeftCorner)
        {
            bool notCollide = true;
            block.Remove(surface);
            foreach (var point in block.coordsOfPoints)
            {
                if (topLeftCorner.Top + point.Top < 0 || topLeftCorner.Top + point.Top > surface.board.GetLength(0) - 1)
                {
                    notCollide = false;
                    break;
                }
                if (topLeftCorner.Left + point.Left < 0 || topLeftCorner.Left + point.Left > surface.board.GetLength(1) - 1)
                {
                    notCollide = false;
                    break;
                }
                if (surface.board[topLeftCorner.Top + point.Top, topLeftCorner.Left + point.Left] != '-')
                {
                    notCollide = false;
                    break;
                }
            }
            block.AddToBoard(surface);
            return notCollide;
        }

        /// <summary>
        /// Checks if its possible to move block's topLeftCorner to specific coords
        /// </summary>
        /// <param name="surface">Game board</param>
        /// <param name="block">Object Block</param>
        /// <param name="moveTop">Move topLeftCorner on top axis for specific amount of points</param>
        /// <param name="moveLeft">Move topLeftCorner on left axis for specific amount of points</param>
        /// <returns>Returns false when blocks collides with other block/border of game board</returns>
        public static bool TryMove(Surface surface, Block block, int moveTop, int moveLeft)
        {
            bool notCollide = true;
            block.Remove(surface);
            foreach (var point in block.coordsOfPoints)
            {
                if (block.topLeftCorner.Top + point.Top + moveTop < 0 ||
                block.topLeftCorner.Top + point.Top + moveTop > surface.board.GetLength(0) - 1)
                {
                    notCollide = false;
                    break;
                }
                if (block.topLeftCorner.Left + point.Left + moveLeft < 0 ||
                block.topLeftCorner.Left + point.Left + moveLeft > surface.board.GetLength(1) - 1)
                {
                    notCollide = false;
                    break;
                }
                if (surface.board[block.topLeftCorner.Top + point.Top + moveTop, block.topLeftCorner.Left + point.Left + moveLeft] != '-')
                {
                    notCollide = false;
                    break;
                }
            }
            block.AddToBoard(surface);
            return notCollide;
        }

        /// <summary>
        /// Checks for rotation collision
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="block"></param>
        /// <param name="rotation">Direction of rotation</param>
        /// <returns>True if no collision occurs when rotate</returns>
        public static bool TryRotate(Surface surface, Block block, Rotation rotation)
        {
            bool toRotate = true;
            var newBlock = new Block(block.coordsOfPoints, block.Type, block.IndexOfRotation, block.topLeftCorner);
            block.Remove(surface);
            switch (rotation)
            {
                case Rotation.CounterClockwise:
                    newBlock.RotateCounterClockwise();
                    break;
                case Rotation.Clockwise:
                    newBlock.RotateClockwise();
                    break;
            }

            foreach (var coords in newBlock.coordsOfPoints)
            {
                if (newBlock.topLeftCorner.Top + coords.Top < 0 ||
                newBlock.topLeftCorner.Top + coords.Top >= surface.board.GetLength(0) ||
                newBlock.topLeftCorner.Left + coords.Left < 0 ||
                newBlock.topLeftCorner.Left + coords.Left >= surface.board.GetLength(1))
                {
                    toRotate = false;
                    break;
                }
            }
            if (toRotate)
                if (!IsFree(surface, newBlock))
                    toRotate = false;

            switch (rotation)
            {
                case Rotation.Clockwise:
                    newBlock.RotateCounterClockwise();
                    break;
                case Rotation.CounterClockwise:
                    newBlock.RotateClockwise();
                    break;
            }
            block.AddToBoard(surface);
            return toRotate;
        }

        /// <summary>
        /// Checks if the board doesn't contain any block on specific location
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="block"></param>
        /// <returns>True if board is clean</returns>
        public static bool IsFree(Surface surface, Block block)
        {
            foreach (var coords in block.coordsOfPoints)
            {
                if (surface.board[block.topLeftCorner.Top + coords.Top, block.topLeftCorner.Left + coords.Left] != '-')
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if there is an event coresponding to user's input
        /// </summary>
        /// <returns>Event or empty string if command not found</returns>
        public static string KeyInput(ConsoleKey key)
        {
            if (key == Program.controlKeys.MoveLeft) return "MoveLeft";
            if (key == Program.controlKeys.MoveRight) return "MoveRight";
            if (key == Program.controlKeys.HoldBlock) return "HoldBlock";
            if (key == Program.controlKeys.RotateLeft) return "RotateLeft";
            if (key == Program.controlKeys.RotateRight) return "RotateRight";
            if (key == Program.controlKeys.SoftDrop) return "SoftDrop";
            if (key == Program.controlKeys.HardDrop) return "HardDrop";
            if (key == Program.controlKeys.Pause) return "Pause";
            else
                return "";
        }

        /// <summary>
        /// Checks if new score is high enough to be on leaderboard
        /// </summary>
        /// <param name="score">Last game's score</param>
        public static void LeaderboardSort(int score)
        {
            // If there are less that 10 score, adds automatically
            if (Program.leaderboard.Count < 10)
                AddScore(score);
            else
                if (Program.leaderboard.Last().Score < score)
            {
                Program.leaderboard.Remove(Program.leaderboard.Last());
                AddScore(score);
            }
            Program.leaderboard.Sort((a, b) => b.Score.CompareTo(a.Score));
        }

        /// <summary>
        /// Asks for player's name and adds to leaderboard
        /// </summary>
        /// <param name="score"></param>
        private static void AddScore(int score)
        {
            var name = "";
            do
            {
                Console.Write($"Name (max {Constants.MAX_CHAR} char.): ");
                name = Console.ReadLine();
            } while (name.Length < 1 || name.Length > Constants.MAX_CHAR);
            if (name.Length != Constants.MAX_CHAR)
            {
                var str = "";
                for (int i = name.Length; i < Constants.MAX_CHAR; i++)
                    str += " ";
                name += str;
            }
            Program.leaderboard.Add(new Leaderboard(name, score));
        }
    }
}