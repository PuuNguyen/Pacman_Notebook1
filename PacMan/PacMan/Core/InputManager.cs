using System.Collections.Generic;
using System.Windows.Forms;

namespace PacMan.Core
{
    public class InputManager
    {
        private readonly HashSet<Keys> _pressedKeys = new HashSet<Keys>();

        public void KeyDown(Keys key)
        {
            _pressedKeys.Add(key);
        }

        public void KeyUp(Keys key)
        {
            _pressedKeys.Remove(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _pressedKeys.Contains(key);
        }

        public (int dx, int dy) GetMovementDirection()
        {
            if (IsKeyDown(Keys.Up) || IsKeyDown(Keys.W)) return (0, -1);
            if (IsKeyDown(Keys.Down) || IsKeyDown(Keys.S)) return (0, 1);
            if (IsKeyDown(Keys.Left) || IsKeyDown(Keys.A)) return (-1, 0);
            if (IsKeyDown(Keys.Right) || IsKeyDown(Keys.D)) return (1, 0);
            return (0, 0);
        }
    }
}
