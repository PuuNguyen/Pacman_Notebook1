using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class MapViewForm : Form
    {
        private const int TileSize = 30;
        private readonly Map map;
        private readonly MemoryStream pacmanStream;
        private readonly Image pacmanImage;
        private readonly MemoryStream greenGhostStream;
        private readonly Image greenGhostImage;
        private readonly MemoryStream redGhostStream;
        private readonly Image redGhostImage;
        private readonly MemoryStream yellowGhostStream;
        private readonly Image yellowGhostImage;
        private readonly MemoryStream orangeGhostStream;
        private readonly Image orangeGhostImage;
        private readonly MemoryStream coinStream;
        private readonly Image coinImage;

        public MapViewForm(Map map, string title)
        {
            this.map = map;

            pacmanStream = new MemoryStream(global::PacMan.Properties.Resources.pacman);
            pacmanImage = Image.FromStream(pacmanStream);
            greenGhostStream = new MemoryStream(global::PacMan.Properties.Resources.GreenGhost);
            greenGhostImage = Image.FromStream(greenGhostStream);
            redGhostStream = new MemoryStream(global::PacMan.Properties.Resources.RedGhost);
            redGhostImage = Image.FromStream(redGhostStream);
            yellowGhostStream = new MemoryStream(global::PacMan.Properties.Resources.YellowGhost);
            yellowGhostImage = Image.FromStream(yellowGhostStream);
            orangeGhostStream = new MemoryStream(global::PacMan.Properties.Resources.OrangeGhost);
            orangeGhostImage = Image.FromStream(orangeGhostStream);
            coinStream = new MemoryStream(global::PacMan.Properties.Resources.Coin);
            coinImage = Image.FromStream(coinStream);

            Text = title;
            BackColor = Color.Black;
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ClientSize = new Size(map.Columns * TileSize, map.Rows * TileSize);
            StartPosition = FormStartPosition.CenterScreen;
            Paint += (s, e) => DrawMap(e.Graphics);

            ImageAnimator.Animate(pacmanImage, FrameChanged);
            ImageAnimator.Animate(greenGhostImage, FrameChanged);
            ImageAnimator.Animate(yellowGhostImage, FrameChanged);
            ImageAnimator.Animate(orangeGhostImage, FrameChanged);
            ImageAnimator.Animate(redGhostImage, FrameChanged);
            ImageAnimator.Animate(coinImage, FrameChanged);
            FormClosed += (s, e) =>
            {
                ImageAnimator.StopAnimate(pacmanImage, FrameChanged);
                ImageAnimator.StopAnimate(greenGhostImage, FrameChanged);
                ImageAnimator.StopAnimate(yellowGhostImage, FrameChanged);
                ImageAnimator.StopAnimate(orangeGhostImage, FrameChanged);
                ImageAnimator.StopAnimate(redGhostImage, FrameChanged);
                ImageAnimator.StopAnimate(coinImage, FrameChanged);
                pacmanImage.Dispose();
                pacmanStream.Dispose();
                greenGhostImage.Dispose();
                greenGhostStream.Dispose();
                redGhostImage.Dispose();
                redGhostStream.Dispose();
                yellowGhostImage.Dispose();
                yellowGhostStream.Dispose();
                orangeGhostImage.Dispose();
                orangeGhostStream.Dispose();
            };

        }

        private void DrawMap(Graphics g)
        {
            ImageAnimator.UpdateFrames(pacmanImage);
            ImageAnimator.UpdateFrames(greenGhostImage);
            ImageAnimator.UpdateFrames(redGhostImage);
            ImageAnimator.UpdateFrames(yellowGhostImage);
            ImageAnimator.UpdateFrames(orangeGhostImage);
            ImageAnimator.UpdateFrames(coinImage);

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
                            DrawSprite(g, coinImage, rect);
                            break;

                        case 'P':
                            DrawSprite(g, pacmanImage, rect);
                            break;

                        case 'G':
                            DrawSprite(g, greenGhostImage, rect);
                            break;

                        case 'R':
                            DrawSprite(g, redGhostImage, rect);
                            break;

                        case 'O':
                            DrawSprite(g, orangeGhostImage, rect);
                            break;

                        case 'Y':
                            DrawSprite(g, yellowGhostImage, rect);
                            break;

                        default:
                            g.FillRectangle(Brushes.Black, rect);
                            break;
                    }
                }
            }
        }

        private static void DrawSprite(Graphics g, Image image, Rectangle rect)
        {
            g.FillRectangle(Brushes.Black, rect);
            g.DrawImage(image, rect);
        }

        private void FrameChanged(object? sender, EventArgs e)
        {
            if (!IsDisposed && IsHandleCreated)
            {
                BeginInvoke(Invalidate);
            }
        }
    }
}