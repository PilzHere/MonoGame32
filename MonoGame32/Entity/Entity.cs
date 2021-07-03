using System;

namespace MonoGame32.Entity
{
    public abstract class Entity
    {
        protected GameState.GameState _gameState;
        protected Guid _id; // Unique id.
        public bool ToDestroy { get; set; }

        protected Entity(GameState.GameState gameState)
        {
            _gameState = gameState;
            _id = Guid.NewGuid();
        }

        public Guid Id => _id;

        public abstract void Tick(float dt);
        public abstract void Render(float dt);
        public abstract void Destroy();
    }
}