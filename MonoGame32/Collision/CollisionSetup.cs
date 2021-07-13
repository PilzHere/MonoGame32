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
        
        public static void CheckCollision(Entity.Entity thisEntity, IBoxComponent thisBoxComp)
        {
            // https://stackoverflow.com/questions/14479981/how-do-i-check-if-bitmask-contains-bit

            foreach (var entity in _entities)
            {
                if (!(entity is IBoxComponent otherBoxComp)) continue; // No box? NEXT!
                if (otherBoxComp.GetBoxComp().GetBox() == thisBoxComp.GetBoxComp().GetBox()) continue; // Not the same box.
                if ((otherBoxComp.GetBoxComp().MaskBits & thisBoxComp.GetBoxComp().CategoryBits) !=
                    thisBoxComp.GetBoxComp().CategoryBits) continue; // Categorybits exist in maskbits?
                if (thisBoxComp.GetBoxComp().GetBox().Intersects(otherBoxComp.GetBoxComp().GetBox())) // Intersection?
                {
                    if (GameSettings.GameSettings.PrintCollisionsInformation)
                        //Console.WriteLine(thisBoxComp.GetBoxComp().CategoryBits + " collides with: " + otherBoxComp.GetBoxComp().CategoryBits);
                        //Console.WriteLine(thisEntity.Id + " collides with: " + entity.Id);

                        otherBoxComp.GetBoxComp().intersection = GameMath.GameMath.GetIntersectionDepth(thisBoxComp.GetBoxComp(), otherBoxComp.GetBoxComp());
                        otherBoxComp.GetBoxComp().intersectionArea =
                            Math.Abs(otherBoxComp.GetBoxComp().intersection.X * otherBoxComp.GetBoxComp().intersection.Y);
                        thisBoxComp.GetBoxComp().overlapingBoxes.Add(otherBoxComp.GetBoxComp());
                        //thisBoxComp.OnCollision(entity, otherBoxComp); // Entity act on collision.
                }
                
                //thisBoxComp.OnCollision(entity, otherBoxComp); // Entity act on collision.
            }

            /*foreach (var entity in _entities)
            {
                if (!(entity is IBoxComponent otherBoxComp)) continue; // No box? NEXT!
                if (thisBoxComp.GetBoxComp().overlapingBoxes.Count == 0) continue;
                foreach (var otherBoxComp2 in thisBoxComp.GetBoxComp().overlapingBoxes)
                {
                    otherBoxComp2.intersection = GameMath.GameMath.GetIntersectionDepth(thisBoxComp.GetBoxComp(), otherBoxComp2);
                    otherBoxComp2.intersectionArea =
                        Math.Abs(otherBoxComp2.intersection.X * otherBoxComp2.intersection.Y);
                }
            }*/

            //Console.WriteLine("BEFORE");
            //thisBoxComp.GetBoxComp().overlapingBoxes.ForEach(comp => Console.WriteLine("intersection: " + comp.intersectionArea));
            //thisBoxComp.GetBoxComp().overlapingBoxes.Sort(new AComparer());
            //Console.WriteLine(string.Join(Environment.NewLine, thisBoxComp.GetBoxComp().overlapingBoxes));
            
            //Console.WriteLine("AFTER");
            thisBoxComp.GetBoxComp().overlapingBoxes.Sort((box1, box2) =>
                box2.intersectionArea.CompareTo(box1.intersectionArea));
            
            //Console.WriteLine("AFTER");
            //thisBoxComp.GetBoxComp().overlapingBoxes.ForEach(comp => Console.WriteLine("intersection: " + comp.intersectionArea));
            
            //Console.WriteLine(string.Join(Environment.NewLine, thisBoxComp.GetBoxComp().overlapingBoxes));
            
            //thisBoxComp.GetBoxComp().overlapingBoxes.OrderBy(comp => comp, new AComparer());
            //Array.Sort(thisBoxComp.GetBoxComp().overlapingBoxes, new AComparer());
            //thisBoxComp.GetBoxComp().overlapingBoxes.OrderBy(comp => comp, new AComparer());
            //Console.WriteLine("Count: " + thisBoxComp.GetBoxComp().overlapingBoxes.Count);
            
            foreach (var boxComp in thisBoxComp.GetBoxComp().overlapingBoxes)
            {
                //Console.WriteLine("Area: " + boxComp.intersectionArea);
                thisBoxComp.OnCollision(null, boxComp); // Entity act on collision.
            }

            //if (thisBoxComp.GetBoxComp().overlapingBoxes.Count != 0)
                //thisBoxComp.OnCollision(null, thisBoxComp.GetBoxComp().overlapingBoxes[0]);
            
            thisBoxComp.GetBoxComp().overlapingBoxes.Clear();
        }
    }
    
    public class AComparer : IComparer<BoxComp>
    {
        public int Compare(BoxComp boxA, BoxComp boxB)
        {
            /*var intersectionA = GameMath.GameMath.GetIntersectionDepth(thisBox, boxA);
            var intersectionVolumeA = Math.Abs(intersectionA.X * intersectionA.Y);
            
            var intersectionB = GameMath.GameMath.GetIntersectionDepth(thisBox, boxB);
            var intersectionVolumeB = Math.Abs(intersectionB.X * intersectionB.Y);*/

            //Console.WriteLine("BoxA: " + boxA.intersectionArea + " BoxB: " + boxB.intersectionArea);

            return boxB.intersectionArea.CompareTo(boxA.intersectionArea);
            
            //if (boxA.intersectionArea > boxB.intersectionArea) return -1;
            //return 1;
            //if (boxA.intersectionArea > boxB.intersectionArea) return 1;
            //return 0;
        }
    }
}