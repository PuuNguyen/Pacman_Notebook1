using System;
using System.Collections.Generic;
using PacMan.Entities;
using WinFormsApp1;

namespace PacMan.Utils
{
    internal class CollisionSystem
    {
        private const float InvincibleDuration = 2f;
        private const float FreezeDuration = 1f;

        private readonly Map map;
        private readonly Player player;
        private readonly IReadOnlyList<Ghost> ghosts;
        private readonly Random random;
        private readonly (int row, int col) start;
        private int dotsRemaining;
        private float invincible;

        public bool IsGameOver { get; private set; }
        public bool IsLevelCleared { get; private set; }

        public CollisionSystem(Map map, Player player, IReadOnlyList<Ghost> ghosts,
                               Random random, (int row, int col) start)
        {
            this.map = map; this.player = player; this.ghosts = ghosts;
            this.random = random; this.start = start;
            foreach (char c in map.Grid)
                if (c == '.') dotsRemaining++;
        }

        public void Update(float dt)
        {
            if (invincible > 0f) invincible -= dt;
            EatItem();
            foreach (Ghost g in ghosts)
                if (Hit(g)) OnHitGhost(g);
        }

        private void EatItem()
        {
            char t = map.Grid[player.Row, player.Col];
            if (t == '.')
            {
                map.Grid[player.Row, player.Col] = ' ';
                player.Score += 10;
                if (--dotsRemaining <= 0) IsLevelCleared = true;
            }
            else if (t == 'S')
            {
                map.Grid[player.Row, player.Col] = ' ';
                player.Score += 50;

            }
        }

        private bool Hit(Ghost g)
        {
            float dx = player.PixelX - g.PixelX, dy = player.PixelY - g.PixelY;
            float r = player.TileSize * 0.6f;
            return dx * dx + dy * dy < r * r;
        }

        private void OnHitGhost(Ghost g)
        {
            if (player.IsPowerMode)
            {
                player.Heal();
                player.Score += 200;
                Respawn(g);
            }
            else if (invincible <= 0f)
            {
                if (--player.Lives <= 0) { IsGameOver = true; return; }
                player.SpawnAt(start.row, start.col);
                player.Freeze(FreezeDuration);
                invincible = InvincibleDuration;
            }
        }

        private void Respawn(Ghost g)
        {
            for (int i = 0; i < 100; i++)
            {
                int r = random.Next(map.Rows), c = random.Next(map.Columns);
                if (map.Grid[r, c] == '#') continue;
                if (Math.Abs(r - player.Row) + Math.Abs(c - player.Col) < 6) continue;
                g.SpawnAt(r, c);
                return;
            }
        }
    }
}
