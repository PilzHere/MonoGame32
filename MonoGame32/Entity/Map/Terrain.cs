using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Collision;
using MonoGame32.Component;
using MonoGame32.Renderable;

namespace MonoGame32.Entity
{
    public class Terrain : Entity, IBoxComponent
    {
        // Positions
        private Vector2 _position;
        
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
        
        public Terrain(GameState.GameState gameState, Vector2 position) : base(gameState)
        {
            // Positions
            _position = position;
            //_oldPosition = _position;
            //_velocity = Vector2.Zero;

            // Textures
            _currentTexture = _gameState.AssetsManager.GetTexture("Textures/default");
            _rects[0] = new Rectangle(16, 0, 16, 16);
            _currentRect = _rects[0];

            // Boxes
            _boxWidth = 16;
            _boxHeight = 16;
            _boxCategoryBits = CollisionSetup.TerrainBit;
            _boxCategoryMask = CollisionSetup.TerrainMask;
            _box = new BoundingBox(new Vector3(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0),
                new Vector3(_position.X + _boxWidth / 2f, _position.Y + _boxHeight / 2f, 0));

            // Sprites
            var spritePosition = new Vector2(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f);
            _sprite = new Sprite(spritePosition, 16, 16,
                _currentTexture);
        }

        public override void Tick(float dt)
        {
            //throw new System.NotImplementedException();
            //Console.WriteLine("box..." + _boxWidth/2f);
            
            //_box.Min = new Vector3(_position.X - _boxWidth / 2f, _position.Y + _boxHeight / 2f, 0);
            //_box.Max = new Vector3(_position.X + _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0);
        }

        public override void Render(float dt)
        {
            _gameState.SpriteBatch.Draw(_sprite.Texture2D, _sprite.Position, _currentRect, Color.White);
        }

        public override void Destroy()
        {
            //throw new System.NotImplementedException();
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
            
        }

        public List<BoundingBox> GetIntersectingBoxes()
        {
            throw new NotImplementedException();
        }

        public bool OnCollisionX(Entity otherEntity, IBoxComponent otherBoxComp)
        {
            //throw new System.NotImplementedException();
            return false;
        }

        public bool OnCollisionY(Entity otherEntity, IBoxComponent otherBoxComp)
        {
            //throw new System.NotImplementedException();
            return false;
        }
    }
}