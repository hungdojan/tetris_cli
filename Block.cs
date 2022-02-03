using System;

namespace Tetris
{
    class Block
    {
        public char Type { get; private set; }

        /// <summary>
        /// List of coordinates of points of the block
        /// </summary>
        public Point[] coordsOfPoints;

        /// <summary>
        /// Coordinates of the top left corner of the block
        /// </summary>
        public Point topLeftCorner;

        /// <summary>
        /// Center point of rotation
        /// Doesn't move during rotation
        /// </summary>
        public Point pointOfRotation;

        // Collections of most left/right/down points for collision check
        public Point[] leftCollisionCoords;   // <top, left>
        public Point[] rightCollisionCoords;  // <top, left>
        public Point[] downCollisionCoords;   // <left, top>
        public int IndexOfRotation { get; private set; }

        public Block() { }
        /// <summary>
        /// To initialize Block
        /// </summary>
        /// <param name="input">String input with 3 parameters: 
        /// coordinates of points, type/shape of the block and index of point of rotation</param>
        public Block(string input)
        {
            var temp = input.Split(' ');
            coordsOfPoints = new Point[temp.Length - 2];
            SetCoord(temp);

            topLeftCorner = new Point();

            // Init point of rotation
            IndexOfRotation = int.Parse(temp[temp.Length - 1]);
            pointOfRotation = new Point();
            pointOfRotation.SetCoords(temp[IndexOfRotation]);

            Type = temp[temp.Length - 2][0];
            UpdateCollisionCoords();
        }

        /// <summary>
        /// To initialize Block
        /// </summary>
        /// <param name="coordsOfPoints">Coordinates of points</param>
        /// <param name="type">Type/shape of the block</param>
        /// <param name="indexOfRotation">Index of point of rotation in variable coordsOfPoints</param>
        public Block(Point[] coordsOfPoints, char type, int indexOfRotation, Point topLeftCorner = null)
        {
            Type = type;
            this.coordsOfPoints = coordsOfPoints;

            this.topLeftCorner = topLeftCorner == null ? new Point() : topLeftCorner;
            pointOfRotation = new Point();
            pointOfRotation.Top = coordsOfPoints[indexOfRotation].Top;
            pointOfRotation.Left = coordsOfPoints[indexOfRotation].Left;
            IndexOfRotation = indexOfRotation;
        }

        /// <summary>
        /// To initialize Block
        /// </summary>
        /// <param name="coordsOfPointsString">Coordinates of points in string format
        /// split by space (char ' ')</param>
        /// <param name="type">Type/shape of the block</param>
        /// <param name="indexOfRotation">Index of point of rotation in array of coordsOfPoints</param>
        /// <param name="topLeftCorner">Coords of top left corner</param>
        public Block(string coordsOfPointsString, char type, int indexOfRotation, Point topLeftCorner = null)
        {
            Type = type;
            var temp = coordsOfPointsString.Split(' ');
            coordsOfPoints = new Point[temp.Length];
            SetCoord(temp);

            this.topLeftCorner = topLeftCorner == null ? new Point() : topLeftCorner;
            pointOfRotation = new Point();
            pointOfRotation.Top = coordsOfPoints[indexOfRotation].Top;
            pointOfRotation.Left = coordsOfPoints[indexOfRotation].Left;
            IndexOfRotation = indexOfRotation;
        }

        /// <summary>
        /// Set coords for every points of the block to create whole block
        /// Init array coordsOfPoints
        /// </summary>
        /// <param name="array">String array with coords</param>
        private void SetCoord(string[] array)
        {
            for (int i = 0; i < coordsOfPoints.Length; i++)
            {
                if (coordsOfPoints[i] == null)
                    coordsOfPoints[i] = new Point();
                coordsOfPoints[i].SetCoords(array[i]);
            }
        }

        /// <summary>
        /// Sets coord of top left point
        /// </summary>
        /// <param name="topLeft">Array.Length = 2, coords of point</param>
        public void SetTopLeft(int[] topLeft)
        {
            topLeftCorner.Top = topLeft[0];
            topLeftCorner.Left = topLeft[1];
        }

        /// <summary>
        /// To rotate an object
        /// </summary>
        /// <param name="board">Game board</param>
        /// <param name="counterClockwise">true if rotate point, else false (true as default)</param>
        public void Rotate(Surface surface, bool counterClockwise = true)
        {
            Remove(surface);

            // To rotate each point of the block
            if (counterClockwise)
                RotateCounterClockwise();
            else
                RotateClockwise();

            UpdateCollisionCoords();
            AddToBoard(surface);
        }

        /// <summary>
        /// Rotate a specific point counterclockwise
        /// </summary>
        public void RotateCounterClockwise()
        {
            for (int i = 0; i < coordsOfPoints.Length; i++)
            {
                int difTop = pointOfRotation.Top - coordsOfPoints[i].Top;
                int difLeft = pointOfRotation.Left - coordsOfPoints[i].Left;
                coordsOfPoints[i].Top = pointOfRotation.Top + difLeft;
                coordsOfPoints[i].Left = pointOfRotation.Left - difTop;
            }
        }

        /// <summary>
        /// Rotate a specific point clockwise
        /// </summary>
        public void RotateClockwise()
        {
            for (int i = 0; i < coordsOfPoints.Length; i++)
            {
                int difTop = pointOfRotation.Top - coordsOfPoints[i].Top;
                int difLeft = pointOfRotation.Left - coordsOfPoints[i].Left;
                coordsOfPoints[i].Top = pointOfRotation.Top - difLeft;
                coordsOfPoints[i].Left = pointOfRotation.Left + difTop;
            }
        }

        /// <summary>
        /// Adds a block to the board to the exact position
        /// </summary>
        /// <param name="surface">Game board</param>
        /// <param name="top">Exact coord for placement (top = y)</param>
        /// <param name="left">Exact coord for placement (left = x)</param>
        public void AddToBoard(Surface surface, int[] topLeft)
        {
            SetTopLeft(topLeft);
            AddToBoard(surface);
        }

        /// <summary>
        /// Adds a block to the board, coords used are defined in variable topLeftCorner
        /// </summary>
        /// <param name="surface">Game board</param>
        public void AddToBoard(Surface surface)
        {
            for (int i = 0; i < coordsOfPoints.Length; i++)   // kolikaty block 
            {
                surface.board[topLeftCorner.Top + coordsOfPoints[i].Top, topLeftCorner.Left + coordsOfPoints[i].Left] = Type;
                Console.SetCursorPosition((topLeftCorner.Left + coordsOfPoints[i].Left) * 2 + Constants.BOARD_LEFT, topLeftCorner.Top + coordsOfPoints[i].Top + Constants.BOARD_TOP);
                Console.WriteLine(Type);
            }
            Console.SetCursorPosition(0, Constants.RESET_CURSOR);
        }

        /// <summary>
        /// Adds block on board but not print it
        /// </summary>
        /// <param name="surface"></param>
        public void AddToBoardM(Surface surface)
        {
            for (int i = 0; i < coordsOfPoints.Length; i++)   // kolikaty block 
                surface.board[topLeftCorner.Top + coordsOfPoints[i].Top, topLeftCorner.Left + coordsOfPoints[i].Left] = Type;
        }

        /// <summary>
        /// Move a block relative to its current position
        /// </summary>
        /// <param name="surface">Game board</param>
        /// <param name="top">Relative coords for placement (example: 2 means 2 points down)</param>
        /// <param name="left">Relative coords for placement (example: 2 means 2 points right)</param>
        public void Move(Surface surface, int top, int left)
        {
            Remove(surface);

            // Set new position
            topLeftCorner.Top += top;
            topLeftCorner.Left += left;

            // Add block with new position on the board
            AddToBoard(surface);
        }

        /// <summary>
        /// Move block to the fixed position
        /// </summary>
        /// <param name="surface">Game board</param>
        /// <param name="top">Exact coords for placement (top = y)</param>
        /// <param name="left">Exact coords for placement (left = x)</param>
        public void MoveTo(Surface surface, int top, int left)
        {
            Remove(surface);

            // Set new position
            topLeftCorner.Top = top;
            topLeftCorner.Left = left;

            // Add block with new position on the board
            AddToBoard(surface);
        }

        /// <summary>
        /// Removes existing block from the block
        /// </summary>
        /// <param name="surface">Game board</param>
        public void Remove(Surface surface)
        {
            for (int i = 0; i < coordsOfPoints.Length; i++)   // kolikaty block 
            {
                surface.board[topLeftCorner.Top + coordsOfPoints[i].Top, topLeftCorner.Left + coordsOfPoints[i].Left] = '-';
                Console.SetCursorPosition((topLeftCorner.Left + coordsOfPoints[i].Left) * 2 + Constants.BOARD_LEFT, topLeftCorner.Top + coordsOfPoints[i].Top + Constants.BOARD_TOP);
                Console.WriteLine('-');
            }
            Console.SetCursorPosition(0, Constants.RESET_CURSOR);
        }

        /// <summary>
        /// Remove block from board but not print it
        /// </summary>
        /// <param name="surface"></param>
        public void RemoveM(Surface surface)
        {
            for (int i = 0; i < coordsOfPoints.Length; i++)   // kolikaty block 
                surface.board[topLeftCorner.Top + coordsOfPoints[i].Top, topLeftCorner.Left + coordsOfPoints[i].Left] = '-';
        }

        /// <summary>
        /// Updates all collision coords
        /// </summary>
        private void UpdateCollisionCoords()
        {
            leftCollisionCoords = SpecialMethods.UpdateSingleCollisionCoords(coordsOfPoints, topIndex: false, max: false);
            rightCollisionCoords = SpecialMethods.UpdateSingleCollisionCoords(coordsOfPoints, topIndex: false);
            downCollisionCoords = SpecialMethods.UpdateSingleCollisionCoords(coordsOfPoints);
        }

        public override string ToString()
        {
            var str = "";
            foreach (var item in coordsOfPoints)
            {
                str += Convert.ToString(item.Top) + Convert.ToString(item.Left) + " ";
            }
            return $"Main points coords: {topLeftCorner.Top}{topLeftCorner.Left}; Block's shape: {str}; Point of rotation: {pointOfRotation.Top}{pointOfRotation.Left}";
        }
    }
}