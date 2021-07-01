using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Asset;
using MonoGame32.Renderable;

namespace MonoGame32.Entity
{
    public class Player : Entity
    {
        private Vector2 _position;
        private Sprite _sprite;
        private Rectangle _rectangle;
        private Point _rectanglePoint;

        private Texture2D _currentTexture;

        public Player(GameState.GameState gameState, Vector2 position) : base(gameState)
        {
            _position = position;
            _currentTexture = _gameState.AssetsManager.GetTexture("Textures/default");

            _sprite = new Sprite(_position, _currentTexture.Width, _currentTexture.Height,
                _currentTexture);

            //_rectanglePoint.X = (int) _position.X;
            //_rectanglePoint.Y = (int) _position.Y;
            //_rectangle.Location = _rectanglePoint;

            _rectangle = new Rectangle((int) _position.X, (int) _position.Y, 16, 16);
        }

        private bool goRight = true;
        private bool goNorth = true;
        private float speed = 50.0f;
        
        public override void Tick(float dt)
        {   
            if (_position.X <= 0) goRight = true;
            else if (_position.X >= 195) goRight = false;
            
            if (_position.Y <= 0) goNorth = false;
            else if (_position.Y >= 100) goNorth = true;

            _position.X = goRight ? _position.X + speed * dt : _position.X - speed * dt;
            _position.Y = goNorth ? _position.Y - speed * dt : _position.Y + speed * dt;
            
            /*if (goRight) _position.X += speed * dt;
            else _position.X -= speed * dt;
            
            if (goNorth) _position.Y -= speed * dt;
            else _position.Y += speed * dt;*/
            
                
            
            //_rectanglePoint.X = (int) _position.X;
            //_rectanglePoint.Y = (int) _position.Y;

            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;
            _sprite.Position = _position;

            //_rectangle.Location = _rectanglePoint;
            //_sprite.Position = _rectangle.Location.ToVector2();
            //_sprite.Position = _position;
            //_rectangle.Offset(_position);
            //_sprite.Position = _position;

        }

        public override void Render(float dt)
        {
            _gameState.SpriteBatch.Draw(_sprite.Texture2D, _position, Color.White);
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public override void Destroy()
        {
        }
    }
}