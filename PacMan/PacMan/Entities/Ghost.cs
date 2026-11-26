using System;
using System.Drawing;
using PacMan.Strategies;

namespace PacMan.Entities
{
    public class Ghost : Entities
    {
        private readonly Image sprite;
        private readonly IGhostMovementStrategy movementStrategy;
        private readonly Random random;
        private Player? player;

        public Ghost(
            int tileSize,
            Image sprite,
            IGhostMovementStrategy movementStrategy,
            Random random,
            float speed = 100f)
            : base(tileSize, speed)
        {
            this.sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
            this.movementStrategy = movementStrategy ?? throw new ArgumentNullException(nameof(movementStrategy));
            this.random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public void SetPlayerTarget(Player player)
        {
            this.player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public override void Update(float dt, char[,] grid)
        {
            if (player == null)
                throw new InvalidOperationException("Ghost chưa được gán Player target.");

            float snapThreshold = Math.Max(2f, Speed * dt * 1.2f);
            if (IsAtTileCenter(snapThreshold))
            {
                SnapToTileCenter();
                (DirX, DirY) = movementStrategy.ChooseDirection(
                    this,
                    player,
                    grid,
                    random);
            }

            MoveInCurrentDirection(dt, grid);
        }

        public override void Render(Graphics g)
        {
            var rectangle = new RectangleF(PixelX, PixelY, TileSize, TileSize);
            g.FillRectangle(Brushes.Black, rectangle);
            g.DrawImage(sprite, rectangle);
        }
    }
}
