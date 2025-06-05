using SO_CC2.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace SO_CC2
{
    public partial class Main : Form
    {
        private string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ !\"'()*+,-.0123456789:?";

        private static readonly string[] EXAMPLES = { "TREASURE", "LOOKLEFT", "SECRETED", "DIGHERE!", "TOMORROW" };
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

        private const string PAWNS = "123456";
        private static readonly int SQUARES = SQUARES_MATRIX.Cast<int>().Count(sq => sq == 1);
        private static readonly string LEXICO_BASE = new string(' ', SQUARES - PAWNS.Length) + PAWNS;

        private Dictionary<char, int> Players = new();

        private char AttachedCharacter = ' ';
        private Point MousePosition = new Point(-1, -1);

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Main_Resize(sender, e);

            codeTextBox.Text = EXAMPLES[(new Random()).Next(EXAMPLES.Length)];
            alphabetTextBox.Text = CharacterSet;
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            boardTableLayoutPanel.Width = Math.Min(boardTableLayoutPanel.Height, this.Width - rightGroupBox.Width - 48);

            double width;
            double height;

            if (((double)boardTableLayoutPanel.BackgroundImage.Width / (double)boardTableLayoutPanel.BackgroundImage.Height) < ((double)boardTableLayoutPanel.Width / (double)boardTableLayoutPanel.Height))
            {
                height = boardTableLayoutPanel.Height;
                width = height * (double)boardTableLayoutPanel.BackgroundImage.Width / (double)boardTableLayoutPanel.BackgroundImage.Height;
            }
            else
            {
                width = boardTableLayoutPanel.Width;
                height = width * (double)boardTableLayoutPanel.BackgroundImage.Height / (double)boardTableLayoutPanel.BackgroundImage.Width;
            }

            boardTableLayoutPanel.SuspendLayout();
            ColumnStyle firstCol = boardTableLayoutPanel.ColumnStyles[0];
            ColumnStyle lastCol = boardTableLayoutPanel.ColumnStyles[boardTableLayoutPanel.ColumnStyles.Count - 1];
            RowStyle firstRow = boardTableLayoutPanel.RowStyles[0];
            RowStyle lastRow = boardTableLayoutPanel.RowStyles[boardTableLayoutPanel.RowStyles.Count - 1];

            firstCol.SizeType = lastCol.SizeType = firstRow.SizeType = lastRow.SizeType = SizeType.Percent;

            double baseBorderCol = ((boardTableLayoutPanel.Width - width) / 2);
            firstCol.Width = (float)((baseBorderCol + (45 * width / boardTableLayoutPanel.BackgroundImage.Width)) / boardTableLayoutPanel.Width * 100);
            lastCol.Width = (float)((baseBorderCol + (71 * width / boardTableLayoutPanel.BackgroundImage.Width)) / boardTableLayoutPanel.Width * 100);

            double baseBorderRow = ((boardTableLayoutPanel.Height - height) / 2);
            firstRow.Height = (float)((baseBorderRow + (46 * height / boardTableLayoutPanel.BackgroundImage.Height)) / boardTableLayoutPanel.Height * 100);
            lastRow.Height = (float)((baseBorderRow + (40 * height / boardTableLayoutPanel.BackgroundImage.Height)) / boardTableLayoutPanel.Height * 100);

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
            boardTableLayoutPanel.ResumeLayout();

            playersPictureBox.Refresh();
        }

        private void boardTableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            #if DEBUG
            if ((e.Column + e.Row) % 2 == 1)
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 0, 0)), e.CellBounds);
            else
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 0, 255)), e.CellBounds);
            #endif
        }
        private void playersPictureBox_Paint(object sender, PaintEventArgs e)
        {
            foreach ((char player, int square) in Players)
            {
                float squareWidth = (float)e.ClipRectangle.Width / (float)SQUARES_MATRIX.GetLength(1);
                float squareHeight = (float)e.ClipRectangle.Height / (float)SQUARES_MATRIX.GetLength(0);

                PointF sqCoords;
                if (AttachedCharacter == player)
                {
                    sqCoords = new PointF(MousePosition.X - (squareWidth / 2), MousePosition.Y - (squareHeight / 2));
                }
                else
                {
                    sqCoords = SquareToCoordinates(square);
                    sqCoords = new PointF(squareWidth * sqCoords.X, squareHeight * sqCoords.Y);
                }

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

        private void codeTextBox_TextChanged(object sender, EventArgs e)
        {
            // Cleaning up the text: all letters become uppercase, and characters not found in CharacterSet disappear
            int sel = codeTextBox.SelectionStart;
            int ogLen = codeTextBox.TextLength;
            string newText = new string(codeTextBox.Text.ToUpper().ToCharArray().Where(c => CharacterSet.Contains(c)).ToArray());
            if (newText != codeTextBox.Text)
            {
                codeTextBox.Text = newText; // This will call this function again, so to avoid rendering twice, we return once we're done cleaning up the text
                codeTextBox.SelectionStart = Math.Max(Math.Min(sel - (ogLen - codeTextBox.TextLength), codeTextBox.TextLength), 0);
                return;
            }

            // Compute the new permutation the inputed string
            string newPermutation = Permutator.IndexToPermutation(LEXICO_BASE, BaseHelper.FromBase(codeTextBox.Text.ToUpper(), CharacterSet));

            // Recalculate players positions according to the computed permutation
            Players = PAWNS.ToDictionary(p => p, p => newPermutation.IndexOf(p));

            // Render the players
            playersPictureBox.Refresh();
        }

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

        private int CoordinatesToSquare(Point coords)
        {
            if (SQUARES_MATRIX[coords.Y, coords.X] == 0)
                return -1;

            return SQUARES_MATRIX.Cast<int>().Take(coords.Y * SQUARES_MATRIX.GetLength(1) + coords.X).Count(sq => sq == 1);
        }

        private void playersPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (int)Math.Floor((float)e.X / ((float)playersPictureBox.Width / (float)SQUARES_MATRIX.GetLength(1)));
            int y = (int)Math.Floor((float)e.Y / ((float)playersPictureBox.Height / (float)SQUARES_MATRIX.GetLength(0)));
            AttachedCharacter = Players.Where(player => (new Point(x, y)) == SquareToCoordinates(player.Value)).Select(kvp => kvp.Key).FirstOrDefault(' ');

            MousePosition = new Point(e.X, e.Y);
            playersPictureBox.Refresh();
        }

        private void playersPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (AttachedCharacter == ' ')
                return;

            int x = (int)Math.Floor((float)e.X / ((float)playersPictureBox.Width / (float)SQUARES_MATRIX.GetLength(1)));
            int y = (int)Math.Floor((float)e.Y / ((float)playersPictureBox.Height / (float)SQUARES_MATRIX.GetLength(0)));
            int square = CoordinatesToSquare(new Point(x, y));
            if (square >= 0)
                Players[AttachedCharacter] = square;

            AttachedCharacter = ' ';

            if (square < 0)
            {
                playersPictureBox.Refresh();
            }
            else
            {
                StringBuilder newPerm = new(new string(' ', SQUARES));
                foreach ((char player, int value) in Players)
                    newPerm[value] = player;

                codeTextBox.Text = BaseHelper.ToBase(Permutator.PermutationToIndex(newPerm.ToString()), CharacterSet); // This will call playersPictureBox.Refresh();
            }
        }

        private void playersPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (AttachedCharacter == ' ')
                return;

            MousePosition = new Point(e.X, e.Y);
            playersPictureBox.Refresh();
        }

        private void nextExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeTextBox.Text = EXAMPLES[(Array.IndexOf(EXAMPLES, codeTextBox.Text) + 1) % EXAMPLES.Length];
        }

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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog();
            }
        }

        private void alphabetTextBox_TextChanged(object sender, EventArgs e)
        {
            if (alphabetTextBox.Text.Length < 2)
            {
                alphabetTextBox.Text = CharacterSet.Substring(0, 2);
                alphabetTextBox.SelectionStart = 2;
            }

            alphabetTextBox.Text = alphabetTextBox.Text.ToUpper();

            CharacterSet = alphabetTextBox.Text;

            codeTextBox.MaxLength = (int)Math.Floor(BigInteger.Log(MathHelper.Factorial(SQUARES) / MathHelper.Factorial(SQUARES - PAWNS.Length)) / BigInteger.Log(CharacterSet.Length));
            codeTextBox_TextChanged(sender, e);
        }
    }
}
