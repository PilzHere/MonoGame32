using Microsoft.Xna.Framework;

namespace MonoGame32.Component
{
    public class BoxComp
    {
        private BoundingBox _box;
        private float _boxWidth, _boxHeight, _boxMidX, _boxMidY;
        private short _categoryBits, _maskBits;

        public BoxComp(Vector3 min, Vector3 max, short categoryBits, short maskBits)
        {
            _box = new BoundingBox(min, max);
            _categoryBits = categoryBits;
            _maskBits = maskBits;
            
            _boxWidth = _box.Max.X - _box.Min.X;
            _boxHeight = _box.Max.Y - _box.Min.Y;
            
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

        public float BoxWidth
        {
            get => _boxWidth = _box.Max.X - _box.Min.X;
        }

        public float BoxHeight
        {
            get => _boxHeight = _box.Max.Y - _box.Min.Y;
        }

        public float BoxMidX
        {
            get => _boxMidX = _box.Min.X + _boxWidth / 2f;
        }

        public float BoxMidY
        {
            get => _boxMidY = _box.Min.Y + _boxHeight / 2f;
        }
    }
}