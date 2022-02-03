namespace Tetris
{
    class Leaderboard
    {
        public string Name { get; private set; }
        public int Score { get; private set; }

        public Leaderboard(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}