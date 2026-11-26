using System;
using PacMan.Entities;

namespace PacMan.Strategies
{
    public interface IGhostMovementStrategy
    {
        (int dx, int dy) ChooseDirection(
            Ghost ghost,
            Player player,
            char[,] grid,
            Random random);
    }
}
