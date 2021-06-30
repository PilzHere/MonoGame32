using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.GameState
{
    public abstract class GameState
    {
        protected Game1 _game;
        protected SpriteBatch _spriteBatch;

        public SpriteBatch SpriteBatch => SpriteBatch;

        protected List<Entity.Entity> Entities = new List<Entity.Entity>();

        protected GameState(Game1 game, SpriteBatch spriteBatch)
        {
            _game = game;
            _spriteBatch = spriteBatch;
            
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