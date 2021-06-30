using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mmState = MonoGame32.GameState.MainMenuState;
using gState = MonoGame32.GameState;

namespace MonoGame32
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Stack<gState.GameState> _gameStates;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;
            IsMouseVisible = true;

            GameSettings.GameSettings.Init(this, _graphics);
            
            // To get gpu info!
            //GraphicsDeviceInformation g = new GraphicsDeviceInformation();
            //g.
        }

        protected override void Initialize()
        {
            // Bug in MonoGame v3.8: graphics need to be set here instead of in constructor. Will be fixed in v3.8.1.
            GameSettings.GameSettings.SettingMaxFps = 144; // TODO read from file and/or change in options.
            GameSettings.GameSettings.SettingFullscreen = false;
            GameSettings.GameSettings.WindowWidth = 1280;
            GameSettings.GameSettings.WindowHeight = 720;
            GameSettings.GameSettings.SettingMsaa = false;
            GameSettings.GameSettings.SettingUseVSync = false;
            GameSettings.GameSettings.SettingCapFpsToMaxFps = true;
            GameSettings.GameSettings.TimeBetweenFramesWhenCappedToMaxFps =
                TimeSpan.FromSeconds(1d / GameSettings.GameSettings.SettingMaxFps);

            GameSettings.GameSettings.ApplyNewWindowAndGraphicsSettings();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _gameStates = new Stack<gState.GameState>();
            AddGameState(new mmState.MainMenuState(this, _spriteBatch));
        }

        protected override void Update(GameTime gameTime)
        {
            GameMath.GameMath.CalculateDeltaTime(gameTime);
            GameMath.GameMath.CalculateFps();

            // TODO: Add your update logic here
            _gameStates.First().HandleInput(GameMath.GameMath.DeltaTime);
            _gameStates.First().Tick(GameMath.GameMath.DeltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _gameStates.First().Render(GameMath.GameMath.DeltaTime);

            Window.Title = "MonoGame32 | " + GameMath.GameMath.SmoothedFps + " FPS @ " +
                           GameMath.GameMath.DeltaTime +
                           "ms";

            base.Draw(gameTime);
        }

        public void ExitCurrentGameState()
        {
            if (_gameStates.Count != 0)
            {
                _gameStates.First().ExitState();
                _gameStates.Pop();
            }
            
            if (_gameStates.Count == 0)
                ExitGame();
        }

        public void AddGameState(gState.GameState gameState)
        {
            _gameStates.Push(gameState);
        }

        public void ExitGame()
        {
            Exit();
        }

        

        protected override void OnExiting(object sender, EventArgs args)
        {
            ExitCurrentGameState();
            _gameStates.Clear();
            
            base.OnExiting(sender, args);
        }
    }
}