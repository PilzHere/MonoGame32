using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame32.Component
{
    public class BoxComp
    {
        private BoundingBox _box;
        private float _boxWidth, _boxHeight, _boxMidX, _boxMidY;
        private short _categoryBits, _maskBits;

        public List<BoxComp> overlapingBoxes = new List<BoxComp>();
        
        public Vector2 intersection;// = GameMath.GameMath.GetIntersectionDepth(thisBox, boxA);
        public float intersectionArea;// = Math.Abs(intersectionA.X * intersectionA.Y);

        public BoxComp(Vector3 min, Vector3 max, short categoryBits, short maskBits)
        {
            _box = new BoundingBox(min, max);
            _categoryBits = categoryBits;
            _maskBits = maskBits;
            
            _boxWidth = Math.Abs(_box.Max.X - _box.Min.X);
            _boxHeight = Math.Abs(_box.Max.Y - _box.Min.Y);
            
            _boxMidX = _box.Min.X + _boxWidth / 2f;
            _boxMidY = _box.Min.Y + _boxHeight / 2f;
        }

        public short CategoryBits
        {
            get => _categoryBits;
        }

        public short MaskBits
        {
            get => _maskBits;
        }

        public BoundingBox GetBox()
        {
            return _box;
        }

        public Vector3 GetBoxMin()
        {
            return _box.Min;
        }

        public void SetBoxMin(Vector3 newMin)
        {
            _box.Min = newMin;
        }
        
        public Vector3 GetBoxMax()
        {
            return _box.Max;
        }

        public void SetBoxMax(Vector3 newMax)
        {
            _box.Max = newMax;
        }

        public float BoxWidth => _boxWidth;

        public float BoxHeight => _boxHeight;

        public float BoxMidX => _boxMidX = _box.Min.X + _boxWidth / 2f;

        public float BoxMidY
        {
            get => _boxMidY = _box.Min.Y + _boxHeight / 2f;
        }
    }
}