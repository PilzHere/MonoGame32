using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.Renderable
{
    public class Sprite
    {
        public Sprite(Vector2 position, int width, int height, Texture2D texture2D)
        {
            Position = position;
            Width = width;
            Height = height;
            Texture2D = texture2D;

            LayerDepth = 0;
        }

        public Vector2 Position { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Texture2D Texture2D { get; set; }

        public float LayerDepth { get; set; }
    }
}