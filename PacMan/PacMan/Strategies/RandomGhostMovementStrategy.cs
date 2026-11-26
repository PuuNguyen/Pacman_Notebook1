using System;
using System.Collections.Generic;
using PacMan.Entities;

namespace PacMan.Strategies
{
    public sealed class RandomGhostMovementStrategy : IGhostMovementStrategy
    {
        private static readonly (int dx, int dy)[] Directions =
        [
            (0, -1),
            (0, 1),
            (-1, 0),
            (1, 0)
        ];

        public (int dx, int dy) ChooseDirection(
            Ghost ghost,
            Player player,
            char[,] grid,
            Random random)
        {
            var validDirections = new List<(int dx, int dy)>();

            foreach (var direction in Directions)
            {
                int nextRow = ghost.Row + direction.dy;
                int nextColumn = ghost.Col + direction.dx;

                if (CanMoveTo(nextRow, nextColumn, grid))
                    validDirections.Add(direction);
            }

            if (validDirections.Count == 0)
                return (0, 0);

            if (validDirections.Count > 1)
            {
                int oppositeDx = -ghost.DirX;
                int oppositeDy = -ghost.DirY;
                validDirections.RemoveAll(direction =>
                    direction.dx == oppositeDx && direction.dy == oppositeDy);
            }

            if (validDirections.Count == 0)
                return (-ghost.DirX, -ghost.DirY);

            return validDirections[random.Next(validDirections.Count)];
        }

        private static bool CanMoveTo(int row, int column, char[,] grid)
        {
            return row >= 0 &&
                   row < grid.GetLength(0) &&
                   column >= 0 &&
                   column < grid.GetLength(1) &&
                   grid[row, column] != '#';
        }
    }
}
