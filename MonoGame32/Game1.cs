using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame32.Asset;
using MonoGame32.PlatformSystem;
using gState = MonoGame32.GameState;

namespace MonoGame32
{
    /**
     * FAQ
     *  1.  Q: Linux - Can't see packed files from MGCB Editor in Content folder,
     *          only original file (I only see filename.png and not filename.xnb):
     *      A: The file is in Content/bin/... Rider show's as same folder in Windows,
     *          but it has same location as in Linux.
     *
     *  2.  Q: Linux - Can't open Monogame-pipeline-tool...
     *      A: BUG: There is a bug in the current pipeline-tool related to
     *          system-installed GTK and the GTK version in the program.
     *          It is fixed and will be released in release of MonoGame.
     *  3.  Q:
     * 
    */
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private AssetsManager _assetsManager;

        private RenderTarget2D _fbo;
        private Texture2D _fboTexture;

        private Stack<gState.GameState> _gameStates;

        public Game1()
        {
            Console.WriteLine("Running MonoGame32");

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;
            IsMouseVisible = true;

            GameSettings.GameSettings.Init(this, _graphics);
        }

        protected override void Initialize()
        {
            // BUG: MonoGame v3.8.0 - Graphics need to be set here instead of in constructor. Will be fixed in v3.8.1.
            GameSettings.GameSettings.SettingMaxFps = 144; // TODO: Read from file and/or change in options.
            GameSettings.GameSettings.SettingFullscreen = false;
            GameSettings.GameSettings.WindowWidth = 854;
            GameSettings.GameSettings.WindowHeight = 480;
            GameSettings.GameSettings.RenderScale = 4;
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
            PlatformSystem.SystemAnalyzer.Init(_graphics.GraphicsDevice);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _assetsManager = new AssetsManager(Content);
            _assetsManager.LoadAllAssets();

            _fbo = new RenderTarget2D(_graphics.GraphicsDevice,
                GameSettings.GameSettings.WindowWidth / GameSettings.GameSettings.RenderScale,
                GameSettings.GameSettings.WindowHeight / GameSettings.GameSettings.RenderScale, false,
                SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
            _fboTexture = _fbo;


            _gameStates = new Stack<gState.GameState>();
            AddGameState(new gState.MainMenuState(this, _spriteBatch, _assetsManager));
        }

        protected override void Update(GameTime gameTime)
        {
            SystemAnalyzer.PrintMemoryUsage();

            GameMath.GameMath.CalculateDeltaTime(gameTime);
            GameMath.GameMath.CalculateFps();

            _gameStates.First().HandleInput(GameMath.GameMath.DeltaTime);
            _gameStates.First().Tick(GameMath.GameMath.DeltaTime);

            base.Update(gameTime);
        }

        protected override bool BeginDraw()
        {
            GameSettings.GameSettings.ApplyNewWindowAndGraphicsSettings();
            
            return base.BeginDraw();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _graphics.GraphicsDevice.SetRenderTarget(_fbo); // Draw on Fbo.

            GraphicsDevice.Clear(Color.WhiteSmoke);
            _gameStates.First().Render(GameMath.GameMath.DeltaTime);

            _graphics.GraphicsDevice.SetRenderTarget(null); // Stop using Fbo.

            // Draw Fbo.
            _fboTexture = _fbo;
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            _spriteBatch.Draw(_fboTexture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero,
                new Vector2((float) _graphics.PreferredBackBufferWidth / _fboTexture.Width,
                    (float) _graphics.PreferredBackBufferHeight / _fboTexture.Height),
                SpriteEffects.None, 0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);

            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            Window.Title = "MonoGame32 | " + GameMath.GameMath.SmoothedFps + " FPS / " +
                           GameMath.GameMath.DeltaTime +
                           "ms | " + SystemAnalyzer.ProcessMemoryUsed + " / " + SystemAnalyzer.ProcessMemoryAllocated +
                           " MiB";
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

            _assetsManager.Dispose();

            _fboTexture.Dispose();
            _fbo.Dispose();

            _spriteBatch.Dispose();

            _graphics.Dispose();

            base.OnExiting(sender, args);
        }
    }
}