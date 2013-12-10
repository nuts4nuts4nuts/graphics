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

namespace Batgai.Input
{
    class GetInput
    {
        private KeyboardState oldKeyState;
        private GamePadState oldPadState;
        private PlayerIndex playerIndex;

        float movementSpeed = 10.0f;

        bool backPressed = false;
        bool actionPressed = false;
        bool actionReleased = false;
        int actionFramesHeld = 0;

        bool upPressed = false;
        bool leftPressed = false;
        bool downPressed = false;
        bool rightPressed = false;
        bool[] directionBoolArray;
        bool isDirectionArrayEmpty = true;
        Vector2 directionPressed = new Vector2(0, 0);

        //Delegates
        public delegate void buttonPressDelegate();
        public delegate void buttonHeldDelegate(int framesHeld);
        public delegate void directionBoolDelegate(bool[] directionArray);
        public delegate void directionPressDelegate(Vector2 value);

        //Events
        public event buttonPressDelegate event_actionPressed;
        public event buttonHeldDelegate event_actionHeld;
        public event buttonPressDelegate event_actionReleased;

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
            actionReleased = false;
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

            if (actionFramesHeld > 0 && Keyboard.GetState().IsKeyUp(Keys.Space) && GamePad.GetState(playerIndex).IsButtonUp(Buttons.A))
            {
                actionReleased = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(playerIndex).IsButtonDown(Buttons.A)) //sad but necessary
            {
                actionFramesHeld++;
            }
            else
            {
                actionFramesHeld = 0;
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

                if (GamePad.GetState(playerIndex).DPad.Up == ButtonState.Pressed)
                {
                    directionBoolArray[0] = true;
                    isDirectionArrayEmpty = false;
                }
                if (GamePad.GetState(playerIndex).DPad.Left == ButtonState.Pressed)
                {
                    directionBoolArray[1] = true;
                    isDirectionArrayEmpty = false;
                }
                if (GamePad.GetState(playerIndex).DPad.Down == ButtonState.Pressed)
                {
                    directionBoolArray[2] = true;
                    isDirectionArrayEmpty = false;
                }
                if (GamePad.GetState(playerIndex).DPad.Right == ButtonState.Pressed)
                {
                    directionBoolArray[3] = true;
                    isDirectionArrayEmpty = false;
                }
            }

            if (actionFramesHeld > 0 && GamePad.GetState(playerIndex).IsButtonUp(Buttons.A) && Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                actionReleased = true;
            }

            if (GamePad.GetState(playerIndex).ThumbSticks.Left.X != 0 || GamePad.GetState(playerIndex).ThumbSticks.Left.Y != 0)
            {
                directionPressed.X = GamePad.GetState(playerIndex).ThumbSticks.Left.X * movementSpeed;
                directionPressed.Y = -GamePad.GetState(playerIndex).ThumbSticks.Left.Y * movementSpeed;
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

            if (actionReleased && event_actionReleased != null)
            {
                event_actionReleased();
            }
            if (actionFramesHeld > 0 && event_actionHeld != null)
            {
                event_actionHeld(actionFramesHeld);
            }
            #endregion
        }
    }
}