using System;

namespace Tetris
{
    public class Point
    {
        public int Top { get; set; }
        public int Left { get; set; }

        public Point() { }
        public Point(int top, int left)
        {
            SetCoords(top, left);
        }

        public void SetCoords(int top, int left)
        {
            Top = top;
            Left = left;
        }

        public void SetCoords(string str)
        {
            if (str.Length != 2)
                throw new Exception("Invalid input to set coords");
            Top = SpecialMethods.CharToInt(str[0]);
            Left = SpecialMethods.CharToInt(str[1]);
        }
    }
}