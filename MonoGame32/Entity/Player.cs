using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Collision;
using MonoGame32.Component;
using MonoGame32.Input;
using MonoGame32.Renderable;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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
        private BoxComp _box;

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
            float _boxWidth = 12;
            float _boxHeight = 12;
            _box = new BoxComp(new Vector3(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f, 0),
                new Vector3(_position.X + _boxWidth / 2f, _position.Y + _boxHeight / 2f, 0), CollisionSetup.PlayerBit,
                CollisionSetup.PlayerMask);

            // Sprites
            var spritePosition = new Vector2(_position.X - _boxWidth / 2f, _position.Y - _boxHeight / 2f);
            _sprite = new Sprite(spritePosition, 16, 16,
                _currentTexture);
        }

        private float _currentMaxVelocityX, _currentForceX, _currentForceMaxX, _currentBrakeX;
        private float _currentMaxVelocityY, _currentForceY, _currentBrakeY;

        // Force - The force to move with, increments.
        // ForceMax - The maximum amount of force ^.
        // Brake - The force to stop player from keep moving. No input X: Brake.
        // MaxVelocity - The maximum speed obtained with force. Pixels per second to move.

        private const float WalkMaxVelocityX = 64, WalkForceX = 133, WalkForceMaxX = 64, WalkBrakeX = 266;
        private const float MinVelocityX = 2f; // 0.0003f
        private const float GroundedMaxVelocityY = 0f;
        private const float InAirMaxVelocityY = GravityY;

        //private const float WalkMaxVelocityY = 0.5f, WalkSpeedY = 5f, WalkResistanceY = 2.75f;
        private const float JumpMaxVelocityY = 64, JumpForceY = 133, JumpBrakeY = 266;
        private const float MinVelocityY = 2;

        private const float GravityY = (float) Math.PI * 24; // (float) Math.PI * 3f;
        private const float AirResistanceY = 266f;
        private const float AirForceY = 133f;
        private const float GroundedBrakeY = 0f;

        private bool _isGrounded; // Detected in collision before Tick().
        private bool _isJumping;
        private bool _hitWallLeft, _hitWallRight;

        private const float accelerationX = 10f;

        public void HandleInput(float dt)
        {
            // TODO: Rework player movement.

            _isJumping = false;

            //_velocity = Vector2.Zero;

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
                _currentForceX += WalkForceX * dt;
                _currentForceMaxX = WalkForceMaxX;

                if (_currentForceX > _currentForceMaxX)
                    _currentForceX = _currentForceMaxX;

                _currentBrakeX = WalkBrakeX;
                _velocity.X = -1;
            }

            if (InputProcessor.KeyMoveRightIsDown)
            {
                _currentMaxVelocityX = WalkMaxVelocityX;
                _currentForceX += WalkForceX * dt;
                _currentForceMaxX = WalkForceMaxX;

                if (_currentForceX > _currentForceMaxX)
                    _currentForceX = _currentForceMaxX;

                _currentBrakeX = WalkBrakeX;
                _velocity.X = 1;
            }

            if (!InputProcessor.KeyMoveLeftIsDown && !InputProcessor.KeyMoveRightIsDown)
            {
                if (_currentForceX > 0)
                    _currentForceX -= _currentBrakeX * dt;
                else
                    _currentForceX += _currentBrakeX * dt;
            }

            if (InputProcessor.KeyJumpWasDownLastFrame)
            {
                //Console.WriteLine(1);
                if (_isGrounded)
                {
                    //Console.WriteLine(2);
                    if (!_isJumping)
                    {
                        //Console.WriteLine(3);
                        _currentMaxVelocityY = JumpMaxVelocityY;
                        _currentForceY = JumpForceY;
                        _currentBrakeY = JumpBrakeY;

                        _velocity.Y = -1;

                        _isJumping = true;
                        _isGrounded = false;
                    }
                }
            }

            if (_isGrounded)
            {
                _currentMaxVelocityY = GravityY / 12f; // GroundedMaxVelocityY
                _currentForceY = 1; // 0
                _currentBrakeY = GroundedBrakeY;
            }
            else // Is in air...
            {
                if (!_isJumping)
                {
                    _currentMaxVelocityY = InAirMaxVelocityY;
                    _currentForceY = AirForceY;
                    _currentBrakeY = AirResistanceY;

                    //_velocity.Y = 1;
                }

                //_velocity.Y += GravityY * dt; // Apply gravity.
            }

            if (_velocity != Vector2.Zero) // else NaN
                _velocity.Normalize();

            //if (_velocity.X > _currentMaxVelocityX)
            //    _velocity.X = _currentMaxVelocityX;

            Console.WriteLine(_currentForceY);
             
            _velocity.X *= _currentForceX;
            _velocity.Y *= _currentForceY;

            _velocity.Y += GravityY; // Apply gravity.

            //Console.WriteLine("Velo X: " +  _velocity.X);

            if (_velocity.X > 0) // moving right?
            {
                //_velocity.X -= _currentBrakeX * dt; // resistance = slow down
                if (_velocity.X <= MinVelocityX) // stop if velocity too low
                    _velocity.X = 0;
            }

            else if (_velocity.X < 0) // moving left?
            {
                //_velocity.X += _currentBrakeX * dt; // resistance = slow down
                if (_velocity.X >= -MinVelocityX) // stop if velocity too low
                    _velocity.X = 0;
            }

            // temp
            if (_velocity.Y > 0) // moving down?
            {
                //_velocity.Y -= _currentBrakeY * dt; // resistance = slow down
                //if (_velocity.Y <= MinVelocityY) // stop if velocity too low
                //    _velocity.Y = 0;
            }
            else if (_velocity.Y < 0) // moving up? 
            {
                //_velocity.Y += _currentBrakeY * dt; // resistance = slow down
                //if (_velocity.Y >= -MinVelocityY) // stop if velocity too low
                //    _velocity.Y = 0;
            }

            //Console.WriteLine("Velo X: " +  _velocity.X);

            // Limit velocity - TODO: Is this still needed?
            if (_velocity.X > _currentMaxVelocityX) _velocity.X = _currentMaxVelocityX;
            else if (_velocity.X < -_currentMaxVelocityX) _velocity.X = -_currentMaxVelocityX;
            if (_velocity.Y > _currentMaxVelocityY) _velocity.Y = _currentMaxVelocityY;
            else if (_velocity.Y < -_currentMaxVelocityY) _velocity.Y = -_currentMaxVelocityY;

            // Add velocity force to current position.
            _position += _velocity * dt;

            //Console.WriteLine("Pos X: " + _position.X);
            Console.WriteLine("Velo X: " + _velocity.X);
            Console.WriteLine("Velo Y: " + _velocity.Y);
        }

        public override void Tick(float dt)
        {
            // decide what to do and what happens

            // ... AI here?

            // after all done

            _box.SetBoxMin(new Vector3(_position.X - _box.BoxWidth / 2f, _position.Y - _box.BoxHeight / 2f, 0));
            _box.SetBoxMax(new Vector3(_position.X + _box.BoxWidth / 2f, _position.Y + _box.BoxHeight / 2f, 0));

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
            /*_gameState.SpriteBatch.Draw(_sprite.Texture2D,
                new Rectangle((int) _sprite.Position.X, (int) _sprite.Position.Y, (int) (_sprite.Width / 16f),
                    (int) (_sprite.Height / 16f)),
                new Rectangle(0, 0, 16, 16), Color.White);*/
            //_gameState.SpriteBatch.Draw(_currentTexture, _sprite.Position, _currentRect,Color.White,0f, Vector2.Zero, 0.16f, SpriteEffects.None, 0f);
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public override void Destroy()
        {
        }

        public BoxComp GetBoxComp()
        {
            return _box;
        }

        public void OnCollision(Entity otherEntity, BoxComp otherBoxComp)
        {
            if (otherBoxComp.MaskBits == CollisionSetup.TerrainMask)
            {
                var intersection = GameMath.GameMath.GetIntersectionDepth(_box, otherBoxComp);

                //OnCollisionX(otherBoxComp);
                //OnCollisionY(otherBoxComp);

                //Console.WriteLine("Intersection X: " + intersection.X + " | Y: " + intersection.Y);

                if (Math.Abs(intersection.X) < Math.Abs(intersection.Y))
                {
                    var thisBoxWidth = Math.Abs(_box.GetBoxMax().X - _box.GetBoxMin().X);

                    if (Math.Sign(intersection.X) < 0) // Collision on the X axis
                    {
                        // Collision on entity right
                        //Console.WriteLine("COL TO PLAYER RIGHT!!!");
                        var targetLeft = otherBoxComp.GetBoxMin().X;
                        _position.X = targetLeft - thisBoxWidth / 2f;

                        _hitWallRight = true;
                    }
                    else
                    {
                        // Collision on entity left
                        //Console.WriteLine("COL TO PLAYER LEFT!!!");
                        var targetRight = otherBoxComp.GetBoxMax().X;
                        _position.X = targetRight + thisBoxWidth / 2f;

                        _hitWallLeft = true;
                    }

                    _velocity.X = 0;
                }
                else if (Math.Abs(intersection.X) > Math.Abs(intersection.Y))
                {
                    var thisBoxHeight = Math.Abs(_box.GetBox().Max.Y - _box.GetBox().Min.Y);

                    // TODO: (?) Are target's positions calculated correctly? (Top -> Min.Y & Bottom -> Max.Y)

                    if (Math.Sign(intersection.Y) < 0) // Collision on the Y axis
                    {
                        // Collision on entity bottom
                        //Console.WriteLine("COL TO PLAYER BOTTOM!!!");
                        var targetTop = otherBoxComp.GetBoxMin().Y; // was Max.Y before...
                        _position.Y = targetTop - thisBoxHeight / 2f;

                        if (_isJumping) _isJumping = false;
                        _isGrounded = true;
                    }
                    else
                    {
                        // Collision on entity top
                        //Console.WriteLine("COL TO PLAYER TOP!!!");
                        var targetBottom = otherBoxComp.GetBoxMax().Y; // was Min.Y before...
                        _position.Y = targetBottom + thisBoxHeight / 2f;

                        if (_isJumping) _isJumping = false;
                    }

                    _velocity.Y = 0;
                }

                // This box needs to move before next OnCollision is checked against another box!
                _box.SetBoxMin(new Vector3(_position.X - _box.BoxWidth / 2f, _position.Y - _box.BoxHeight / 2f, 0));
                _box.SetBoxMax(new Vector3(_position.X + _box.BoxWidth / 2f, _position.Y + _box.BoxHeight / 2f, 0));
            }
        }

        public void OnCollisionX(BoxComp otherBoxComp)
        {
            var intersection = GameMath.GameMath.GetIntersectionDepth(_box, otherBoxComp);

            //Console.WriteLine("X Intersection X: " + intersection.X + " | Y: " + intersection.Y);

            if (Math.Abs(intersection.X) < Math.Abs(intersection.Y))
            {
                var thisBoxWidth = Math.Abs(_box.GetBoxMax().X - _box.GetBoxMin().X);

                if (Math.Sign(intersection.X) < 0) // Collision on the X axis
                {
                    // Collision on entity right
                    //Console.WriteLine("COL TO PLAYER RIGHT!!!");
                    var targetLeft = otherBoxComp.GetBoxMin().X;
                    _position.X = targetLeft - thisBoxWidth / 2f;

                    _hitWallRight = true;
                }
                else
                {
                    // Collision on entity left
                    //Console.WriteLine("COL TO PLAYER LEFT!!!");
                    var targetRight = otherBoxComp.GetBoxMax().X;
                    _position.X = targetRight + thisBoxWidth / 2f;

                    _hitWallLeft = true;
                }

                _velocity.X = 0;
            }
        }

        public void OnCollisionY(BoxComp otherBoxComp)
        {
            var intersection = GameMath.GameMath.GetIntersectionDepth(_box, otherBoxComp);

            //Console.WriteLine("Y Intersection X: " + intersection.X + " | Y: " + intersection.Y);

            if (Math.Abs(intersection.X) > Math.Abs(intersection.Y))
            {
                var thisBoxHeight = Math.Abs(_box.GetBox().Max.Y - _box.GetBox().Min.Y);

                // TODO: (?) Are target's positions calculated correctly? (Top -> Min.Y & Bottom -> Max.Y)

                if (Math.Sign(intersection.Y) < 0) // Collision on the Y axis
                {
                    // Collision on entity bottom
                    //Console.WriteLine("COL TO PLAYER BOTTOM!!!");
                    var targetTop = otherBoxComp.GetBoxMin().Y; // was Max.Y before...
                    _position.Y = targetTop - thisBoxHeight / 2f;

                    if (_isJumping) _isJumping = false;
                    _isGrounded = true;
                }
                else
                {
                    // Collision on entity top
                    //Console.WriteLine("COL TO PLAYER TOP!!!");
                    var targetBottom = otherBoxComp.GetBoxMax().Y; // was Min.Y before...
                    _position.Y = targetBottom + thisBoxHeight / 2f;

                    if (_isJumping) _isJumping = false;
                }

                _velocity.Y = 0;
            }
        }
    }
}