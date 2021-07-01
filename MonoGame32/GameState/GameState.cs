using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Asset;

namespace MonoGame32.GameState
{
    public abstract class GameState
    {
        protected Game1 _game;
        protected SpriteBatch _spriteBatch;
        protected AssetsManager _assetsManager;

        public AssetsManager AssetsManager => _assetsManager;

        public SpriteBatch SpriteBatch => _spriteBatch;

        protected List<Entity.Entity> Entities = new List<Entity.Entity>();

        protected GameState(Game1 game, SpriteBatch spriteBatch, AssetsManager assetsManager)
        {
            _game = game;
            _spriteBatch = spriteBatch;
            _assetsManager = assetsManager;

            //Console.WriteLine("GameState constructor");
        }

        public abstract void HandleInput(float dt);
        public abstract void Tick(float dt);
        public abstract void Render(float dt);

        public virtual void ExitState()
        {
            if (Entities.Count != 0)
            {
                foreach (var entity in Entities)
                {
                    entity.ToDestroy = true;
                }

                foreach (var entity in Entities)
                {
                    entity.Destroy();
                }

                Entities.Clear();
            }
        }
    }
}