using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Asset;
using MonoGame32.Collision;
using MonoGame32.Component;

namespace MonoGame32.GameState
{
    public abstract class GameState
    {
        protected Game1 Game;
        protected SpriteBatch _spriteBatch;
        protected AssetsManager _assetsManager;

        public AssetsManager AssetsManager => _assetsManager;

        public SpriteBatch SpriteBatch => _spriteBatch;

        protected List<Entity.Entity> Entities = new List<Entity.Entity>();

        protected GameState(Game1 game, SpriteBatch spriteBatch, AssetsManager assetsManager)
        {
            Game = game;
            _spriteBatch = spriteBatch;
            _assetsManager = assetsManager;
            
            CollisionSetup.SetEntities(Entities); // Not sure this is needed here. Or just in Tick.
        }

        public abstract void HandleCollisions();
        public abstract void HandleInput(float dt);
        public abstract void Tick(float dt);
        public abstract void Render(float dt);

        /// <summary>
        /// Use only to draw boundingBoxes when debugging. Shader usage is not so good I think.
        /// </summary>
        public void RenderBoundingBoxes()
        {
            if (!GameSettings.GameSettings.DrawBoundingBoxes) return;
            if (Entities.Count == 0) return;

            var basicEffect = new BasicEffect(Game.GraphicsDevice);
            basicEffect.World = Matrix.CreateOrthographicOffCenter(
                0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height, 0, 0, 1);

            var effectTechnique = basicEffect.Techniques[0];
            var effectPassCollection = effectTechnique.Passes;

            foreach (var entity in Entities)
                if (entity is IBoxComponent boxComponent)
                {
                    var currentBox = boxComponent.GetBoundingBox();
                    var vertexPositionColors = new[]
                    {
                        new VertexPositionColor(
                            new Vector3(currentBox.GetCorners()[3].X, currentBox.GetCorners()[3].Y, 0),
                            Color.White),
                        new VertexPositionColor(
                            new Vector3(currentBox.GetCorners()[0].X, currentBox.GetCorners()[0].Y, 0),
                            Color.White),
                        new VertexPositionColor(
                            new Vector3(currentBox.GetCorners()[1].X, currentBox.GetCorners()[1].Y, 0),
                            Color.White),
                        new VertexPositionColor(
                            new Vector3(currentBox.GetCorners()[2].X, currentBox.GetCorners()[2].Y, 0),
                            Color.White),
                        new VertexPositionColor(
                            new Vector3(currentBox.GetCorners()[3].X, currentBox.GetCorners()[3].Y, 0),
                            Color.White),
                    };

                    foreach (var pass in effectPassCollection)
                    {
                        pass.Apply();
                        Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertexPositionColors,
                            0, 4);
                    }
                }
        }

        public virtual void ExitState()
        {
            if (Entities.Count == 0) return;

            foreach (var entity in Entities)
            {
                entity.ToDestroy = true;
            }

            foreach (var entity in Entities)
            {
                entity.Destroy();
            }

            Entities.Clear();
        }
    }
}