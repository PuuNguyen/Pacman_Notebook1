using System;
using System.Diagnostics;
using System.Windows.Forms;
using PacMan.Scenes;

namespace PacMan.Core
{
    public class GameLoop : IDisposable
    {
        private readonly Control canvas;
        private readonly IScene scene;
        private readonly System.Windows.Forms.Timer timer;
        private readonly Stopwatch stopwatch = new();
        private bool isRunning;
        private bool isInitialized;

        public GameLoop(Control canvas, IScene scene, int targetFps = 60)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene));

            if (targetFps <= 0)
                throw new ArgumentOutOfRangeException(nameof(targetFps));

            timer = new System.Windows.Forms.Timer
            {
                Interval = Math.Max(1, 1000 / targetFps)
            };

            timer.Tick += OnTick;
            canvas.Paint += OnCanvasPaint;
        }

        public void Start()
        {
            if (isRunning)
                return;

            if (!isInitialized)
            {
                scene.Init();
                isInitialized = true;
            }
            stopwatch.Restart();
            isRunning = true;
            timer.Start();
        }

        public void Stop()
        {
            if (!isRunning)
                return;

            timer.Stop();
            stopwatch.Stop();
            isRunning = false;
        }

        private void OnTick(object? sender, EventArgs e)
        {
            double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();

            float deltaTime = (float)Math.Min(elapsedSeconds, 0.1);
            scene.Update(deltaTime);
            canvas.Invalidate();
        }

        private void OnCanvasPaint(object? sender, PaintEventArgs e)
        {
            scene.Render(e.Graphics);
        }

        public void Dispose()
        {
            Stop();
            timer.Tick -= OnTick;
            canvas.Paint -= OnCanvasPaint;
            timer.Dispose();
        }
    }
}
