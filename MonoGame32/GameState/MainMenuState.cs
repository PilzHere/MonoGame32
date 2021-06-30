using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame32.Entity;

namespace MonoGame32.GameState
{
    public class MainMenuState : GameState
    {
        public MainMenuState(Game1 game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            //Console.WriteLine("MainMenuState constructor");
            Entities.Add(new Player(this, Vector2.Zero));
        }

        public override void HandleInput(float dt)
        {
            //Console.WriteLine("MainMenuState input");

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ExitGame();
        }

        public override void Tick(float dt)
        {
            //Console.WriteLine("MainMenuState tick");
        }

        public override void Render(float dt)
        {
            //Console.WriteLine("MainMenuState render");
        }

        public override void ExitState()
        {
            base.ExitState(); // Should be called here.
            
            //Console.WriteLine("MainMenuState exit!");
        }
    }
}