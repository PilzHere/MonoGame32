using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame32.GameState
{
    public class MainMenuState : GameState
    {
        public MainMenuState(Game1 game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            //Console.WriteLine("MainMenuState constructor");
        }

        public override void HandleInput(float dt)
        {
            //Console.WriteLine("MainMenuState input");

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.ExitGame();
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
            Console.WriteLine("MainMenuState exit!");
        }
    }
}