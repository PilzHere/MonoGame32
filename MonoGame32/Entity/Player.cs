using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Renderable;

namespace MonoGame32.Entity
{
    public class Player : Entity
    {
        private Vector2 _position;
        private Sprite _sprite;
        private Rectangle _rectangle;
        private Point _rectanglePoint;
        
        public Player(GameState.GameState gameState, Vector2 position) : base(gameState)
        {
            _position = position;

            //_sprite = new Sprite(_position, 16, 16, null);

            _rectanglePoint.X = (int)_position.X;
            _rectanglePoint.Y = (int)_position.Y;
            //_rectangle = new Rectangle((int)_position.X, (int)_position.Y, 16, 16);
        }

        public override void Tick(float dt)
        {
            _rectanglePoint.X = (int)_position.X;
            _rectanglePoint.Y = (int)_position.Y;
            
            _rectangle.Location = _rectanglePoint;
            _sprite.Position = _position;
        }

        public override void Render(float dt)
        {
            //_gameState.SpriteBatch.Draw(_sprite.Texture2D, _position, _rectangle, Color.White);
        }

        public override void Destroy()
        {
            
        }
    }
}