using System;
using Microsoft.Xna.Framework;
using MonoGame32.Component;

namespace MonoGame32.GameMath
{
    public static class GameMath
    {
        private const double KiloByte = 1024d;

        private static float _deltaTime;

        public static float DeltaTime => _deltaTime;

        private static float _fps;

        public static float Fps => _fps;

        private static int _smoothedFps;

        public static int SmoothedFps => _smoothedFps;

        public static void CalculateDeltaTime(GameTime gameTime)
        {
            _deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        public static void CalculateFps()
        {
            _fps = (1 / _deltaTime);
            _smoothedFps = (int) Math.Round(_fps);
        }

        public static double BytesToKilobytes(int bytes)
        {
            return bytes / KiloByte;
        }

        public static double BytesToKilobytes(long bytes)
        {
            return bytes / KiloByte;
        }

        public static double BytesToMegabytes(int bytes)
        {
            return bytes / KiloByte / KiloByte;
        }

        public static double BytesToMegabytes(long bytes)
        {
            return bytes / KiloByte / KiloByte;
        }

        public static Vector2 GetIntersectionDepth(BoxComp thisBox, BoxComp otherBox)
        {
            // https://stackoverflow.com/questions/46172953/aabb-collision-resolution-slipping-sides

            /*var thisBoxWidth = thisBox.Max.X - thisBox.Min.X;
            var thisBoxHeight = thisBox.Max.Y - thisBox.Min.Y;
            var thisBoxMidX = thisBox.Min.X + thisBoxWidth / 2f;
            var thisBoxMidY = thisBox.Min.Y + thisBoxHeight / 2f;

            var otherBoxWidth = otherBox.Max.X - otherBox.Min.X;
            var otherBoxHeight = otherBox.Max.Y - otherBox.Min.Y;
            var otherBoxMidX = otherBox.Min.X + otherBoxWidth / 2f;
            var otherBoxMidY = otherBox.Min.Y + otherBoxHeight / 2f;*/

            // Calculate current and minimum-non-intersecting distances between centers.
            var distanceX = thisBox.BoxMidX - otherBox.BoxMidX;
            var distanceY = thisBox.BoxMidY - otherBox.BoxMidY;
            var minDistanceX = thisBox.BoxWidth / 2f + otherBox.BoxWidth / 2f;
            var minDistanceY = thisBox.BoxHeight / 2f + otherBox.BoxHeight / 2f;
            
            /*var distanceX = thisBoxMidX - otherBoxMidX;
            var distanceY = thisBoxMidY - otherBoxMidY;
            var minDistanceX = thisBoxWidth / 2f + otherBoxWidth / 2f;
            var minDistanceY = thisBoxHeight / 2f + otherBoxHeight / 2f;*/
            
            /*Console.WriteLine("COLL INFO:");
            Console.WriteLine("thisBoxWidth: " + thisBoxWidth);
            Console.WriteLine("thisBoxHeight: " + thisBoxHeight);
            Console.WriteLine("thisBoxMidX: " + thisBoxMidX);
            Console.WriteLine("thisBoxMidY: " + thisBoxMidY);
            Console.WriteLine("otherBoxWidth: " + otherBoxWidth);
            Console.WriteLine("otherBoxHeight: " + otherBoxHeight);
            Console.WriteLine("otherBoxMidX: " + otherBoxMidX);
            Console.WriteLine("otherBoxMidY: " + otherBoxMidY);*/

            // If we are not intersecting at all, return 0.
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            {
                return Vector2.Zero;
            }

            // Calculate and return intersection depths.
            var depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            var depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

            return new Vector2(depthX, depthY);
        }
    }
}