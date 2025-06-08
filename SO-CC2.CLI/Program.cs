using SO_CC2.Utils;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;

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

        private static int MaxChars => 1 + (int)Math.Floor(BigInteger.Log(MathHelper.Factorial(Cluedo.SQUARES_COUNT) / MathHelper.Factorial(Cluedo.SQUARES_COUNT - Cluedo.PAWNS.Length)) / BigInteger.Log(CharacterSet.Length));

        private static void Main(string[] args)
        {
            CurrentCode = Cluedo.EXAMPLES[(new Random()).Next(Cluedo.EXAMPLES.Length)];
            Console.WriteLine();

            int afterBoard = Console.CursorTop;

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Message: ");
            (int messageLeft, int messageTop) = Console.GetCursorPosition();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(CurrentCode);

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Character set: ");
            (int charsetLeft, int charsetTop) = Console.GetCursorPosition();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(CharacterSet);

            (int menuLeft, int menuTop) = Console.GetCursorPosition();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press TAB for the menu. Press ESC to close the program.");

            int bottomestTop = Console.CursorTop;

            Console.ResetColor();

            int status = EditMessageState(messageLeft, messageTop);
            while(status > 0)
            {
                Console.ResetColor();

                switch (status)
                {
                    case 1:
                        status = EditMessageState(messageLeft, messageTop);
                        break;
                    case 2:
                        status = EditCharsetState(charsetLeft, charsetTop, messageLeft, messageTop);
                        break;
                    case 3:
                        status = MovePawnsState(menuLeft, menuTop, messageLeft, messageTop);
                        break;
                    default:
                        status = -1;
                        break;
                }
            }

            Console.ResetColor();
            Console.SetCursorPosition(0, bottomestTop);
        }

        private static void RerenderMessageTextbox(int messageLeft, int messageTop, ConsoleColor backgroundColor)
        {
            (int cLeft, int cTop) = Console.GetCursorPosition();
            ConsoleColor srcBColor = Console.BackgroundColor;
            ConsoleColor srcFColor = Console.ForegroundColor;

            Console.SetCursorPosition(messageLeft, messageTop);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = backgroundColor;
            Console.Write(CurrentCode.PadLeft(MaxChars, CharacterSet[0]));

            if (cTop != messageTop && cLeft != messageLeft)
            {
                Console.ForegroundColor = srcFColor;
                Console.BackgroundColor = srcBColor;
                Console.SetCursorPosition(cLeft, cTop);
            }
        }

        private static int EditMessageState(int messageLeft, int messageTop) // State 1
        {
            Console.SetCursorPosition(messageLeft, messageTop);
            RerenderMessageTextbox(messageLeft, messageTop, ConsoleColor.DarkGreen);

            while (true)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);

                if (pressed.Key == ConsoleKey.Escape)
                    return -1;

                if (pressed.Key == ConsoleKey.DownArrow || pressed.Key == ConsoleKey.Tab)
                {
                    RerenderMessageTextbox(messageLeft, messageTop, ConsoleColor.Red);

                    return (pressed.Key == ConsoleKey.DownArrow) ? 2 : 3;
                }

                if(pressed.Key == ConsoleKey.Backspace || (pressed.Key == ConsoleKey.Delete && Console.CursorLeft < messageLeft + MaxChars))
                {
                    if(pressed.Key == ConsoleKey.Delete)
                        Console.CursorLeft++;

                    if (Console.CursorLeft > messageLeft && CurrentCode.Length > 0)
                    {
                        int currIdx = Console.CursorLeft - messageLeft;
                        int realIdx = currIdx - 1 - (MaxChars - CurrentCode.Length);
                        if (realIdx >= 0)
                        {
                            CurrentCode = CurrentCode.Remove(realIdx, 1);
                            RerenderMessageTextbox(messageLeft, messageTop, ConsoleColor.DarkGreen);
                        }
                    }
                }
                else if(pressed.Key == ConsoleKey.LeftArrow)
                {
                    if (Console.CursorLeft > messageLeft + (MaxChars - CurrentCode.Length))
                        Console.CursorLeft--;
                }
                else if(pressed.Key == ConsoleKey.RightArrow)
                {
                    if (Console.CursorLeft < messageLeft + MaxChars)
                        Console.CursorLeft++;
                }
                else if(pressed.Key == ConsoleKey.Home) // Begin
                {
                    Console.CursorLeft = messageLeft + (MaxChars - CurrentCode.Length);
                }
                else if(pressed.Key == ConsoleKey.End)
                {
                    Console.CursorLeft = messageLeft + MaxChars;
                }
                else if(pressed.KeyChar != '\0')
                {
                    if (CurrentCode.Length < MaxChars)
                    {
                        int currIdx = Console.CursorLeft - messageLeft;
                        int realIdx = currIdx - (MaxChars - CurrentCode.Length);

                        CurrentCode = CurrentCode.Insert(realIdx, pressed.KeyChar.ToString());
                        RerenderMessageTextbox(messageLeft, messageTop, ConsoleColor.DarkGreen);
                    }
                }
            }
        }

        private static int EditCharsetState(int charsetLeft, int charsetTop, int messageLeft, int messageTop) // State 2
        {
            Console.SetCursorPosition(charsetLeft, charsetTop);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            int maxChars = 49;

            Console.Write(CharacterSet.PadRight(maxChars, ' '));
            Console.CursorLeft = charsetLeft + CharacterSet.Length;

            while (true)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);

                if (pressed.Key == ConsoleKey.Escape)
                    return -1;

                if (pressed.Key == ConsoleKey.UpArrow || pressed.Key == ConsoleKey.Tab)
                {
                    Console.SetCursorPosition(charsetLeft, charsetTop);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(CharacterSet.PadRight(maxChars, ' '));

                    return (pressed.Key == ConsoleKey.UpArrow) ? 1 : 3;
                }

                if (pressed.Key == ConsoleKey.Backspace || pressed.Key == ConsoleKey.Delete)
                {
                    if (CharacterSet.Length > 2) {
                        if (pressed.Key == ConsoleKey.Delete)
                            Console.CursorLeft++;

                        if (Console.CursorLeft > charsetLeft && CharacterSet.Length > 0)
                        {
                            int currIdx = Console.CursorLeft - charsetLeft;
                            int realIdx = currIdx - 1;
                            if (realIdx >= 0)
                            {
                                CharacterSet = CharacterSet.Remove(realIdx, 1);

                                Console.SetCursorPosition(charsetLeft, charsetTop);
                                Console.Write(CharacterSet.PadRight(maxChars, ' '));
                                CurrentCode = CurrentCode;
                                RerenderMessageTextbox(messageLeft, messageTop, ConsoleColor.Red);
                                Console.CursorLeft = charsetLeft + currIdx - 1;
                            }
                        }
                    }
                }
                else if (pressed.Key == ConsoleKey.LeftArrow)
                {
                    if (Console.CursorLeft > charsetLeft)
                        Console.CursorLeft--;
                }
                else if (pressed.Key == ConsoleKey.RightArrow)
                {
                    if (Console.CursorLeft < charsetLeft + CharacterSet.Length)
                        Console.CursorLeft++;
                }
                else if (pressed.Key == ConsoleKey.Home) // Begin
                {
                    Console.CursorLeft = charsetLeft;
                }
                else if (pressed.Key == ConsoleKey.End)
                {
                    Console.CursorLeft = charsetLeft + CharacterSet.Length;
                }
                else if (pressed.KeyChar != '\0')
                {
                    if (CharacterSet.Length < maxChars)
                    {
                        char rChar = pressed.KeyChar.ToString().ToUpper()[0];

                        int currIdx = Console.CursorLeft - charsetLeft;

                        string newText = CharacterSet.Insert(currIdx, rChar.ToString());

                        CharacterSet = newText;
                        Console.SetCursorPosition(charsetLeft, charsetTop);
                        Console.Write(CharacterSet.PadRight(maxChars, ' '));
                        CurrentCode = CurrentCode;
                        RerenderMessageTextbox(messageLeft, messageTop, ConsoleColor.Red);
                        Console.CursorLeft = charsetLeft + currIdx + 1;
                    }
                }
            }
        }

        private static int MovePawnsState(int menuLeft, int menuTop, int messageLeft, int messageTop) // State 3
        {
            Console.SetCursorPosition(menuLeft, menuTop);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Press P to move pawns, N for the next example, R for a random message, M for the max message, ESC to leave the menu.".PadRight(Console.WindowWidth, ' '));

            bool moving = false;

            while(true)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);

                if (!moving)
                {
                    if (pressed.Key == ConsoleKey.Escape)
                        break;

                    if (pressed.KeyChar.ToString().ToUpper()[0] == 'N')
                    {
                        CurrentCode = Cluedo.EXAMPLES[(Array.IndexOf(Cluedo.EXAMPLES, CurrentCode) + 1) % Cluedo.EXAMPLES.Length];
                        break;
                    }

                    if (pressed.KeyChar.ToString().ToUpper()[0] == 'R')
                    {
                        Random rand = new();

                        BigInteger N = Permutator.GetTotalPermutations(Cluedo.LEXICO_BASE);
                        byte[] bytes = N.ToByteArray();
                        BigInteger R;

                        do
                        {
                            rand.NextBytes(bytes);
                            bytes[bytes.Length - 1] &= (byte)0x7F;
                            R = new BigInteger(bytes);
                        } while (R >= N);

                        CurrentCode = BaseHelper.ToBase(R, CharacterSet);
                        break;
                    }

                    if (pressed.KeyChar.ToString().ToUpper()[0] == 'M')
                    {
                        CurrentCode = BaseHelper.ToBase(Permutator.GetTotalPermutations(Cluedo.LEXICO_BASE) - 1, CharacterSet);
                        break;
                    }

                    if (pressed.KeyChar.ToString().ToUpper()[0] == 'P')
                    {
                        Console.SetCursorPosition(menuLeft, menuTop);
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("ESC to stop moving paws.".PadRight(Console.WindowWidth, ' '));

                        moving = true;
                    }
                }
                else
                {
                    if (pressed.Key == ConsoleKey.Escape)
                    {
                        Console.SetCursorPosition(menuLeft, menuTop);
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Press P to move pawns, N for the next example, R for a random message, M for the max message, ESC to leave the menu.".PadRight(Console.WindowWidth, ' '));

                        moving = false;
                    }
                }
            }

            Console.SetCursorPosition(menuLeft, menuTop);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Press TAB for the menu. Press ESC to close the program.".PadRight(Console.WindowWidth, ' '));
            return 1;
        }

        private static ConsoleColor GetConsoleColorForCoordinates(int x, int y, Dictionary<char, Point> playerPos)
        {
            char potentialPlayer = playerPos.Where(p => p.Value.X == x && p.Value.Y == y).Select(p => p.Key).FirstOrDefault(' ');

            if (potentialPlayer != ' ')
            {
                return potentialPlayer switch
                {
                    '1' => ConsoleColor.Red,
                    '2' => ConsoleColor.Yellow,
                    '3' => ConsoleColor.White,
                    '4' => ConsoleColor.Green,
                    '5' => ConsoleColor.Blue,
                    '6' => ConsoleColor.DarkMagenta,
                    _ => throw new Exception("Unsupported pawn color")
                };
            }
            else if (y < Cluedo.SQUARES_MATRIX.GetLength(0) && Cluedo.SQUARES_MATRIX[y, x] == 0)
                return ConsoleColor.DarkGray;
            else
                return ConsoleColor.Black;
        }

        private static void RenderBoard()
        {
            (int cLeft, int cTop) = Console.GetCursorPosition();
            ConsoleColor srcBColor = Console.BackgroundColor;
            ConsoleColor srcFColor = Console.ForegroundColor;

            Dictionary<char, Point> playerPos = PlayerPositions.ToDictionary(kvp => kvp.Key, kvp => Cluedo.SquareToCoordinates(kvp.Value));

            Console.SetCursorPosition(0, 0);
            Console.ResetColor();

            for (int y = 0; y < Cluedo.SQUARES_MATRIX.GetLength(0); y += 2)
            {
                for (int x = 0; x < Cluedo.SQUARES_MATRIX.GetLength(1); x++)
                {
                    Console.ForegroundColor = GetConsoleColorForCoordinates(x, y, playerPos);
                    Console.BackgroundColor = GetConsoleColorForCoordinates(x, y + 1, playerPos);

                    Console.Write('\u2580'); // ▀
                }
                Console.ResetColor();
                Console.SetCursorPosition(0, Console.CursorTop + 1); // This avoid overwriting things written on the right
            }

            if (cTop != 0 && cLeft != 0)
            {
                Console.ForegroundColor = srcFColor;
                Console.BackgroundColor = srcBColor;
                Console.SetCursorPosition(cLeft, cTop);
            }
        }
    }
}
