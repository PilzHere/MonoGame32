using System;
using Microsoft.Xna.Framework;

namespace MonoGame32.GameMath
{
    public static class GameMath
    {
        private static float _deltaTime;

        public static float DeltaTime => _deltaTime;

        private static float _fps;

        public static float Fps => _fps;

        private static int _smoothedFps;

        public static int SmoothedFps => _smoothedFps;

        public static void CalculateDeltaTime(GameTime gameTime)
        {
            _deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static void CalculateFps()
        {
            _fps = (1 / _deltaTime);
            _smoothedFps = (int) Math.Round(_fps);
        }
    }
}