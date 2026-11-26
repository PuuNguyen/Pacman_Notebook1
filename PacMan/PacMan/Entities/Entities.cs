using System;
using System.Collections.Generic;
using System.Text;

namespace PacMan.Entities
{
    public abstract class Entities
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public float PixelX { get; set; }
        public float PixelY { get; set; }
        public float Speed { get; set; }
        public int TileSize { get; protected set; }

        public int DirX { get; set; }
        public int DirY { get; set; }
        public int DesiredDirX { get; set; }
        public int DesiredDirY { get; set; }

        protected Entities(int tileSize, float speed)
        {
            TileSize = tileSize;
            Speed = speed;
        }

        public virtual void SpawnAt(int row, int col)
        {
            Row = row;
            Col = col;
            PixelX = col * TileSize;
            PixelY = row * TileSize;
            DirX = 0;
            DirY = 0;
            DesiredDirX = 0;
            DesiredDirY = 0;
        }

        public abstract void Update(float dt, char[,] grid);
        public abstract void Render(Graphics g);

        protected bool CanMoveTo(int row, int column, char[,] grid)
        {
            return row >= 0 &&
                   row < grid.GetLength(0) &&
                   column >= 0 &&
                   column < grid.GetLength(1) &&
                   grid[row, column] != '#';
        }

        protected bool IsAtTileCenter(float threshold)
        {
            float tileX = Col * TileSize;
            float tileY = Row * TileSize;

            return Math.Abs(PixelX - tileX) <= threshold &&
                   Math.Abs(PixelY - tileY) <= threshold;
        }

        protected void SnapToTileCenter()
        {
            PixelX = Col * TileSize;
            PixelY = Row * TileSize;
        }

        protected bool MoveInCurrentDirection(float dt, char[,] grid)
        {
            if (DirX == 0 && DirY == 0)
                return false;

            int targetColumn = Col + DirX;
            int targetRow = Row + DirY;

            if (!CanMoveTo(targetRow, targetColumn, grid))
            {
                SnapToTileCenter();
                DirX = 0;
                DirY = 0;
                return false;
            }

            float currentX = Col * TileSize;
            float currentY = Row * TileSize;
            PixelX += DirX * Speed * dt;
            PixelY += DirY * Speed * dt;

            if (Math.Abs(PixelX - currentX) >= TileSize ||
                Math.Abs(PixelY - currentY) >= TileSize)
            {
                Col = targetColumn;
                Row = targetRow;
                SnapToTileCenter();
            }

            return true;
        }
    }
}
