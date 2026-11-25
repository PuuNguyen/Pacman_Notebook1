using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class MapViewForm : Form
    {
        private const int TileSize = 30;
        private readonly Map map;

        public MapViewForm(Map map, string title)
        {
            this.map = map;
            Text = title;
            BackColor = Color.Black;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ClientSize = new Size(map.Columns * TileSize, map.Rows * TileSize);
            StartPosition = FormStartPosition.CenterScreen;
            Paint += (s, e) => DrawMap(e.Graphics);
        }

        private void DrawMap(Graphics g)
        {
            for (int row = 0; row < map.Rows; row++)
            {
                for (int col = 0; col < map.Columns; col++)
                {
                    var rect = new Rectangle(col * TileSize, row * TileSize, TileSize, TileSize);
                    char tile = map.Grid[row, col];

                    switch (tile)
                    {
                        case '#':
                            g.FillRectangle(Brushes.Blue, rect);
                            break;

                        case '.':
                            g.FillRectangle(Brushes.Black, rect);
                            int dot = 6;
                            g.FillEllipse(Brushes.Yellow,
                                rect.X + (TileSize - dot) / 2,
                                rect.Y + (TileSize - dot) / 2, dot, dot);
                            break;

                        case 'P':
                            g.FillRectangle(Brushes.Black, rect);
                            g.FillEllipse(Brushes.Gold, rect.X + 3, rect.Y + 3, TileSize - 6, TileSize - 6);
                            break;

                        case 'G':
                            g.FillRectangle(Brushes.Black, rect);
                            g.FillEllipse(Brushes.LimeGreen, rect.X + 3, rect.Y + 3, TileSize - 6, TileSize - 6);
                            break;

                        case 'S':
                            g.FillRectangle(Brushes.Black, rect);
                            int ball = TileSize / 2;
                            g.FillEllipse(Brushes.Yellow,
                                rect.X + (TileSize - ball) / 2,
                                rect.Y + (TileSize - ball) / 2, ball, ball);
                            break;

                        default:
                            g.FillRectangle(Brushes.Black, rect);
                            break;
                    }
                }
            }
        }
    }
}