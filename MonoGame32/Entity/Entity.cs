using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.Entity
{
    public abstract class Entity
    {
        protected GameState.GameState _gameState;

        private bool _toToDestroy;

        public bool ToDestroy
        {
            get => _toToDestroy;
            set => _toToDestroy = value;
        }

        protected Entity(GameState.GameState gameState)
        {
            _gameState = gameState;
        }

        public abstract void Tick(float dt);
        public abstract void Render(float dt);
        public abstract void Destroy();
    }
}