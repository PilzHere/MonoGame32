using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Input;
using KeyState = MonoGame32.Input.InputKeys.KeyState;

namespace MonoGame32.Input
{
    public static class InputProcessor
    {
        private static Keys _keyExit;
        private static KeyState _keyExitState;
        
        private static Keys _keyFullscreen;
        private static KeyState _keyFullscreenState;
        
        private static Keys _keyRenderBoundingBoxes;
        private static KeyState _keyRenderBoundingBoxesState;

        public static KeyState KeyRenderBoundingBoxesState => _keyRenderBoundingBoxesState;

        public static KeyState KeyFullscreenState => _keyFullscreenState;

        public static KeyState KeyExitState => _keyExitState;

        /*private static bool keyExitIsDown;
        private static bool keyExitWasDownLastFrame;
        private static bool keyExitIsUp;
        private static bool keyExitWasUpLastFrame;*/
        //public static bool KeyExitIsDown => keyExitIsDown;

        private static Keys _KeyMoveUp;

        private static bool keyMoveUpIsDown;
        public static bool KeyMoveUpIsDown => keyMoveUpIsDown;

        private static Keys _KeyMoveDown;

        private static bool keyMoveDownIsDown;
        public static bool KeyMoveDownIsDown => keyMoveDownIsDown;

        private static Keys _KeyMoveLeft;

        private static bool keyMoveLeftIsDown;
        public static bool KeyMoveLeftIsDown => keyMoveLeftIsDown;

        private static Keys _KeyMoveRight;

        private static bool keyMoveRightIsDown;
        public static bool KeyMoveRightIsDown => keyMoveRightIsDown;

        private static Keys _KeyJump;

        private static bool keyJumpIsDown;
        private static bool keyJumpWasDownLastFrame;
        private static bool keyJumpIsUp;
        private static bool keyJumpWasUpLastFrame;
        public static bool KeyJumpIsDown => keyJumpIsDown;
        public static bool KeyJumpWasDownLastFrame => keyJumpWasDownLastFrame;
        public static bool KeyJumpIsUp => keyJumpIsUp;
        public static bool KeyJumpWasUpLastFrame => keyJumpWasUpLastFrame;

        public static void SetupKeys()
        {
            // Default key setup.
            _keyExit = Keys.Escape;
            _keyFullscreen = Keys.F;
            _keyRenderBoundingBoxes = Keys.F3;

            _KeyMoveUp = Keys.W;
            _KeyMoveDown = Keys.S;
            _KeyMoveLeft = Keys.A;
            _KeyMoveRight = Keys.D;

            _KeyJump = Keys.Space;
        }

        public static void ReadInput()
        {
            //keyExitIsDown = Keyboard.GetState().IsKeyDown(_keyExit); // OLD
            _keyExitState = UpdateKey(_keyExitState, Keyboard.GetState().IsKeyUp(_keyExit),
                Keyboard.GetState().IsKeyDown(_keyExit));
            _keyFullscreenState = UpdateKey(_keyFullscreenState, Keyboard.GetState().IsKeyUp(_keyFullscreen),
                Keyboard.GetState().IsKeyDown(_keyFullscreen));
            _keyRenderBoundingBoxesState = UpdateKey(_keyRenderBoundingBoxesState, Keyboard.GetState().IsKeyUp(_keyRenderBoundingBoxes),
                Keyboard.GetState().IsKeyDown(_keyRenderBoundingBoxes));
            
            // TODO: Update rest of keys like the ones above?
            keyMoveUpIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveUp);
            keyMoveDownIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveDown);
            keyMoveLeftIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveLeft);
            keyMoveRightIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveRight);

            UpdateJumpKey();
        }

        private static void UpdateJumpKey()
        {
            keyJumpIsUp = Keyboard.GetState().IsKeyUp(_KeyJump);
            keyJumpIsDown = Keyboard.GetState().IsKeyDown(_KeyJump);

            if (keyJumpIsUp) // No press
            {
                keyJumpWasDownLastFrame = false;
                keyJumpWasUpLastFrame = true;
            }

            if (keyJumpIsDown) // Pressed down
            {
                if (keyJumpWasDownLastFrame)
                {
                    keyJumpWasDownLastFrame = false;
                }

                if (keyJumpWasUpLastFrame)
                {
                    keyJumpWasDownLastFrame = true;
                    keyJumpWasUpLastFrame = false;
                }
            }
        }

        private static KeyState UpdateKey(KeyState keyState, bool keyIsUp, bool keyIsDown)
        {
            if (keyIsUp) // No press
            {
                keyState = KeyState.Up;
                return keyState;
            }

            if (keyIsDown) // Pressed down
            {
                if (keyState == KeyState.DownOneFrame) // Key got pressed last frame
                {
                    keyState = KeyState.Down;
                    return keyState;
                }

                if (keyState == KeyState.Up) // Key was up last frame
                {
                    keyState = KeyState.DownOneFrame;
                    return keyState;
                }
            }
            
            // If this line is reached, keyState is Down.
            return keyState;
        }
    }
}