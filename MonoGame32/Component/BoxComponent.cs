using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame32.Component
{
    public interface IBoxComponent // MonoGame BoundingBox is not extendable :( 
    {
        BoundingBox GetBoundingBox();
        short GetCategoryBits(); // What am I.
        void SetCategoryBits(short bits);
        short GetMaskBits(); // What do I collide with.
        void SetMaskBits(short bits);
        void OnCollision(Entity.Entity otherEntity, IBoxComponent otherBoxComp);
        List<BoundingBox> GetIntersectingBoxes();
    }
}