using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame32.Asset;
using MonoGame32.Entity;

namespace MonoGame32.GameState
{
    public class MainMenuState : GameState
    {
        //private Player player;

        public MainMenuState(Game1 game, SpriteBatch spriteBatch, AssetsManager assetsManager) : base(game, spriteBatch,
            assetsManager)
        {
            //Console.WriteLine("MainMenuState constructor");

            //Entities.Add(new Player(this, Vector2.Zero));
            //player = new Player(this, new Vector2(16, 16));
            //Entities.Add(player);

            int numOfEntities = 100000; // 150000
            Random rand = new Random();
            for (int i = 1; i <= numOfEntities; i++)
            {
                float x = rand.Next(0, 195);
                float y = rand.Next(0, 100);
                Entities.Add(new Player(this, new Vector2(x, y)));                
            }
        }

        private float x = 16; // temp
        private float y = 16; // temp

        public override void HandleInput(float dt)
        {
            //Console.WriteLine("MainMenuState input");

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) _game.ExitGame();

            if (Keyboard.GetState().IsKeyDown(Keys.F))
                GameSettings.GameSettings.SettingFullscreen = !GameSettings.GameSettings.SettingFullscreen;

            /*if (Keyboard.GetState().IsKeyDown(Keys.W)) y -= 100 * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) y += 100 * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) x -= 100 * dt;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) x += 100* dt;*/
        }

        public override void Tick(float dt)
        {
            //Console.WriteLine("MainMenuState tick");
            //player.Position = new Vector2(x, y);

            foreach (var entity in Entities) entity.Tick(dt);

            //Console.WriteLine("X: "+ x +" Y: " + y);
        }

        public override void Render(float dt)
        {
            //Console.WriteLine("MainMenuState render");
            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Matrix.CreateTranslation(x, y, 0));
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            foreach (var entity in Entities) entity.Render(dt);
            _spriteBatch.End();
        }

        public override void ExitState()
        {
            base.ExitState(); // Should be called here.

            //Console.WriteLine("MainMenuState exit!");
        }
    }
}