using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Collision;
using MonoGame32.Component;
using MonoGame32.Input;
using MonoGame32.Renderable;

namespace MonoGame32.Entity
{
    public class Player : Entity, IBoxComponent
    {
        // Positions
        private Vector2 _position;
        private Vector2 _oldPosition;

        // Textures
        private Texture2D _currentTexture;
        private Rectangle _currentRect;
        private Rectangle[]
            _rects = new Rectangle[1]; // subtextures from currenttexture which should be a textureAtlas.

        // Sprites
        private Sprite _sprite;

        // Boxes
        private BoundingBox _box;
        private short _boxCategoryBits, _boxCategoryMask;
        private float _boxWidth, _boxHeight;

        public Player(GameState.GameState gameState, Vector2 position) : base(gameState)
        {
            // Positions
            _position = position;
            _oldPosition = _position;

            // Textures
            _currentTexture = _gameState.AssetsManager.GetTexture("Textures/default");
            _rects[0] = new Rectangle(0, 0, 16, 16);
            _currentRect = _rects[0];

            // Boxes
            _boxWidth = 16;
            _boxHeight = 16;
            _boxCategoryBits = CollisionSetup.PlayerBit;
            _boxCategoryMask = CollisionSetup.PlayerMask;
            _box = new BoundingBox(new Vector3(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0),
                new Vector3(_position.X + _boxWidth / 2f, _position.Y + _boxHeight / 2f, 0));

            // Sprites
            var spritePosition = new Vector2(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f);
            _sprite = new Sprite(spritePosition, 16, 16,
                _currentTexture);
        }

        private const float Speed = 100.0f;

        public void HandleInput(float dt)
        {
            if (InputProcessor.KeyMoveUpIsDown)
                _position.Y -= Speed * dt;
            
            if (InputProcessor.KeyMoveDownIsDown)
                _position.Y += Speed * dt;
            
            if (InputProcessor.KeyMoveLeftIsDown)
                _position.X -= Speed * dt;

            if (InputProcessor.KeyMoveRightIsDown)
                _position.X += Speed * dt;
        }
        
        public override void Tick(float dt)
        {
            // new position set?

            //decide what to do and what happens

            // after all done

            _box.Min = new Vector3(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0);
            _box.Max = new Vector3(_position.X + 8, _position.Y + 8, 0);

            _sprite.Position = new Vector2(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f);

            _oldPosition = _position;

            //Console.WriteLine("pos: " + _position);
        }

        public override void Render(float dt)
        {
            _gameState.SpriteBatch.Draw(_sprite.Texture2D, _sprite.Position, _currentRect, Color.White);
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public override void Destroy()
        {
        }

        public BoundingBox GetBoundingBox()
        {
            return _box;
        }

        public short GetCategoryBits()
        {
            return _boxCategoryBits;
        }

        public void SetCategoryBits(short bits)
        {
            _boxCategoryBits = bits;
        }

        public short GetMaskBits()
        {
            return _boxCategoryMask;
        }

        public void SetMaskBits(short bits)
        {
            _boxCategoryMask = bits;
        }

        public void OnCollision(Entity otherEntity, IBoxComponent otherBoxComp)
        {
            Console.WriteLine(_id + " collides with: " + otherEntity.Id);

            if (otherBoxComp.GetMaskBits() == CollisionSetup.PlayerMask)
            {
                OnCollisionX(otherEntity, otherBoxComp);
                OnCollisionY(otherEntity, otherBoxComp);
                
                Console.WriteLine("pos: " + _position);
                Console.WriteLine("oldPos: " +_oldPosition);
            }
        }

        public void OnCollisionX(Entity otherEntity, IBoxComponent otherBoxComp)
        {
            // Use new X and old Y.
            _box.Min = new Vector3(_position.X - _boxWidth / 2f, _oldPosition.Y - _boxHeight / 2f, 0);
            _box.Max = new Vector3(_position.X + 8, _oldPosition.Y + 8, 0);

            if (_box.Intersects(otherBoxComp.GetBoundingBox()))
            {
                // move back
                _position.X = _oldPosition.X;
            }
        }

        public void OnCollisionY(Entity otherEntity, IBoxComponent otherBoxComp)
        {
            // Use old X and new Y.
            _box.Min = new Vector3(_oldPosition.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0);
            _box.Max = new Vector3(_oldPosition.X + 8, _position.Y + 8, 0);

            if (_box.Intersects(otherBoxComp.GetBoundingBox()))
            {
                // move back
                _position.Y = _oldPosition.Y;
            }
        }
    }
}