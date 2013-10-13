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

namespace Johnston_06_Particles.Input
{
    class GetInput
    {
        private KeyboardState oldKeyState;
        private GamePadState oldPadState;
        private PlayerIndex playerIndex;

        float movementSpeed = 10.0f;

        bool backPressed = false;
        bool actionPressed = false;
        bool upPressed = false;
        bool leftPressed = false;
        bool downPressed = false;
        bool rightPressed = false;
        bool[] directionBoolArray;
        bool isDirectionArrayEmpty = true;
        Vector2 directionPressed = new Vector2(0, 0);

        //Delegates
        public delegate void buttonPressDelegate();
        public delegate void directionBoolDelegate(bool[] directionArray);
        public delegate void directionPressDelegate(Vector2 value);

        //Events
        public event buttonPressDelegate event_actionPressed;
        public event buttonPressDelegate event_backPressed;
        public event directionBoolDelegate event_directionBoolPressed;

        public event directionPressDelegate event_directionPressed;

        public GetInput(PlayerIndex index)
        {
            playerIndex = index;
            directionBoolArray = new bool[4] { upPressed, leftPressed, downPressed, rightPressed };
            oldKeyState = Keyboard.GetState();
            oldPadState = GamePad.GetState(playerIndex);
        }

        public void update(GameTime gameTime)
        {
            backPressed = false;
            actionPressed = false;
            isDirectionArrayEmpty = true;

            for (int i = 0; i < 4; i++)
            {
                directionBoolArray[i] = false;
            }

            directionPressed = Vector2.Zero;

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

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    directionBoolArray[0] = true;
                    isDirectionArrayEmpty = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    directionBoolArray[1] = true;
                    isDirectionArrayEmpty = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    directionBoolArray[2] = true;
                    isDirectionArrayEmpty = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    directionBoolArray[3] = true;
                    isDirectionArrayEmpty = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                directionPressed.Y = -movementSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                directionPressed.Y = movementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                directionPressed.X = -movementSpeed;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                directionPressed.X = movementSpeed;
            }

            oldKeyState = Keyboard.GetState();

            #endregion

            #region xbox

            if (oldPadState != GamePad.GetState(playerIndex))
            {
                if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.Start))
                {
                    backPressed = true;
                }

                if (GamePad.GetState(playerIndex).IsButtonDown(Buttons.A))
                {
                    actionPressed = true;
                }

                if (GamePad.GetState(playerIndex).ThumbSticks.Left.Y < 0)
                {
                    directionBoolArray[0] = true;
                    isDirectionArrayEmpty = false;
                }
                if (GamePad.GetState(playerIndex).ThumbSticks.Left.X < 0)
                {
                    directionBoolArray[1] = true;
                    isDirectionArrayEmpty = false;
                }
                if (GamePad.GetState(playerIndex).ThumbSticks.Left.Y > 0)
                {
                    directionBoolArray[2] = true;
                    isDirectionArrayEmpty = false;
                }
                if (GamePad.GetState(playerIndex).ThumbSticks.Left.X > 0)
                {
                    directionBoolArray[3] = true;
                    isDirectionArrayEmpty = false;
                }
            }

            if (GamePad.GetState(playerIndex).ThumbSticks.Left.X != 0 && GamePad.GetState(playerIndex).ThumbSticks.Left.Y != 0)
            {
                directionPressed = GamePad.GetState(playerIndex).ThumbSticks.Left * movementSpeed;
            }

            oldPadState = GamePad.GetState(playerIndex);
            #endregion

            #region events

            if (actionPressed && event_actionPressed != null)
            {
                event_actionPressed();
            }
            if (backPressed && event_backPressed != null)
            {
                event_backPressed();
            }
            if (!isDirectionArrayEmpty && event_directionBoolPressed != null)
            {
                event_directionBoolPressed(directionBoolArray);
            }

            if (directionPressed != Vector2.Zero && event_directionPressed != null)
            {
                event_directionPressed(directionPressed);
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