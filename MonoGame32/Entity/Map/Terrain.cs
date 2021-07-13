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
        private BoxComp _box;
        
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
            float _boxWidth = 16;
            float _boxHeight = 16;
            _box = new BoxComp(new Vector3(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0),
                new Vector3(_position.X + _boxWidth / 2f, _position.Y + _boxHeight / 2f, 0), CollisionSetup.TerrainBit, CollisionSetup.TerrainMask);

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

        public BoxComp GetBoxComp()
        {
            return _box;
        }

        public void OnCollision(Entity otherEntity, BoxComp otherBoxComp)
        {
            
        }
    }
}