using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.GameSettings
{
    public static class GameSettings
    {
        private static GraphicsDeviceManager _graphics;
        private static Game _game;

        private static bool _applyNewChanges;

        private static bool _drawBoundingBoxes;

        public static bool DrawBoundingBoxes
        {
            get => _drawBoundingBoxes;
            set => _drawBoundingBoxes = value;
        }

        private static bool _printRenderInformation;

        public static bool PrintRenderInformation
        {
            get => _printRenderInformation;
            set => _printRenderInformation = value;
        }
        
        private static bool _printCollisionsInformation;

        public static bool PrintCollisionsInformation
        {
            get => _printCollisionsInformation;
            set => _printCollisionsInformation = value;
        }

        public static void Init(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            _game = game;
            _graphics = graphicsDeviceManager;
        }

        private static int _settingTargetFps = 60; // 60 hertz is standard for monitors.

        public static int SettingTargetFps
        {
            get => _settingTargetFps;
            set => _settingTargetFps = value;
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

        private static int _windowWidth = 426, _windowHeight = 240; // Default values.

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
        private static int _renderScale = 1; // 4 Default value.

        public static int RenderScale
        {
            get => _renderScale;
            set => _renderScale = value;
        }

        private static bool _settingFullscreen;

        public static bool SettingFullscreen
        {
            get => _settingFullscreen;
            set
            {
                _settingFullscreen = value;
                //_graphics.IsFullScreen = _settingFullscreen;

                _applyNewChanges = true;
            }
        }

        /// <summary>
        /// Bool for switching from windowed -> fullscreen and fullscreen -> windowed.
        /// <br/><b>true</b>: Uses hardware to switch. Slower but better performance.
        /// <br/><b>false</b>: Faster but worse in performance.
        /// </summary>
        private static bool _settingHardwareModeSwitch = true;

        public static bool SettingHardwareModeSwitch
        {
            get => _settingHardwareModeSwitch;
            set
            {
                _settingHardwareModeSwitch = value;
                _graphics.HardwareModeSwitch = _settingHardwareModeSwitch;

                _applyNewChanges = true;
            }
        }

        private static bool _settingMsaa; // Multisampling.

        public static bool SettingMsaa
        {
            get => _settingMsaa;
            set
            {
                _settingMsaa = value;

                _applyNewChanges = true;
            }
        }

        private static bool _settingSyncUpsWithFps = true;

        /// <summary>
        /// Limits maximum UPS to maximum FPS.
        /// </summary>
        /// <example>
        /// <br/><b>The state of Vertical Sync controls the behaviour such as:</b>
        /// <br/><br/><b><see cref="SettingUseVSync"/> is active:<br/>false:</b> Limits FPS and UPS to <see cref="SettingTargetFps"/>.
        /// <br/><b>true:</b> No Limit on FPS and UPS.
        /// <br/><br/>
        /// <b><see cref="SettingUseVSync"/> is disabled:<br/>false:</b> Limits FPS to VSync's FPS target. Limits UPS to <see cref="SettingTargetFps"/>.
        /// <br/><b>true:</b> Limits FPS and UPS to VSync's FPS target.
        /// <seealso cref="SettingUseVSync"/>
        /// <seealso cref="SettingTargetFps"/>
        /// </example>
        public static bool SettingSyncUpsWithFps
        {
            get => _settingSyncUpsWithFps;
            set
            {
                _settingSyncUpsWithFps = !value;
                _game.IsFixedTimeStep = _settingSyncUpsWithFps;
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

                    _graphics.IsFullScreen = true;
                }
                else
                {
                    _graphics.PreferredBackBufferWidth = _windowWidth;
                    _graphics.PreferredBackBufferHeight = _windowHeight;

                    _graphics.IsFullScreen = false;
                }

                if (_settingUseVSync) _settingSyncUpsWithFps = false;

                _graphics.SynchronizeWithVerticalRetrace = _settingUseVSync;
                _graphics.PreferMultiSampling = _settingMsaa;

                _graphics.ApplyChanges();

                _applyNewChanges = false;
            }
        }
    }
}