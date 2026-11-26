using System;
using System.Collections.Generic;
using System.Drawing;
using PacMan.Core;
using PacMan.Entities;
using PacMan.Strategies;
using WinFormsApp1;

namespace PacMan.Scenes
{
    public class PlayScene : IScene
    {
        private readonly Map map;
        private readonly InputManager input;
        private readonly Player player;
        private readonly IReadOnlyDictionary<char, Image> ghostSprites;
        private readonly Func<char, IGhostMovementStrategy> strategyFactory;
        private readonly Random random = new();
        private readonly List<Ghost> ghosts = new();

        public Player Player => player;
        public IReadOnlyList<Ghost> Ghosts => ghosts;

        public PlayScene(
            Map map,
            InputManager input,
            Image playerImage,
            IReadOnlyDictionary<char, Image> ghostSprites,
            Func<char, IGhostMovementStrategy>? strategyFactory = null,
            int tileSize = 30)
        {
            this.map = map ?? throw new ArgumentNullException(nameof(map));
            this.input = input ?? throw new ArgumentNullException(nameof(input));
            this.ghostSprites = ghostSprites ?? throw new ArgumentNullException(nameof(ghostSprites));
            this.strategyFactory = strategyFactory ?? (_ => new RandomGhostMovementStrategy());
            player = new Player(tileSize, sprite: playerImage);
        }

        public void Init()
        {
            ghosts.Clear();
            bool playerFound = false;

            for (int row = 0; row < map.Rows; row++)
            {
                for (int column = 0; column < map.Columns; column++)
                {
                    if (map.Grid[row, column] == 'P')
                    {
                        player.SpawnAt(row, column);
                        map.Grid[row, column] = '.';
                        playerFound = true;
                    }
                }
            }

            if (!playerFound)
                throw new InvalidOperationException("Map không có vị trí bắt đầu 'P'.");

            for (int row = 0; row < map.Rows; row++)
            {
                for (int column = 0; column < map.Columns; column++)
                {
                    char symbol = map.Grid[row, column];
                    if (!ghostSprites.TryGetValue(symbol, out Image? sprite))
                        continue;

                    var ghost = new Ghost(
                        player.TileSize,
                        sprite,
                        strategyFactory(symbol),
                        random);
                    ghost.SpawnAt(row, column);
                    ghost.SetPlayerTarget(player);
                    ghosts.Add(ghost);
                    map.Grid[row, column] = '.';
                }
            }
        }

        public void Update(float dt)
        {
            var (dx, dy) = input.GetMovementDirection();
            player.SetDesiredDirection(dx, dy);
            player.Update(dt, map.Grid);

            foreach (Ghost ghost in ghosts)
                ghost.Update(dt, map.Grid);
        }

        public void Render(Graphics g)
        {
            player.Render(g);

            foreach (Ghost ghost in ghosts)
                ghost.Render(g);
        }

    }
}
