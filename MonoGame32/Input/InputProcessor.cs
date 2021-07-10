using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Input;

namespace MonoGame32.Input
{
    public static class InputProcessor
    {
        private static Keys _keyExit;

        private static bool keyExitIsDown;
        public static bool KeyExitIsDown => keyExitIsDown;

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

            _KeyMoveUp = Keys.W;
            _KeyMoveDown = Keys.S;
            _KeyMoveLeft = Keys.A;
            _KeyMoveRight = Keys.D;

            _KeyJump = Keys.Space;
        }

        public static void ReadInput()
        {
            keyExitIsDown = Keyboard.GetState().IsKeyDown(_keyExit);
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
    }
}