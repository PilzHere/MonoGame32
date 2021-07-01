using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.GameSettings
{
    public static class GameSettings
    {
        private static GraphicsDeviceManager _graphics;
        private static Game _game;

        private static bool _applyNewChanges = false;

        public static void Init(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            _game = game;
            _graphics = graphicsDeviceManager;
        }

        private static int _settingMaxFps = 60; // 60 hertz is standard for monitors.

        public static int SettingMaxFps
        {
            get => _settingMaxFps;
            set => _settingMaxFps = value;
        }

        private static bool _settingUseVSync = true;

        public static bool SettingUseVSync
        {
            get => _settingUseVSync;
            set
            {
                _settingUseVSync = value;

                _applyNewChanges = true;
            }
        }

        private static int _windowWidth = 854, _windowHeight = 480; // Default values.

        public static int WindowWidth
        {
            get => _windowWidth;
            set
            {
                _windowWidth = value;
                //_graphics.PreferredBackBufferWidth = _windowWidth;
                
                _applyNewChanges = true;
            }
        }

        public static int WindowHeight
        {
            get => _windowHeight;
            set
            {
                _windowHeight = value;
                //_graphics.PreferredBackBufferHeight = _windowHeight;
                
                _applyNewChanges = true;
            }
        }
        
        private static int _fullscreenWindowWidth = 1920, _fullscreenWindowHeight = 1080; // Default values.

        public static int FullscreenWindowWidth
        {
            get => _fullscreenWindowWidth;
            set => _fullscreenWindowWidth = value;
        }

        public static int FullscreenWindowHeight
        {
            get => _fullscreenWindowHeight;
            set => _fullscreenWindowHeight = value;
        }

        // X screen pixels per fbo pixel.
        private static int _renderScale = 4; // 4 Default value.

        public static int RenderScale
        {
            get => _renderScale;
            set => _renderScale = value;
        }

        private static bool _settingFullscreen = false;

        public static bool SettingFullscreen
        {
            get => _settingFullscreen;
            set
            {
                _settingFullscreen = value;
                _graphics.IsFullScreen = _settingFullscreen;

                _applyNewChanges = true;
            }
        }

        private static bool _settingMsaa = false; // Multisampling.

        public static bool SettingMsaa
        {
            get => _settingMsaa;
            set
            {
                _settingMsaa = value;

                _applyNewChanges = true;
            }
        }

        private static bool _settingCapFpsToMaxFps = true;

        public static bool SettingCapFpsToMaxFps
        {
            get => _settingCapFpsToMaxFps;
            set
            {
                _settingCapFpsToMaxFps = value;
                _game.IsFixedTimeStep = _settingCapFpsToMaxFps;
            }
        }

        private static TimeSpan _timeBetweenFramesWhenCappedToMaxFps;

        public static TimeSpan TimeBetweenFramesWhenCappedToMaxFps
        {
            get => _timeBetweenFramesWhenCappedToMaxFps;
            set
            {
                _timeBetweenFramesWhenCappedToMaxFps = value;
                _game.TargetElapsedTime = _timeBetweenFramesWhenCappedToMaxFps;
            }
        }

        public static void ApplyNewWindowAndGraphicsSettings()
        {
            if (_applyNewChanges)
            {
                if (_settingFullscreen)
                {
                    _fullscreenWindowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    _fullscreenWindowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                    _graphics.PreferredBackBufferWidth = _fullscreenWindowWidth;
                    _graphics.PreferredBackBufferHeight = _fullscreenWindowHeight;
                }
                else
                {
                    _graphics.PreferredBackBufferWidth = _windowWidth;
                    _graphics.PreferredBackBufferHeight = _windowHeight;
                }
                
                _graphics.SynchronizeWithVerticalRetrace = _settingUseVSync;
                _graphics.PreferMultiSampling = _settingMsaa;

                _graphics.ApplyChanges();

                _applyNewChanges = false;
            }
        }
    }
}