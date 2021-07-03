using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Microsoft.Xna.Framework;
using MonoGame32.Component;

namespace MonoGame32.Collision
{
    public static class CollisionSetup
    {
        // Category bits
        public const short DefaultBit = 1, PlayerBit = 2, Enemy = 4, Wall = 8; // 16, 32, 64...

        // Masks
        public const short PlayerMask = DefaultBit | PlayerBit;

        private static List<Entity.Entity> _entities;

        public static void SetEntities(List<Entity.Entity> entities)
        {
            _entities = entities;
        }

        public static void CheckCollision(Entity.Entity thisEntity, IBoxComponent thisBoxComp)
        {
            for (var i = 0; i < _entities.Count; i++)
            {
                if (_entities[i] is IBoxComponent otherBoxComp)
                {
                    var otherEntity = _entities[i];

                    if (otherBoxComp.GetBoundingBox() != thisBoxComp.GetBoundingBox())
                    {
                        if ((thisBoxComp.GetCategoryBits() & otherBoxComp.GetMaskBits()) != 0)
                        {
                            if (thisBoxComp.GetBoundingBox().Intersects(otherBoxComp.GetBoundingBox()))
                            {
                                thisBoxComp.OnCollision(otherEntity, otherBoxComp);
                                otherBoxComp.OnCollision(thisEntity, thisBoxComp);
                                //Console.WriteLine(thisBoxComp.GetCategoryBits() + " exists in " + otherBoxComp.GetMaskBits());                                
                            }
                        }
                    }
                }
            }
        }
    }
}