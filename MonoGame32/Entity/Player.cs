using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
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
        //private Vector2 _newPosition; // Lets not use this anymore?

        private Vector2 _velocity;
        private Vector2 _oldVelocity;

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
            _velocity = Vector2.Zero;
            _oldVelocity = _velocity;

            // Textures
            _currentTexture = _gameState.AssetsManager.GetTexture("Textures/default");
            _rects[0] = new Rectangle(0, 0, 16, 16);
            _currentRect = _rects[0];

            // Boxes
            _boxWidth = 12;
            _boxHeight = 12;
            _boxCategoryBits = CollisionSetup.PlayerBit;
            _boxCategoryMask = CollisionSetup.PlayerMask;
            _box = new BoundingBox(new Vector3(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0),
                new Vector3(_position.X + _boxWidth / 2f, _position.Y + _boxHeight / 2f, 0));

            // Sprites
            var spritePosition = new Vector2(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f);
            _sprite = new Sprite(spritePosition, 16, 16,
                _currentTexture);
        }

        private float _currentMaxVelocityX, _currentForceX, _currentResistanceX;
        private float _currentMaxVelocityY, _currentForceY, _currentResistanceY;

        private const float WalkMaxVelocityX = 0.5f, WalkForceX = 5f, WalkResistanceX = 2.75f;
        private const float MinVelocityX = 0.015f;
        private const float GroundedMaxVelocityY = 0f;
        private const float InAirMaxVelocityY = GravityY;

        //private const float WalkMaxVelocityY = 0.5f, WalkSpeedY = 5f, WalkResistanceY = 2.75f;
        private const float JumpMaxVelocityY = 1.5f, JumpForceY = 300f;
        private const float MinVelocityY = 0.015f;

        private const float GravityY = (float) Math.PI * 2f;
        private const float AirResistanceY = 1f;
        private const float GroundedResistanceY = 0f;

        private bool _isGrounded; // Detected in collision before Tick().
        private bool _isJumping;
        private bool _hitWallLeft, _hitWallRight;

        public void HandleInput(float dt)
        {
            _isJumping = false;

            // Reset input sensitive data
            //_currentMaxVelocityX = 0;
            //_currentForceX = 0;
            //_currentResistanceX = 0;

            // Read input
            if (InputProcessor.KeyMoveUpIsDown)
            {
                // temp
                /*_currentMaxVelocityY = WalkMaxVelocityY;
                _currentSpeedY = WalkSpeedY;
                _currentResistanceY = WalkResistanceY;

                _velocity.Y -= _currentSpeedY * dt;*/
            }

            if (InputProcessor.KeyMoveDownIsDown)
            {
                // temp
                /*_currentMaxVelocityY = WalkMaxVelocityY;
                _currentSpeedY = WalkSpeedY;
                _currentResistanceY = WalkResistanceY;

                _velocity.Y += _currentSpeedY * dt;*/
            }

            if (InputProcessor.KeyMoveLeftIsDown)
            {
                _currentMaxVelocityX = WalkMaxVelocityX;
                _currentForceX = -WalkForceX;
                _currentResistanceX = WalkResistanceX;

                _velocity.X += _currentForceX * dt;
            }

            if (InputProcessor.KeyMoveRightIsDown)
            {
                _currentMaxVelocityX = WalkMaxVelocityX;
                _currentForceX = WalkForceX;
                _currentResistanceX = WalkResistanceX;

                _velocity.X += _currentForceX * dt;
            }

            if (InputProcessor.KeyJumpWasDownLastFrame)
            {
                Console.WriteLine(1);
                if (_isGrounded)
                {
                    Console.WriteLine(2);
                    if (!_isJumping)
                    {
                        Console.WriteLine(3);
                        _currentMaxVelocityY = JumpMaxVelocityY;
                        _currentForceY = -JumpForceY;
                        _currentResistanceY = 1f;

                        _isJumping = true;
                        _isGrounded = false;
                    }
                }
            }

            if (_isGrounded)
            {
                _currentMaxVelocityY = GravityY; // GroundedMaxVelocityY
                _currentForceY = GravityY; // 0
                _currentResistanceY = GroundedResistanceY;
            }
            else // Is in air...
            {
                if (!_isJumping)
                {
                    _currentMaxVelocityY = InAirMaxVelocityY;
                    _currentForceY = 1f;
                    _currentResistanceY = AirResistanceY;
                }

                //_velocity.Y += GravityY * dt; // Apply gravity.
            }

            _velocity.Y += _currentForceY * dt;
            
            _velocity.Y += GravityY * dt; // Apply gravity.

            if (_velocity.X > 0) // moving right?
            {
                _velocity.X -= _currentResistanceX * dt; // resistance = slow down
                if (_velocity.X <= MinVelocityX) // stop if velocity too low
                    _velocity.X = 0;
            }

            else if (_velocity.X < 0) // moving left?
            {
                _velocity.X += _currentResistanceX * dt; // resistance = slow down
                if (_velocity.X >= -MinVelocityX) // stop if velocity too low
                    _velocity.X = 0;
            }

            // temp
            if (_velocity.Y > 0) // moving down?
            {
                _velocity.Y -= _currentResistanceY * dt; // resistance = slow down
                //if (_velocity.Y <= MinVelocityY) // stop if velocity too low
                //    _velocity.Y = 0;
            }
            else if (_velocity.Y < 0) // moving up? 
            {
                _velocity.Y += _currentResistanceY * dt; // resistance = slow down
                //if (_velocity.Y >= -MinVelocityY) // stop if velocity too low
                //    _velocity.Y = 0;
            }

            // Limit velocity
            if (_velocity.X > _currentMaxVelocityX) _velocity.X = _currentMaxVelocityX;
            else if (_velocity.X < -_currentMaxVelocityX) _velocity.X = -_currentMaxVelocityX;
            if (_velocity.Y > _currentMaxVelocityY) _velocity.Y = _currentMaxVelocityY;
            else if (_velocity.Y < -_currentMaxVelocityY) _velocity.Y = -_currentMaxVelocityY;

            // Add velocity force to current position.
            _position += _velocity;
            
            Console.WriteLine("Pos Y: " +  _position.Y);
            Console.WriteLine("Velo Y: " +  _velocity.Y);
        }

        public override void Tick(float dt)
        {
            // decide what to do and what happens

            // ... AI here?

            // after all done

            _box.Min = new Vector3(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0);
            _box.Max = new Vector3(_position.X + _boxWidth / 2f, _position.Y + _boxHeight / 2f, 0);

            _sprite.Position = new Vector2(_position.X - _sprite.Width / 2f, _position.Y - _sprite.Height / 2f);

            _oldPosition = _position;
            _oldVelocity = _velocity;

            // Reset data from old tick/collision
            _isGrounded = false;
            _hitWallLeft = false;
            _hitWallRight = false;
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
            if (otherBoxComp.GetMaskBits() == CollisionSetup.TerrainMask)
            {
                var intersection = GameMath.GameMath.GetIntersectionDepth(_box, otherBoxComp.GetBoundingBox());

                Console.WriteLine("Intersection X: " + intersection.X + " | Y: " + intersection.Y);
                
                if (Math.Abs(intersection.X) < Math.Abs(intersection.Y))
                {
                    var thisBoxWidth = Math.Abs(_box.Max.X - _box.Min.X);

                    if (Math.Sign(intersection.X) < 0) // Collision on the X axis
                    {
                        // Collision on entity right
                        //Console.WriteLine("COL TO PLAYER RIGHT!!!");
                        var targetLeft = otherBoxComp.GetBoundingBox().Min.X;
                        _position.X = targetLeft - thisBoxWidth / 2f;

                        _hitWallRight = true;
                    }
                    else
                    {
                        // Collision on entity left
                        //Console.WriteLine("COL TO PLAYER LEFT!!!");
                        var targetRight = otherBoxComp.GetBoundingBox().Max.X;
                        _position.X = targetRight + thisBoxWidth / 2f;

                        _hitWallLeft = true;
                    }

                    _velocity.X = 0;
                }
                else if (Math.Abs(intersection.X) > Math.Abs(intersection.Y))
                {
                    var thisBoxHeight = Math.Abs(_box.Max.Y - _box.Min.Y);

                    // TODO: (?) Are target's positions calculated correctly? (Top -> Min.Y & Bottom -> Max.Y)

                    if (Math.Sign(intersection.Y) < 0) // Collision on the Y axis
                    {
                        // Collision on entity bottom
                        //Console.WriteLine("COL TO PLAYER BOTTOM!!!");
                        var targetTop = otherBoxComp.GetBoundingBox().Min.Y; // was Max.Y before...
                        _position.Y = targetTop - thisBoxHeight / 2f;

                        if (_isJumping) _isJumping = false;
                        _isGrounded = true;
                    }
                    else
                    {
                        // Collision on entity top
                        //Console.WriteLine("COL TO PLAYER TOP!!!");
                        var targetBottom = otherBoxComp.GetBoundingBox().Max.Y; // was Min.Y before...
                        _position.Y = targetBottom + thisBoxHeight / 2f;

                        if (_isJumping) _isJumping = false;
                    }

                    _velocity.Y = 0;
                }
                /*else
                {
                    Console.WriteLine("WTF! Intersection X: " + intersection.X + " | Y: " + intersection.Y);
                }*/
            }
        }
        
        private List<BoundingBox> intersectingBoxes = new List<BoundingBox>();

        public List<BoundingBox> GetIntersectingBoxes()
        {
            return intersectingBoxes;
        }
    }
}