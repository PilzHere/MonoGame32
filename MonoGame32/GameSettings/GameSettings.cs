using System;
using Microsoft.Xna.Framework;

namespace MonoGame32.GameSettings
{
    public static class GameSettings
    {
        private static GraphicsDeviceManager _graphics;
        private static Game _game;

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
                _graphics.SynchronizeWithVerticalRetrace = _settingUseVSync;
            }
        }

        private static int _windowWidth = 1280, _windowHeight = 720; // Default values.

        public static int WindowWidth
        {
            get => _windowWidth;
            set
            {
                _windowWidth = value;
                _graphics.PreferredBackBufferWidth = _windowWidth;
            }
        }

        public static int WindowHeight
        {
            get => _windowHeight;
            set
            {
                _windowHeight = value;
                _graphics.PreferredBackBufferHeight = _windowHeight;
            }
        }

        private static bool _settingFullscreen = false;

        public static bool SettingFullscreen
        {
            get => _settingFullscreen;
            set
            {
                _settingFullscreen = value;
                _graphics.IsFullScreen = _settingFullscreen;
            }
        }

        private static bool _settingMsaa = false; // Multisampling.

        public static bool SettingMsaa
        {
            get => _settingMsaa;
            set => _settingMsaa = value;
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
            _graphics.ApplyChanges();
        }
    }
}