using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Tetris
{
    static class Collections
    {
        // Read only array with points per numbers of rows
        // for one row it's 40p, for 2 rows it's 100p etc.
        // it also depends on current lvl
        // final point count is - POINTS_PER_ROW * CURRENT_LVL
        public static readonly int[] POINTS_PER_ROW = { 0, 40, 100, 300, 1200 };
        public static readonly string[] LIST_OF_PIECES = {
            "00 01 10 11 O 3", // O
            "00 10 20 21 L 1", // L
            "01 11 20 21 J 1", // J
            "00 10 20 30 I 1", // I
            "00 10 11 21 S 1", // S
            "01 10 11 20 Z 1", // Z
            "01 10 11 21 T 2"  // T
        };
        public static readonly string[] MENU_WINDOW_LINES = { "New game", "Load game", "Leaderboard", "Settings", "Help", "Exit" };
        public static readonly string[] SETTINGS_WINDOW_LINES = { "Change control keys", "Reset data", "About", "Back" };
        public static readonly string[] PAUSE_WINDOW_LINES = { "Continue", "Save", "Settings", "Help", "Return to Menu" };
        public static readonly string[] YES_NO_LINES = { "Yes", "No" };

        // For testing
        public static ReadOnlyDictionary<char, List<string>> TYPES_OF_ROTATION = new ReadOnlyDictionary<char, List<string>>(new Dictionary<char, List<string>> {
            {'O', new List<string> {"00 01 10 11"}},
            {'L', new List<string> {"00 10 20 21", "00 01 02 10", "00 01 11 21", "02 10 11 12"}},
            {'J', new List<string> {"01 11 20 21", "00 10 11 12", "00 01 10 20", "00 01 02 12"}},
            {'I', new List<string> {"00 10 20 30", "00 01 02 03"}},
            {'S', new List<string> {"00 10 11 21", "01 02 10 11"}},
            {'2', new List<string> {"01 10 11 20", "00 01 11 12"}},
            {'T', new List<string> {"01 10 11 21", "01 10 11 12", "00 10 11 20", "00 01 02 11"}}
        });
    }
}