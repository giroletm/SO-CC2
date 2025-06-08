using SO_CC2.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace SO_CC2.GUI
{
    public partial class Main : Form
    {
        #region Constants

        /// <summary>
        /// The examples provided in the challenge's instructions
        /// </summary>
        private static readonly string[] EXAMPLES = { "TREASURE", "LOOKLEFT", "SECRETED", "DIGHERE!", "TOMORROW" };

        /// <summary>
        /// The squares of a classic Cluedo board. <c>1</c>s represent walkable squares, <c>0</c>s represent non-walkable ones.
        /// </summary>
        private static readonly int[,] SQUARES_MATRIX =
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
        private const string PAWNS = "123456";

        /// <summary>
        /// The number of walkable squares on the Cluedo board.
        /// Calculated at runtime from <see cref="SQUARES_MATRIX"/>.
        /// </summary>
        private static readonly int SQUARES_COUNT = SQUARES_MATRIX.Cast<int>().Count(sq => sq == 1);

        /// <summary>
        /// Base permutation to represent pawns on the Cluedo board
        /// </summary>
        private static readonly string LEXICO_BASE = new string(' ', SQUARES_COUNT - PAWNS.Length) + PAWNS;

        #endregion

        #region User-editable resources

        /// <summary>
        /// The character set to be used as a base to encode permutation IDs
        /// </summary>
        private string CharacterSet { get; set; } = " ABCDEFGHIJKLMNOPQRSTUVWXYZ!\"'()*+,-.0123456789:?";

        /// <summary>
        /// Dictionary holding each playing pawn's position by indexing its internal character with its square number
        /// </summary>
        private Dictionary<char, int> PlayerPositions { get; set; } = new();

        /// <summary>
        /// Internal character of the pawn currently attached to the mouse during drag-and-drops.
        /// Set to <c>' '</c> when there is none.
        /// </summary>
        private char AttachedCharacter { get; set; } = ' ';

        /// <summary>
        /// Current position of the user's mouse over <see cref="playersPictureBox"/> during drag-and-drops.
        /// </summary>
        private Point CursorPosition { get; set; } = new Point(-1, -1);

        private string CurrentCode { get; set; } = "";

        #endregion

        public Main()
        {
            InitializeComponent();
        }

        #region Board utility functions

        /// <summary>
        /// Computes the coordinates on the board of the <paramref name="squareId"/>-th square.
        /// <br/>
        /// Acts as the reverse of <see cref="CoordinatesToSquare"/>.
        /// </summary>
        /// <param name="squareId">The square number to compute the coordinates of</param>
        /// <returns>The coordinates on the board of the <paramref name="squareId"/>-th square</returns>
        private Point SquareToCoordinates(int squareId)
        {
            int height = SQUARES_MATRIX.GetLength(0);
            int width = SQUARES_MATRIX.GetLength(1);

            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (SQUARES_MATRIX[y, x] > 0)
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
        private int CoordinatesToSquare(Point coords)
        {
            if (SQUARES_MATRIX[coords.Y, coords.X] == 0)
                return -1;

            return SQUARES_MATRIX.Cast<int>().Take(coords.Y * SQUARES_MATRIX.GetLength(1) + coords.X).Count(sq => sq == 1);
        }

        #endregion

        #region Callbacks

        #region Form callbacks

        /// <summary>
        /// Called when the Form loads
        /// </summary>
        private void Main_Load(object sender, EventArgs e)
        {
            Main_Resize(sender, e);

            codeTextBox.Text = EXAMPLES[(new Random()).Next(EXAMPLES.Length)];
            characterSetTextBox.Text = CharacterSet;
        }

        /// <summary>
        /// Called when the form resizes
        /// </summary>
        private void Main_Resize(object sender, EventArgs e)
        {
            // The board's width must be the minimum between the board's height and the form's width minus rightGroupBox's width, margins included
            boardTableLayoutPanel.Width = Math.Min(boardTableLayoutPanel.Height, this.Width - rightGroupBox.Width - 48);

            // Figure out the read width and height of the board as it's displayed
            double width;
            double height;

            if (((double)boardTableLayoutPanel.BackgroundImage!.Width / (double)boardTableLayoutPanel.BackgroundImage.Height) < ((double)boardTableLayoutPanel.Width / (double)boardTableLayoutPanel.Height))
            {
                height = boardTableLayoutPanel.Height;
                width = height * (double)boardTableLayoutPanel.BackgroundImage.Width / (double)boardTableLayoutPanel.BackgroundImage.Height;
            }
            else
            {
                width = boardTableLayoutPanel.Width;
                height = width * (double)boardTableLayoutPanel.BackgroundImage.Height / (double)boardTableLayoutPanel.BackgroundImage.Width;
            }

            // Stop refreshing layouts and find the bordering rows and columns
            boardTableLayoutPanel.SuspendLayout();
            ColumnStyle firstCol = boardTableLayoutPanel.ColumnStyles[0];
            ColumnStyle lastCol = boardTableLayoutPanel.ColumnStyles[boardTableLayoutPanel.ColumnStyles.Count - 1];
            RowStyle firstRow = boardTableLayoutPanel.RowStyles[0];
            RowStyle lastRow = boardTableLayoutPanel.RowStyles[boardTableLayoutPanel.RowStyles.Count - 1];

            // Size are going to be in percents
            firstCol.SizeType = lastCol.SizeType = firstRow.SizeType = lastRow.SizeType = SizeType.Percent;

            // Set the first and last columns and rows to be the space we're padding to reach the usable squares of the board

            double baseBorderCol = ((boardTableLayoutPanel.Width - width) / 2);
            firstCol.Width = (float)((baseBorderCol + (43 * width / boardTableLayoutPanel.BackgroundImage.Width)) / boardTableLayoutPanel.Width * 100);
            lastCol.Width = (float)((baseBorderCol + (70 * width / boardTableLayoutPanel.BackgroundImage.Width)) / boardTableLayoutPanel.Width * 100);

            double baseBorderRow = ((boardTableLayoutPanel.Height - height) / 2);
            firstRow.Height = (float)((baseBorderRow + (44 * height / boardTableLayoutPanel.BackgroundImage.Height)) / boardTableLayoutPanel.Height * 100);
            lastRow.Height = (float)((baseBorderRow + (38 * height / boardTableLayoutPanel.BackgroundImage.Height)) / boardTableLayoutPanel.Height * 100);

            // Spread the remaining space across the remaining columns and rows

            float totalPrcC = firstCol.Width + lastCol.Width;
            float totalPrcR = firstRow.Height + lastRow.Height;

            float diffCol = (float)((100 - (2 * firstCol.Width)) / (boardTableLayoutPanel.ColumnCount - 2));
            for (int col = 1; col < boardTableLayoutPanel.ColumnCount - 1; col++)
            {
                boardTableLayoutPanel.ColumnStyles[col].SizeType = SizeType.Percent;
                boardTableLayoutPanel.ColumnStyles[col].Width = diffCol;
                totalPrcC += diffCol;
            }

            float diffRow = (float)((100 - (2 * firstRow.Height)) / (boardTableLayoutPanel.RowCount - 2));
            for (int row = 1; row < boardTableLayoutPanel.RowCount - 1; row++)
            {
                boardTableLayoutPanel.RowStyles[row].SizeType = SizeType.Percent;
                boardTableLayoutPanel.RowStyles[row].Height = diffRow;
                totalPrcR += diffRow;
            }

            // Resume refreshing layouts
            boardTableLayoutPanel.ResumeLayout();

            // Renders the players on top of the board
            playersPictureBox.Refresh();
        }

        #endregion

        #region Rendering callbacks

        /// <summary>
        /// Executed on every repaint of <see cref="boardTableLayoutPanel"/>.
        /// Useful to render solid colors over cells to see their exact size.
        /// </summary>
        private void boardTableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            /*
            #if DEBUG
            e.Graphics.FillRectangle(new SolidBrush(((x + y) % 2 == 1) ? Color.FromArgb(64, 255, 0, 0) : Color.FromArgb(64, 0, 0, 255)), e.CellBounds);
            #endif
            */
        }

        /// <summary>
        /// Executed on every repaint of <see cref="playersPictureBox"/>.
        /// Effectively renders players to <paramref name="sender"/>'s <see cref="PaintEventArgs"/> <paramref name="e"/>.
        /// </summary>
        private void playersPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // The size of a square is the size of the picture box divided by the total amount of square in the same dimension

            float squareWidth = (float)e.ClipRectangle.Width / (float)SQUARES_MATRIX.GetLength(1);
            float squareHeight = (float)e.ClipRectangle.Height / (float)SQUARES_MATRIX.GetLength(0);

            // Render squares with solid colors for debugging purposes

            /*
            #if DEBUG
            for (int y = 0; y < SQUARES_MATRIX.GetLength(1); y++)
            {
                for (int x = 0; x < SQUARES_MATRIX.GetLength(0); x++)
                {
                    e.Graphics.FillRectangle(new SolidBrush(((x + y) % 2 == 1) ? Color.FromArgb(64, 255, 0, 0) : Color.FromArgb(64, 0, 0, 255)), new RectangleF(x * squareWidth, y * squareHeight, squareWidth, squareHeight));
                }
            }
            #endif
            */

            // Renders each playing pawn

            foreach ((char player, int square) in PlayerPositions.OrderBy(player => (player.Key == AttachedCharacter) ? 1 : 0)) // If we're currently drag-and-drop'ing a pawn, draw it last so it's on top of others
            {
                // If we're rendering the attached pawn, draw it around the mouse's position. Otherwise, get the coordinates to its designated square and draw it there

                PointF sqCoords;
                if (AttachedCharacter == player)
                {
                    sqCoords = new PointF(CursorPosition.X - (squareWidth / 2), CursorPosition.Y - (squareHeight / 2));
                }
                else
                {
                    sqCoords = SquareToCoordinates(square);
                    sqCoords = new PointF(squareWidth * sqCoords.X, squareHeight * sqCoords.Y);
                }

                // Rendering to the picture box. It performs much better to render directly to it than to a Bitmap that would then be linked to the picture box.

                e.Graphics.FillEllipse(
                    new SolidBrush(player switch
                    {
                        '1' => Color.Red,
                        '2' => Color.Yellow,
                        '3' => Color.White,
                        '4' => Color.Green,
                        '5' => Color.Blue,
                        '6' => Color.Purple,
                    }),
                    new RectangleF(sqCoords.X, sqCoords.Y, squareWidth, squareHeight)
                );
            }
        }

        #endregion

        #region UI inputs callbacks

        /// <summary>
        /// Called each time the text in <see cref="codeTextBox"/> is changed.
        /// This is effectively called every time there's a new message to encode into a permutation
        /// </summary>
        private void codeTextBox_TextChanged(object sender, EventArgs e)
        {
            // Cleaning up the text: all letters become uppercase, and characters not found in CharacterSet disappear

            int sel = codeTextBox.SelectionStart;
            int ogLen = codeTextBox.TextLength;
            string newText = new string(codeTextBox.Text.ToUpper().ToCharArray().Where(c => CharacterSet.Contains(c)).ToArray());

            // If the inputed code is greater than the max, restore the previous input

            BigInteger textAsNum = BaseHelper.FromBase(newText, CharacterSet);
            if (textAsNum >= Permutator.GetTotalPermutations(LEXICO_BASE))
                newText = CurrentCode;

            // Set the code to use to the computed one if needed, and try not to break the selection cursor

            if (newText != codeTextBox.Text)
            {
                codeTextBox.Text = newText; // This will call this function again, so to avoid rendering twice, we return once we're done cleaning up the text
                codeTextBox.SelectionStart = Math.Max(Math.Min(sel - (ogLen - codeTextBox.TextLength), codeTextBox.TextLength), 0);
                return;
            }

            CurrentCode = codeTextBox.Text;

            // Compute the new permutation the inputed string
            string newPermutation = Permutator.IndexToPermutation(LEXICO_BASE, BaseHelper.FromBase(codeTextBox.Text.ToUpper(), CharacterSet));

            // Recalculate players positions according to the computed permutation
            PlayerPositions = PAWNS.ToDictionary(p => p, p => newPermutation.IndexOf(p));

            // Render the players
            playersPictureBox.Refresh();
        }

        /// <summary>
        /// Called when <see cref="characterSetTextBox"/>'s text is changed.
        /// Effectively keeps the character set > 2 characters and uppercase, while updating the maximum possible length of <see cref="codeTextBox"/> and cleaning it up from any forbidden character.
        /// </summary>
        private void characterSetTextBox_TextChanged(object sender, EventArgs e)
        {
            // Base 2 is the minimum base, we can't go below it.

            string charSet = characterSetTextBox.Text;

            if (charSet.Length < 2)
            {
                charSet = CharacterSet.Substring(0, 2);
                characterSetTextBox.SelectionStart = 2;
            }

            // For simplicity's sake, let's make the character set case insensitive
            charSet = charSet.ToUpper();

            // Update the character set
            if (characterSetTextBox.Text != charSet || CharacterSet != charSet || codeTextBox.MaxLength == 0)
            {
                CharacterSet = characterSetTextBox.Text = charSet;

                // The maximum encodable length can be calculated using this formula, minus one: https://stackoverflow.com/a/29847712/9399492
                codeTextBox.MaxLength = 1 + (int)Math.Floor(BigInteger.Log(MathHelper.Factorial(SQUARES_COUNT) / MathHelper.Factorial(SQUARES_COUNT - PAWNS.Length)) / BigInteger.Log(CharacterSet.Length));

                // Clean up the currently encoded message and rerender accordingly
                codeTextBox_TextChanged(sender, e);
            }
        }

        #endregion

        #region Drag-and-drop callbacks

        /// <summary>
        /// Called when the user presses their mouse while hovering over <see cref="playersPictureBox"/>.
        /// Effectively attaches the hovered player to the mouse if there is one.
        /// </summary>
        private void playersPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (int)Math.Floor((float)e.X / ((float)playersPictureBox.Width / (float)SQUARES_MATRIX.GetLength(1)));
            int y = (int)Math.Floor((float)e.Y / ((float)playersPictureBox.Height / (float)SQUARES_MATRIX.GetLength(0)));
            AttachedCharacter = PlayerPositions.Where(player => (new Point(x, y)) == SquareToCoordinates(player.Value)).Select(kvp => kvp.Key).FirstOrDefault(' ');

            CursorPosition = new Point(e.X, e.Y);
            playersPictureBox.Refresh();
        }

        /// <summary>
        /// Called when the user releases their mouse after having pressed it over <see cref="playersPictureBox"/> beforehand.
        /// Effectively detaches the hovered player from the mouse if there is one, and gives it its new position if allowed.
        /// </summary>
        private void playersPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (AttachedCharacter == ' ')
                return;

            int x = (int)Math.Floor((float)e.X / ((float)playersPictureBox.Width / (float)SQUARES_MATRIX.GetLength(1)));
            int y = (int)Math.Floor((float)e.Y / ((float)playersPictureBox.Height / (float)SQUARES_MATRIX.GetLength(0)));
            int square = CoordinatesToSquare(new Point(x, y));

            // A square is valid if it's free and walkable
            bool validSquare = (square >= 0) && (!PlayerPositions.ContainsValue(square));
            if (validSquare)
                PlayerPositions[AttachedCharacter] = square;

            AttachedCharacter = ' ';

            if (validSquare)
            {
                StringBuilder newPerm = new(new string(' ', SQUARES_COUNT));
                foreach ((char player, int value) in PlayerPositions)
                    newPerm[value] = player;

                string newCode = BaseHelper.ToBase(Permutator.PermutationToIndex(newPerm.ToString()), CharacterSet, codeTextBox.MaxLength);
                if (newCode != codeTextBox.Text)
                    codeTextBox.Text = newCode; // This will call playersPictureBox.Refresh();
                else
                    playersPictureBox.Refresh();
            }
            else
                playersPictureBox.Refresh();
        }

        /// <summary>
        /// Called when the user moves their mouse after having pressed it over <see cref="playersPictureBox"/> beforehand.
        /// Effectively moves the attached player with the mouse, if there is one.
        /// </summary>
        private void playersPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (AttachedCharacter == ' ')
                return;

            CursorPosition = new Point(e.X, e.Y);
            playersPictureBox.Refresh();
        }

        #endregion

        #region Menu callbacks

        /// <summary>
        /// Sets <see cref="codeTextBox.Text"/> to the next element of the <see cref="EXAMPLES"/> array, or the first if the current value isn't one of the examples.
        /// </summary>
        private void nextExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeTextBox.Text = EXAMPLES[(Array.IndexOf(EXAMPLES, codeTextBox.Text) + 1) % EXAMPLES.Length];
        }

        /// <summary>
        /// Sets <see cref="codeTextBox.Text"/> to a random encodable value.
        /// </summary>
        private void randomizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new();
            Random rand = new();

            for (var i = 0; i < codeTextBox.MaxLength; i++)
            {
                var c = CharacterSet[rand.Next(0, CharacterSet.Length)];
                builder.Append(c);
            }

            codeTextBox.Text = builder.ToString();
        }

        private void maxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeTextBox.Text = BaseHelper.ToBase(Permutator.GetTotalPermutations(LEXICO_BASE) - 1, CharacterSet);
        }

        /// <summary>
        /// Displays an <see cref="AboutForm"/> in a blocking way.
        /// </summary>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog();
            }
        }

        #endregion

        #endregion
    }
}
