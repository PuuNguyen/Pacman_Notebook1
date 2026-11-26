using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PacMan.Scenes
{
    public interface IScene
    {
        void Init();
        void Update(float dt);
        void Render(Graphics g);
    }
}
