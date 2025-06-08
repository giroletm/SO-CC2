using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SO_CC2.Utils
{
    public static class Cluedo
    {
        #region Constants

        /// <summary>
        /// The examples provided in the challenge's instructions
        /// </summary>
        public static readonly string[] EXAMPLES = { "TREASURE", "LOOKLEFT", "SECRETED", "DIGHERE!", "TOMORROW" };

        /// <summary>
        /// The squares of a classic Cluedo board. <c>1</c>s represent walkable squares, <c>0</c>s represent non-walkable ones.
        /// </summary>
        public static readonly int[,] SQUARES_MATRIX =
        {
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
        };

        /// <summary>
        /// The characters representing each pawn internally.
        /// There's 6 of them on a classic Cluedo board.
        /// </summary>
        public const string PAWNS = "123456";

        /// <summary>
        /// The number of walkable squares on the Cluedo board.
        /// Calculated at runtime from <see cref="SQUARES_MATRIX"/>.
        /// </summary>
        public static readonly int SQUARES_COUNT = SQUARES_MATRIX.Cast<int>().Count(sq => sq == 1);

        /// <summary>
        /// Base permutation to represent pawns on the Cluedo board
        /// </summary>
        public static readonly string LEXICO_BASE = new string(' ', SQUARES_COUNT - PAWNS.Length) + PAWNS;

        #endregion


        #region Board utility functions

        /// <summary>
        /// Computes the coordinates on the board of the <paramref name="squareId"/>-th square.
        /// <br/>
        /// Acts as the reverse of <see cref="CoordinatesToSquare"/>.
        /// </summary>
        /// <param name="squareId">The square number to compute the coordinates of</param>
        /// <returns>The coordinates on the board of the <paramref name="squareId"/>-th square</returns>
        public static Point SquareToCoordinates(int squareId)
        {
            int height = Cluedo.SQUARES_MATRIX.GetLength(0);
            int width = Cluedo.SQUARES_MATRIX.GetLength(1);

            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (Cluedo.SQUARES_MATRIX[y, x] > 0)
                    {
                        if (i == squareId)
                            return new Point(x, y);

                        i++;
                    }
                }
            }

            return new Point(-1, -1);
        }

        /// <summary>
        /// Computes the square number that corresponds to <paramref name="coords"/>.
        /// <br/>
        /// Acts as the reverse of <see cref="SquareToCoordinates"/>.
        /// </summary>
        /// <param name="coords">The coordinates to compute the square number of</param>
        /// <returns>The square number that corresponds to <paramref name="coords"/></returns>
        public static int CoordinatesToSquare(Point coords)
        {
            if (Cluedo.SQUARES_MATRIX[coords.Y, coords.X] == 0)
                return -1;

            return Cluedo.SQUARES_MATRIX.Cast<int>().Take(coords.Y * Cluedo.SQUARES_MATRIX.GetLength(1) + coords.X).Count(sq => sq == 1);
        }

        #endregion
    }
}
