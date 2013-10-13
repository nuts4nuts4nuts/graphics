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
        private PlayerIndex playerIndex;

        bool backPressed = false;
        bool actionPressed = false;
        bool wPressed = false;
        bool aPressed = false;
        bool sPressed = false;
        bool dPressed = false;
        Vector2 directionPressed = new Vector2(0, 0);
        Vector2 rightStickMoved = new Vector2(0, 0);
        Vector2 mouseMoved = new Vector2(0, 0);

        //Delegates
        public delegate void buttonPressDelegate();
        public delegate void directionPressDelegate(Vector2 value);
        public delegate void rightStickMovedDelegate(Vector2 value);
        public delegate void mouseMovedDelegate(Vector2 value);

        //Events
        public event buttonPressDelegate event_actionPressed;
        public event buttonPressDelegate event_backPressed;
        public event buttonPressDelegate event_wPressed;
        public event buttonPressDelegate event_aPressed;
        public event buttonPressDelegate event_sPressed;
        public event buttonPressDelegate event_dPressed;

        public event directionPressDelegate event_directionPressed;
        public event rightStickMovedDelegate event_rightStickMoved;
        public event mouseMovedDelegate event_mouseMoved;

        public void update(GameTime gameTime)
        {

            backPressed = false;
            actionPressed = false;
            wPressed = false;
            aPressed = false;
            sPressed = false;
            dPressed = false;
            directionPressed = Vector2.Zero;
            rightStickMoved = Vector2.Zero;
            mouseMoved = Vector2.Zero;

            #region keyboardInput

            if (oldKeyState != Keyboard.GetState())
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    backPressed = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    actionPressed = true;
                }
            }
    
            #endregion
        }

        /*
        #region oldInput
        public GetInput()
        {
            playerIndex = PlayerIndex.One;
        }

        public GetInput(PlayerIndex index)
        {
            playerIndex = index;
        }

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
            GamePadState newPadState = GamePad.GetState(playerIndex);

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
            GamePadState newPadState = GamePad.GetState(playerIndex);

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
            GamePadState padState = GamePad.GetState(playerIndex);

            if (padState.IsButtonDown(button))
            {
                return true;
            }

            return false;
        }

        #endregion
        */
    }
}