using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.GameState
{
    public abstract class GameState
    {
        protected Game1 Game;
        protected SpriteBatch SpriteBatch;

        protected GameState(Game1 game, SpriteBatch spriteBatch)
        {
            Game = game;
            SpriteBatch = spriteBatch;
            Console.WriteLine("GameState constructor");
        }

        public abstract void HandleInput(float dt);
        public abstract void Tick(float dt);
        public abstract void Render(float dt);
        public abstract void ExitState();
    }
}