using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Johnston_04_Depth.Input
{
    class GetInput
    {
        private KeyboardState oldKeyState;
        private GamePadState oldPadState;

        public bool isKeyHit(Keys key)
        {
            KeyboardState newKeyState = Keyboard.GetState();

            if (oldKeyState.IsKeyUp(key) && newKeyState.IsKeyDown(key))
            {
                oldKeyState = newKeyState;
                return true;
            }
            else
            {
                oldKeyState = newKeyState;
                return false;
            }
        }

        public bool isButtonPressed(Buttons button)
        {
            GamePadState newPadState = GamePad.GetState(PlayerIndex.One);

            if (oldPadState.IsButtonUp(button) && newPadState.IsButtonDown(button))
            {
                oldPadState = newPadState;
                return true;
            }
            else
            {
                oldPadState = newPadState;
                return false;
            }
        }

        public bool isKeyReleased(Keys key)
        {
            KeyboardState newKeyState = Keyboard.GetState();

            if (oldKeyState.IsKeyDown(key) && newKeyState.IsKeyUp(key))
            {
                oldKeyState = newKeyState;
                return true;
            }
            else
            {
                oldKeyState = newKeyState;
                return false;
            }
        }

        public bool isButtonReleased(Buttons button)
        {
            GamePadState newPadState = GamePad.GetState(PlayerIndex.One);

            if (oldPadState.IsButtonDown(button) && newPadState.IsButtonUp(button))
            {
                oldPadState = newPadState;
                return true;
            }
            else
            {
                oldPadState = newPadState;
                return false;
            }
        }

        public bool isKeyHeld(Keys key)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(key))
            {
                return true;
            }

            return false;
        }

        public bool isButtonHeld(Buttons button)
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            if (padState.IsButtonDown(button))
            {
                return true;
            }

            return false;
        }
    }
}
