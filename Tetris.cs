using System;
using System.Threading;

namespace Tetris
{
    // Main game class
    class Tetris
    {
        public Surface Board { get; private set; }      // The game board
        public Block heldBlock;                        // Held Block
        public Block currentBlock;                     // Currently falling Block
        public int score;                               // Player's score
        public int level;                               // Current level
        private int timer;
        public int timer_cap = Constants.MAX_TIMER_COUNT;
        private string lastAction;
        private bool usedHeldAction;
        private bool checkAllow;
        private int[] startingPosition;
        public int lineCount;
        public bool gameRunning;
        public string result = "Game";

        // To initialize a game
        public Tetris()
        {
            int[] arr = SpecialMethods.StringToIntArray(Constants.STARTING_POSITION);
            startingPosition = new int[] { arr[0], arr[1] };
            NewGame();
        }

        /// <summary>
        /// Resets values
        /// </summary>
        public void NewGame()
        {
            Board = new Surface();
            heldBlock = null; currentBlock = null;
            score = 0; level = 1; timer = 0;
            lastAction = "";
            usedHeldAction = false;
            checkAllow = false;
            lineCount = 0;
            gameRunning = true;
        }

        /// <summary>
        /// Main game's method
        /// </summary>
        public string Game()
        {
            Console.Clear();
            UpdatePannel();
            Board.PrintBoard();
            Console.CursorVisible = false;
            if (currentBlock == null)
            {
                currentBlock = new Block(Collections.LIST_OF_PIECES[new Random().Next(0, 7)]);
                currentBlock.AddToBoard(Board, startingPosition);
            }
            while (gameRunning)
            {
                gameRunning = KeyAction();
                if (checkAllow)
                {
                    ScoreCount(score, level, lineCount, out score, out level, out lineCount);
                    checkAllow = false;
                    currentBlock = new Block(Collections.LIST_OF_PIECES[new Random().Next(0, 7)]);
                    currentBlock.SetTopLeft(startingPosition);
                    if (!SpecialMethods.IsFree(Board, currentBlock))
                    {
                        gameRunning = false;
                        result = "Menu";
                        break;
                    }
                    currentBlock.AddToBoard(Board);
                }
            }
            Console.CursorVisible = true;
            if (result == "Menu")
            {
                Console.WriteLine("Game Over!");
                Console.WriteLine($"Your score: {score}");
                Console.WriteLine($"Your level: {level}");
                Console.WriteLine($"You cleared {lineCount} line{(lineCount > 1 ? "s" : "")}");
                Console.WriteLine();
                SpecialMethods.LeaderboardSort(score);
                Console.WriteLine("Click any key to return to Menu");
                Console.ReadKey(true);

            }
            return result;
        }

        /// <summary>
        /// Checks for player's input and deals with timer
        /// if timer equals to Constants.MAX_TIMER_COUNT
        /// moves currentBlock (1, 0)
        /// </summary>
        /// <returns>Returns true if there is no more move and needs to check board for row cleared</returns>
        private bool KeyAction()
        {
            ConsoleKey key = ConsoleKey.NoName;
            if (!Console.KeyAvailable)
            {
                Thread.Sleep(Constants.DELAY);
                if (timer < timer_cap)
                    timer++;
                else
                {
                    if (SpecialMethods.TryMove(Board, currentBlock, 1, 0))
                        currentBlock.Move(Board, 1, 0);
                    else
                    {
                        checkAllow = true;
                        usedHeldAction = false;
                    }
                    timer = 0;
                }
                return true;
            }
            key = Console.ReadKey(true).Key;
            return EventCheck(key);
        }

        /// <summary>
        /// Main event method
        /// Does actions base on key input
        /// </summary>
        private bool EventCheck(ConsoleKey key)
        {
            string keyInput = SpecialMethods.KeyInput(key);
            switch (keyInput)
            {
                case "MoveLeft":
                    // Checks for collision, pass action if argument is false
                    if (SpecialMethods.TryMove(Board, currentBlock, 0, -1))
                        currentBlock.Move(Board, 0, -1);
                    break;
                case "MoveRight":
                    // Checks for collision, pass action if argument is false
                    if (SpecialMethods.TryMove(Board, currentBlock, 0, 1))
                        currentBlock.Move(Board, 0, 1);
                    break;
                case "HoldBlock":
                    // If player has already used this action in current play
                    // action is ignored
                    if (usedHeldAction)
                        break;

                    usedHeldAction = true;      // Changes value of usedHeldAction
                    if (heldBlock == null)
                    {
                        // If there wasn't saved any block to heldBlock
                        // assign currentBlock to heldBlock
                        // and generate a new block
                        currentBlock.Remove(Board);
                        heldBlock = currentBlock;
                        currentBlock = new Block(Collections.LIST_OF_PIECES[new Random().Next(0, 7)]);
                        currentBlock.SetTopLeft(startingPosition);
                        if (!SpecialMethods.IsFree(Board, currentBlock))
                            return false;
                        currentBlock.AddToBoard(Board);
                        timer = 0;
                    }
                    else
                    {
                        // If there is a block available in heldBlock
                        // swap values of currentBlock and heldBlock
                        currentBlock.Remove(Board);
                        var temp = currentBlock;
                        currentBlock = heldBlock;
                        heldBlock = temp;
                        currentBlock.SetTopLeft(startingPosition);
                        if (!SpecialMethods.IsFree(Board, currentBlock))
                            return false;
                        currentBlock.AddToBoard(Board);
                        timer = 0;
                    }
                    UpdatePannel();
                    break;
                case "RotateLeft":
                    if (currentBlock.Type == 'O')
                        break;

                    if (SpecialMethods.TryRotate(Board, currentBlock, Rotation.CounterClockwise))
                        currentBlock.Rotate(Board);

                    // If player's used rotation action as last action
                    // program ignores timer resetting
                    if (lastAction != "RotateLeft" && lastAction != "RotateRight" && lastAction != "MoveLeft" && lastAction != "MoveRight")
                        timer = 0;
                    break;
                case "RotateRight":
                    if (currentBlock.Type == 'O')
                        break;

                    if (SpecialMethods.TryRotate(Board, currentBlock, Rotation.Clockwise))
                        currentBlock.Rotate(Board, false);

                    // If player's used rotation action as last action
                    // program ignores timer resetting
                    if (lastAction != "RotateLeft" && lastAction != "RotateRight" && lastAction != "MoveLeft" && lastAction != "MoveRight")
                        timer = 0;
                    break;
                case "SoftDrop":
                    // Moves block down if possible
                    if (SpecialMethods.TryMove(Board, currentBlock, 1, 0))
                    {
                        currentBlock.Move(Board, 1, 0);
                        timer = 0;
                    }
                    break;
                case "HardDrop":
                    // Counts how deep can block drop and move it down
                    int i = 0;
                    while (true)
                    {
                        if (SpecialMethods.TryMove(Board, currentBlock, i, 0))
                            i++;
                        else
                            break;
                    }
                    currentBlock.Move(Board, i - 1, 0);
                    timer = 0;      // Resets timer
                    checkAllow = true;  // Allows program to check for row cleared
                    usedHeldAction = false;
                    break;
                case "Pause":
                    result = "Pause";
                    return false;
            }
            if (keyInput != "" && lastAction != keyInput)
                lastAction = keyInput;
            return true;
        }

        /// <summary>
        /// Method checks board thru Surface().BoardCheck() method
        /// Adds score, lineCount and level
        /// Returns new values back to old variables
        /// </summary>
        /// <param name="Score">Current player's score</param>
        /// <param name="Level">Current player's level</param>
        /// <param name="LineCount">Current player's row count</param>
        /// <param name="score">New player's score</param>
        /// <param name="level">New player's level</param>
        /// <param name="lineCount">New player's row count</param>
        private void ScoreCount(int Score, int Level, int LineCount, out int score, out int level, out int lineCount)
        {
            level = Level;
            score = Score;
            lineCount = LineCount;

            // Checks the board, returns how many rows were cleared
            int rowCleared = Board.BoardCheck();

            // increase score count by amounts of points times level
            score += Collections.POINTS_PER_ROW[rowCleared] * level;
            lineCount += rowCleared;         // increase lineCount by rowCleared
            level = lineCount / 10 + 1;      // for every 10 rows cleared increase level by one
            // FIXME: after level 10, the speed is accelerating
            // need to reset the "level counter" so that the speed changes only once per 10 levels
            // if (Constants.MAX_TIMER_COUNT > 0)
            timer_cap = Constants.MAX_TIMER_COUNT - 4 * (level / 4);
            if (rowCleared != 0)
                UpdatePannel();
        }

        /// <summary>
        /// Generate a new random block from list of possible blocks
        /// Sets coords of topLeftCorner
        /// </summary>
        /// <param name="startingPosition">Coords of topLeftCorner</param>
        /// <returns>Generated block</returns>
        private Block GenerateBlock(Point startingPosition)
        {
            var block = new Block(Collections.LIST_OF_PIECES[new Random().Next(0, 7)]);
            block.topLeftCorner = startingPosition;
            return block;
        }

        /// <summary>
        /// Generate a specific block from Collections.TYPE_OF_ROTATION
        /// Sets coords of topLeftCorner
        /// </summary>
        /// <param name="startingPosition">Coords of topLeftCorner</param>
        /// <param name="blockType">Specific block from collection</param>
        /// <returns>Generated block</returns>
        private Block GenerateBlock(Point startingPosition, string blockType)
        {
            var block = new Block(blockType);
            block.topLeftCorner = startingPosition;
            return block;
        }

        /// <summary>
        /// Prints status pannel
        /// </summary>
        private void UpdatePannel()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Held piece: {0, -10} {1}: {2}", (heldBlock != null ? Convert.ToString(heldBlock.Type) : "null"), "Score", score);
            Console.WriteLine("Level: {0, -10} {1}: {2}", level, "Line cleared", lineCount);
            Console.SetCursorPosition(0, Constants.RESET_CURSOR);
        }
    }
}
