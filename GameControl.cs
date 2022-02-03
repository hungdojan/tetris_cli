using System;

namespace Tetris
{
    class GameControl
    {
        public ConsoleKey MoveLeft { get; set; }
        public ConsoleKey MoveRight { get; set; }
        public ConsoleKey HoldBlock { get; set; }
        public ConsoleKey RotateLeft { get; set; }
        public ConsoleKey RotateRight { get; set; }
        public ConsoleKey SoftDrop { get; set; }
        public ConsoleKey HardDrop { get; set; }
        public ConsoleKey Pause { get; set; }

        public GameControl() { }

        public GameControl(int moveLeft, int moveRight, int holdBlock, int rotateLeft, int rotateRight, int softDrop, int hardDrop, int pause)
        {
            MoveLeft = (ConsoleKey)moveLeft;
            MoveRight = (ConsoleKey)moveRight;
            HoldBlock = (ConsoleKey)holdBlock;
            RotateLeft = (ConsoleKey)rotateLeft;
            RotateRight = (ConsoleKey)rotateRight;
            SoftDrop = (ConsoleKey)softDrop;
            HardDrop = (ConsoleKey)hardDrop;
            Pause = (ConsoleKey)pause;
        }

        public GameControl(ConsoleKey moveLeft, ConsoleKey moveRight, ConsoleKey holdBlock, ConsoleKey rotateLeft, ConsoleKey rotateRight, ConsoleKey softDrop, ConsoleKey hardDrop, ConsoleKey pause)
        {
            MoveLeft = moveLeft;
            MoveRight = moveRight;
            HoldBlock = holdBlock;
            RotateLeft = rotateLeft;
            RotateRight = rotateRight;
            SoftDrop = softDrop;
            HardDrop = hardDrop;
            Pause = pause;
        }

        // To update control keys
        public void UpdateKeys()
        {
            Console.Clear();
            Console.WriteLine("Choose control keys for: ");
            // Move block left
            Console.Write("Move block left: ");
            MoveLeft = Console.ReadKey(true).Key;
            Console.WriteLine(MoveLeft);
            // Move block right
            Console.Write("Move block right: ");
            MoveRight = Console.ReadKey(true).Key;
            Console.WriteLine(MoveRight);
            // Hold a block
            Console.Write("Hold a block: ");
            HoldBlock = Console.ReadKey(true).Key;
            Console.WriteLine(HoldBlock);
            // Rotate block left
            Console.Write("Rotate block left: ");
            RotateLeft = Console.ReadKey(true).Key;
            Console.WriteLine(RotateLeft);
            // Rotate block right
            Console.Write("Rotate block right: ");
            RotateRight = Console.ReadKey(true).Key;
            Console.WriteLine(RotateRight);
            // Soft drop
            Console.Write("Soft drop: ");
            SoftDrop = Console.ReadKey(true).Key;
            Console.WriteLine(SoftDrop);
            // Hard drop
            Console.Write("Hard drop: ");
            HardDrop = Console.ReadKey(true).Key;
            Console.WriteLine(HardDrop);
        }

        // Sets back default keys
        public void SetDefaultKeys()
        {
            MoveLeft = ConsoleKey.LeftArrow;
            MoveRight = ConsoleKey.RightArrow;
            HoldBlock = ConsoleKey.C;
            RotateLeft = ConsoleKey.Z;
            RotateRight = ConsoleKey.X;
            SoftDrop = ConsoleKey.DownArrow;
            HardDrop = ConsoleKey.Spacebar;
            Pause = ConsoleKey.P;
        }

        // Prints all control keys
        public override string ToString()
        {
            return $"MoveLeft: {MoveLeft}\nMoveRight: {MoveRight}\nHoldBlock: {HoldBlock}\nRotateLeft: {RotateLeft}\nRotateRight: {RotateRight}\nSoftDrop: {SoftDrop}\nHardDrop: {HardDrop}\nPause: {Pause}";
        }
    }
}