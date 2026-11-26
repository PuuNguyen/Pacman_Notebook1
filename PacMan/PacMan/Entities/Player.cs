using System;
using System.Drawing;

namespace PacMan.Entities
{
    public class Player : Entities
    {
        public int Lives { get; set; } = 3;
        public int Score { get; set; } = 0;
        public bool IsPowerMode { get; set; } = false;
        public float PowerModeTimer { get; set; } = 0f;

        private readonly Image? sprite;
        private float _mouthAnimTime;

        public Player(int tileSize, float speed = 150f, Image? sprite = null) : base(tileSize, speed)
        {
            this.sprite = sprite;
        }

        public void SetDesiredDirection(int dx, int dy)
        {
            if (dx != 0 || dy != 0)
            {
                DesiredDirX = dx;
                DesiredDirY = dy;
            }
        }

        public override void Update(float dt, char[,] grid)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            if (rows == 0 || cols == 0) return;

            if (IsPowerMode)
            {
                PowerModeTimer -= dt;
                if (PowerModeTimer <= 0f)
                {
                    PowerModeTimer = 0f;
                    IsPowerMode = false;
                }
            }

            float tileX = Col * TileSize;
            float tileY = Row * TileSize;
            float snapThreshold = Math.Max(2f, Speed * dt * 1.2f);

            bool nearTile = Math.Abs(PixelX - tileX) <= snapThreshold &&
                            Math.Abs(PixelY - tileY) <= snapThreshold;

            if (nearTile)
            {
                PixelX = tileX;
                PixelY = tileY;

                if ((DesiredDirX != 0 || DesiredDirY != 0) &&
                    CanMoveTo(Row + DesiredDirY, Col + DesiredDirX, grid))
                {
                    DirX = DesiredDirX;
                    DirY = DesiredDirY;
                }
            }

            MoveInCurrentDirection(dt, grid);

            if (DirX != 0 || DirY != 0)
                _mouthAnimTime += dt * 12f;
        }

        public override void Render(Graphics g)
        {
            var rectangle = new RectangleF(PixelX, PixelY, TileSize, TileSize);

            g.FillRectangle(Brushes.Black, rectangle);

            if (sprite != null)
            {
                g.DrawImage(sprite, rectangle);
            }
            else
            {
                g.FillEllipse(Brushes.Yellow, rectangle);
            }
        }

    }
}
