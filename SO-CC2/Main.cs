using SO_CC2.Utils;
using System.Diagnostics;
using System.Drawing;

namespace SO_CC2
{
    public partial class Main : Form
    {
        private static readonly string BASE49 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ !\"'()*+,-.0123456789:?";

        private const string PAWNS = "123456";
        private const int SQUARES = 182;
        private static readonly string LEXICO_BASE = new string(' ', SQUARES - PAWNS.Length) + PAWNS;

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

        private Dictionary<char, int> Players = new();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Main_Resize(sender, e);
            codeTextBox.Text = EXAMPLES[(new Random()).Next(EXAMPLES.Length)];
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


            Debug.WriteLine(totalPrcC + ";" + totalPrcR);
            Debug.WriteLine(firstCol.Width + ":" + firstRow.Height);
            Debug.WriteLine(diffCol + "_" + diffRow);

            RedrawPlayers();
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

        private void codeTextBox_TextChanged(object sender, EventArgs e)
        {
            string newPermutation = Permutator.IndexToPermutation(LEXICO_BASE, BaseHelper.FromBase(codeTextBox.Text.ToUpper(), BASE49));

            Players = PAWNS.ToDictionary(p => p, p => newPermutation.IndexOf(p));

            RedrawPlayers();
        }

        private void RedrawPlayers()
        {
            Bitmap bmp = new Bitmap(playersPictureBox.Width, playersPictureBox.Height);
            
            using(Graphics g = Graphics.FromImage(bmp))
            {
                foreach((char player, int square) in Players)
                {
                    Point sqCoords = SquareToCoordinates(square);
                    float squareWidth = (float)bmp.Width / (float)SQUARES_MATRIX.GetLength(1);
                    float squareHeight = (float)bmp.Height / (float)SQUARES_MATRIX.GetLength(0);
                    g.FillEllipse(
                        new SolidBrush(player switch
                        {
                            '1' => Color.Red,
                            '2' => Color.Yellow,
                            '3' => Color.White,
                            '4' => Color.Green,
                            '5' => Color.Blue,
                            '6' => Color.Purple,
                        }),
                        new RectangleF(squareWidth * sqCoords.X, squareHeight * sqCoords.Y, squareWidth, squareHeight)
                    );
                }
            }

            Image? oldPicture = playersPictureBox.BackgroundImage;
            playersPictureBox.BackgroundImage = bmp;
            if(oldPicture != null)
                oldPicture.Dispose();
        }

        private Point SquareToCoordinates(int squareId)
        {
            int height = SQUARES_MATRIX.GetLength(0);
            int width = SQUARES_MATRIX.GetLength(1);

            int i = 0;
            for(int y =  0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
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

    }
}
