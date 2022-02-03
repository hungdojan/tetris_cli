using System;

namespace Tetris
{
    class Surface
    {
        public char[,] board;

        // To initialize object Surface
        public Surface()
        {
            board = new char[Constants.BOARD_HEIGHT, Constants.BOARD_WIDTH];
            BoardInit();
        }

        // To initialize board
        // adding special character to fill the array
        private void BoardInit()
        {
            Console.Clear();
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = '-';
                }
            }
        }

        // Prints a board
        public void PrintBoard()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Console.SetCursorPosition(Constants.BOARD_LEFT, Constants.BOARD_TOP + i);
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Checks how many lines are filled to be cleared
        /// Pushes down blocks to fill empty lines
        /// </summary>
        /// <returns>Number of cleared lines</returns>
        public int BoardCheck()
        {
            int lineCount = 0;
            // Starts checking from the bottom
            int line = board.GetLength(0) - 1;

            // Search for first filled line
            // Stops when line found, hits empty line or checks all of them
            while (!LineIsFilled(line))
            {
                if (LineIsEmpty(line))
                    return 0;
                else if (line == 0)
                    break;
                else
                    line--;
            }

            // Counts how many lines are filled (max 4)
            for (int i = 0; i < 5 && line - i >= 0; i++)
            {
                if (LineIsFilled(line - i))
                {
                    ResetLine(line - i);
                    lineCount++;
                }
            }

            // Push down blocks
            PushBlocksDown(line);
            return lineCount;
        }

        /// <summary>
        /// Checks if the line is filled
        /// </summary>
        /// <param name="line">Index of the line</param>
        /// <returns>True if line is completely filled</returns>
        private bool LineIsFilled(int line)
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                if (board[line, i] == '-')
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the line is empty
        /// </summary>
        /// <param name="line">Index of the line</param>
        /// <returns>True if line is completely empty</returns>
        private bool LineIsEmpty(int line)
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                if (board[line, i] != '-')
                {
                    return false;
                }
            }
            return true;
        }

        // Testing method
        public void PrintLine(int line)
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                Console.Write(board[line, i]);
            }
            Console.WriteLine();
        }

        // Fill empty lines by pushing down existing blocks
        private void PushBlocksDown(int indexOfFirstLine)
        {
            // check tetris
            int count = 0;
            int lineIndex = indexOfFirstLine;
            do
            {
                if (LineIsEmpty(lineIndex - count))
                    count++;

                else
                {
                    PushDownLine(lineIndex - count, lineIndex);
                    lineIndex--;
                }
            } while (count < 5 && lineIndex - count > 0);
        }

        public void PushDownLine(int lineFrom, int lineTo)
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                board[lineTo, i] = board[lineFrom, i];
                Console.SetCursorPosition(i * 2 + Constants.BOARD_LEFT, lineTo + Constants.BOARD_TOP);
                Console.WriteLine(board[lineFrom, i]);
            }
            ResetLine(lineFrom);
        }

        // Rewrite the line
        public void ResetLine(int line)
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                board[line, i] = '-';
                Console.SetCursorPosition(i * 2 + Constants.BOARD_LEFT, line + Constants.BOARD_TOP);
                Console.WriteLine("-");
            }
            Console.SetCursorPosition(0, Constants.RESET_CURSOR);
        }
    }
}