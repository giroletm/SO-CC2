using SO_CC2.Utils;
using System.Drawing;
using System.Numerics;

namespace SO_CC2.CLI
{
    internal static class Program
    {
        /// <summary>
        /// The character set to be used as a base to encode permutation IDs
        /// </summary>
        private static string CharacterSet { get; set; } = " ABCDEFGHIJKLMNOPQRSTUVWXYZ!\"'()*+,-.0123456789:?";

        /// <summary>
        /// Dictionary holding each playing pawn's position by indexing its internal character with its square number
        /// </summary>
        private static Dictionary<char, int> PlayerPositions { get; set; } = new();

        private static string _currentCode = "";

        /// <summary>
        /// The message currently displayed on screen
        /// </summary>
        private static string CurrentCode
        {
            get => _currentCode;
            set
            {
                string newText = new string(value.ToUpper().ToCharArray().Where(c => CharacterSet.Contains(c)).ToArray());

                // If the inputed code is greater than the max, restore the previous input

                BigInteger textAsNum = BaseHelper.FromBase(newText, CharacterSet);
                if (textAsNum >= Permutator.GetTotalPermutations(Cluedo.LEXICO_BASE))
                    newText = _currentCode;

                // Set the code to use to the computed one if needed, and try not to break the selection cursor

                if (newText == _currentCode)
                    return;

                _currentCode = newText; // This will call this function again, so to avoid rendering twice, we return once we're done cleaning up the text

                // Compute the new permutation the inputed string
                string newPermutation = Permutator.IndexToPermutation(Cluedo.LEXICO_BASE, BaseHelper.FromBase(_currentCode.ToUpper(), CharacterSet));

                // Recalculate players positions according to the computed permutation
                PlayerPositions = Cluedo.PAWNS.ToDictionary(p => p, p => newPermutation.IndexOf(p));

                // Render the players
                RenderBoard();
            }
        }

        private static void Main(string[] args)
        {
            CurrentCode = Cluedo.EXAMPLES[(new Random()).Next(Cluedo.EXAMPLES.Length)];
        }

        private static void RenderBoard()
        {
            (int cLeft, int cTop) = Console.GetCursorPosition();

            Dictionary<char, Point> playerPos = PlayerPositions.ToDictionary(kvp => kvp.Key, kvp => Cluedo.SquareToCoordinates(kvp.Value));

            Console.SetCursorPosition(0, 0);

            for(int y = 0; y < Cluedo.SQUARES_MATRIX.GetLength(0); y++)
            {
                for (int x = 0; x < Cluedo.SQUARES_MATRIX.GetLength(1); x++)
                {
                    char potentialPlayer = playerPos.Where(p => p.Value.X == x && p.Value.Y == y).Select(p => p.Key).FirstOrDefault(' ');

                    if (potentialPlayer != ' ')
                    {
                        Console.BackgroundColor = potentialPlayer switch
                        {
                            '1' => ConsoleColor.Red,
                            '2' => ConsoleColor.Yellow,
                            '3' => ConsoleColor.White,
                            '4' => ConsoleColor.Green,
                            '5' => ConsoleColor.Blue,
                            '6' => ConsoleColor.DarkMagenta,
                            _ => throw new Exception("Unsupported pawn color")
                        };
                        Console.Write(' ' /*potentialPlayer*/);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (Cluedo.SQUARES_MATRIX[y, x] == 0)
                        Console.Write('\u2588'); // █
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }
}
