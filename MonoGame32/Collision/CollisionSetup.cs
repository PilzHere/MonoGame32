using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using MonoGame32.Component;

namespace MonoGame32.Collision
{
    public static class CollisionSetup
    {
        // Category bits
        public const short DefaultBit = 1, PlayerBit = 2, TerrainBit = 4, EnemyBit = 8; // 16, 32, 64...

        // Masks
        public const short TerrainMask = DefaultBit | PlayerBit;
        public const short PlayerMask = DefaultBit | TerrainBit;

        private static List<Entity.Entity> _entities;

        public static void SetEntities(List<Entity.Entity> entities)
        {
            _entities = entities;
        }

        //private static BoundingBox currentBoundingBox;
        
        public static void CheckCollision(Entity.Entity thisEntity, IBoxComponent thisBoxComp)
        {
            // https://stackoverflow.com/questions/14479981/how-do-i-check-if-bitmask-contains-bit

            foreach (var entity in _entities)
            {
                if (!(entity is IBoxComponent otherBoxComp)) continue; // No box? NEXT!
                if (otherBoxComp.GetBoundingBox() == thisBoxComp.GetBoundingBox()) continue; // Not the same box.
                if ((otherBoxComp.GetMaskBits() & thisBoxComp.GetCategoryBits()) !=
                    thisBoxComp.GetCategoryBits()) continue; // Categorybits exist in maskbits?
                if (thisBoxComp.GetBoundingBox().Intersects(otherBoxComp.GetBoundingBox())) // Intersection?
                {
                    if (GameSettings.GameSettings.PrintCollisionsInformation)
                        Console.WriteLine(thisBoxComp.GetCategoryBits() + " collides with: " + otherBoxComp.GetCategoryBits());
                        //Console.WriteLine(thisEntity.Id + " collides with: " + entity.Id);

                        thisBoxComp.OnCollision(entity, otherBoxComp); // Entity act on collision.    
                }
                
                //thisBoxComp.OnCollision(entity, otherBoxComp); // Entity act on collision.
            }
        }
    }
    
    /*class AComparer : IComparer<BoundingBox>
    {
        public int Compare(BoundingBox thisBox, BoundingBox boxA, BoundingBox boxB)
        {
            var intersectionA = GameMath.GameMath.GetIntersectionDepth(thisBox, boxA);
            var intersectionVolumeA = Math.Abs(intersectionA.X * intersectionA.Y);
            
            var intersectionB = GameMath.GameMath.GetIntersectionDepth(thisBox, boxB);
            var intersectionVolumeB = Math.Abs(intersectionB.X * intersectionB.Y);

            if (intersectionVolumeA < intersectionVolumeB) return -1;
            if (intersectionVolumeA > intersectionVolumeB) return 1;
            return 0;
        }
    }*/
}