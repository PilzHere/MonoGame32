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

        public static void SetupKeys()
        {
            // Default key setup.
            _keyExit = Keys.Escape;
            _KeyMoveUp = Keys.W;
            _KeyMoveDown = Keys.S;
            _KeyMoveLeft = Keys.A;
            _KeyMoveRight = Keys.D;
        }

        public static void ReadInput()
        {
            keyExitIsDown = Keyboard.GetState().IsKeyDown(_keyExit);
            keyMoveUpIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveUp);
            keyMoveDownIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveDown);
            keyMoveLeftIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveLeft);
            keyMoveRightIsDown = Keyboard.GetState().IsKeyDown(_KeyMoveRight);
        }
    }
}