using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame32.Asset;
using MonoGame32.Collision;
using MonoGame32.Component;
using MonoGame32.Entity;
using MonoGame32.Input;

namespace MonoGame32.GameState
{
    public class MainMenuState : GameState
    {
        private Player player;
        private Player player2;

        public MainMenuState(Game1 game, SpriteBatch spriteBatch, AssetsManager assetsManager) : base(game, spriteBatch,
            assetsManager)
        {
            //Console.WriteLine("MainMenuState constructor");

            //Entities.Add(new Player(this, Vector2.Zero));
            //Entities.Add(new Terrain(this, new Vector2(64, 32)));
            //Entities.Add(new Terrain(this, new Vector2(80, 32)));
            Entities.Add(new Terrain(this, new Vector2(64, 16)));
            Entities.Add(new Terrain(this, new Vector2(8, 96)));
            Entities.Add(new Terrain(this, new Vector2(-8 + 96, 96)));
            Entities.Add(new Terrain(this, new Vector2(-8 + 112, 80)));
            Entities.Add(new Terrain(this, new Vector2(-8 + 112 + 32, 64)));
            Entities.Add(new Terrain(this, new Vector2(96, 0)));
            
            //floor
            //Entities.Add(new Terrain(this, new Vector2(8, 112)));
            //Entities.Add(new Terrain(this, new Vector2(208, 112)));
            
            for (int i = 8; i <= 224 * 2; i += 16)
                Entities.Add(new Terrain(this, new Vector2(i, 224+8)));
            
            // roof
            for (int i = 8; i <= 224 * 2; i += 16)
                Entities.Add(new Terrain(this, new Vector2(i, 8)));
            
            player = new Player(this, new Vector2(0, 200));
            Entities.Add(player);
            
            //player2 = new Player(this, new Vector2(96, 32));
            //Entities.Add(player2);

            /*int numOfEntities = 1; // 64k
            Random rand = new Random();
            for (int i = 1; i <= numOfEntities; i++)
            {
                float x = rand.Next(0, 195);
                float y = rand.Next(0, 100);
                Entities.Add(new Player(this, new Vector2(x, y)));
            }*/
        }

        public override void HandleInput(float dt)
        {
            InputProcessor.ReadInput();
            
            //Console.WriteLine("MainMenuState input");

            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) Game.ExitGame();*/
            
            if (InputProcessor.KeyExitIsDown) Game.ExitGame();

            if (Keyboard.GetState().IsKeyDown(Keys.F))
                GameSettings.GameSettings.SettingFullscreen = !GameSettings.GameSettings.SettingFullscreen;
            
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
                GameSettings.GameSettings.DrawBoundingBoxes = !GameSettings.GameSettings.DrawBoundingBoxes;

            player.HandleInput(dt);
        }
        
        public override void HandleCollisions()
        {
            CollisionSetup.SetEntities(Entities); // Not sure this is needed here. Or just in GameState constructor.

            // check collisions
            foreach (var entity in Entities)
                if (entity is IBoxComponent boxComponent)
                    CollisionSetup.CheckCollision(entity, boxComponent);
        }

        public override void Tick(float dt)
        {
            Console.WriteLine("NEW TICK");
            
            // Tick!
            foreach (var entity in Entities)
                entity.Tick(dt);
        }

        public override void Render(float dt)
        {
            //Console.WriteLine("MainMenuState render");
            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(x, y, 0));
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (var entity in Entities) entity.Render(dt);
            _spriteBatch.End();

            /*if (GameSettings.GameSettings.DrawBoundingBoxes)
            {
                BoundingBox currentBox;
                //Vector3 corner1, corner2, corner3, corner4;
                VertexPositionColor[] vertexPositionColors;
                
                //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                foreach (var entity in Entities)
                {
                    if (entity is IBoxComponent boxComponent)
                    {
                        currentBox = boxComponent.GetBoundingBox();

                        vertexPositionColors = new[]
                        {
                            new VertexPositionColor(new Vector3(currentBox.GetCorners()[0].X, currentBox.GetCorners()[0].Y, 0), Color.Red),
                            new VertexPositionColor(new Vector3(currentBox.GetCorners()[1].X, currentBox.GetCorners()[1].Y, 0), Color.Red),
                            new VertexPositionColor(new Vector3(0, 0, 0), Color.Black),
                            new VertexPositionColor(new Vector3(0, 0, 0), Color.Black)
                        };
                        
                        _game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertexPositionColors, 0, 4);
                        
                        //corner1 = box.GetCorners()[0]; // 8, 0-7 max.
                        //corner2 = box.GetCorners()[1];
                        //corner3 = box.GetCorners()[2];
                        //corner4 = box.GetCorners()[3];
                        
                        //_spriteBatch.Draw(texture, new Rectangle(corner1.X, corner1.Y, 16, 16), Color.Green);
                    }
                }
                //_spriteBatch.End();
            }*/
        }

        public override void ExitState()
        {
            base.ExitState(); // Should be called here.

            //Console.WriteLine("MainMenuState exit!");
        }
    }
}