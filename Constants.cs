namespace Tetris
{
    static class Constants
    {
        public const string VERSION = "v1.2";
        public const string AUTHOR = "Rebulien";
        public const string RELEASE_DATE = "16.04.2020";
        public const string PATH_CONTROL_KEYS = @"game_control.json";
        public const string PATH_LEADERBOARD = @"leaderboard.txt";
        public const string PATH_SAVE_FILE = @"savefile.txt";
        
        public const int WINDOW_HEIGHT = BOARD_TOP + BOARD_HEIGHT + 3;
        public const int WINDOW_WIDTH = 40;
        public const int BOARD_WIDTH = 10;
        public const int BOARD_HEIGHT = 20;
        public const int DELAY = 20;
        public const int MAX_TIMER_COUNT = 24;
        private const int PANNEL_HEIGHT = 3;
        public const int MAX_CHAR = 10;

        public const string STARTING_POSITION = "04";
        public static Point STARTING_POS = new Point(0, 4);

        public const int PANNEL_TOP = 0;
        public const int BOARD_TOP = PANNEL_HEIGHT;
        public const int BOARD_LEFT = (WINDOW_WIDTH - 2 * BOARD_WIDTH) / 2;
        public const int RESET_CURSOR = BOARD_TOP + BOARD_HEIGHT;
    }

    enum Rotation
    {
        Clockwise,
        CounterClockwise
    }
}