using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.Renderable
{
    public class Sprite
    {
        private Vector2 _position;
        private int _width, _height;
        private Texture2D _texture2D;
        private float _layerDepth;

        public Sprite(Vector2 position, int width, int height, Texture2D texture2D)
        {
            _position = position;
            _width = width;
            _height = height;
            _texture2D = texture2D;

            _layerDepth = 0;
        }

        public Vector2 Position
        {
            get => _position;
            set { _position = value;
                //_texture2D.Bounds.Offset(_position.X, _position.Y);
                //_texture2D.Bounds.X = _position.X;
            }
        }

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public Texture2D Texture2D
        {
            get => _texture2D;
            set => _texture2D = value;
        }

        public float LayerDepth
        {
            get => _layerDepth;
            set => _layerDepth = value;
        }
    }
}