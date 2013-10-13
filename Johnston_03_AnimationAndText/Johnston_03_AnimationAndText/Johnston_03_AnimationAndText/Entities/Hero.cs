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

namespace Johnston_03_AnimationAndText.Entities
{
    class Hero : AnimatedSprite
    {
        //Misc Data
        private const int FLOOR_Y = 645;
        private const int JUMP_FORCE = -20;

        private bool isRunning = false;
        private bool isGrounded = true;
        private bool isColliding = false;

        private int ySpeed;
        private int xSpeed;

        public Hero()
        {
            initIdleAnimation();
            mDestinationRect = new Rectangle(mCurrentCelLocation.Width, FLOOR_Y - mCurrentCelLocation.Height, mCurrentCelLocation.Width, mCurrentCelLocation.Height);
        }

        public Hero(Texture2D texture)
        {
            setTexture(texture);
            initIdleAnimation();
            mDestinationRect = new Rectangle(mCurrentCelLocation.Width, FLOOR_Y - mCurrentCelLocation.Height, mCurrentCelLocation.Width, mCurrentCelLocation.Height);
        }

        public void initIdleAnimation()
        {
            mNumberOfCels = 22;
            mCurrentCel = 0;

            msPerCel = 80;
            msUntilNextCel = msPerCel;

            mCurrentCelLocation.X = 0;
            mCurrentCelLocation.Y = 140;
            mCurrentCelLocation.Width = 82;
            mCurrentCelLocation.Height = 140;

            mDestinationRect.Width = mCurrentCelLocation.Width;
            mDestinationRect.Height = mCurrentCelLocation.Height;
        }

        public void initRunAnimation()
        {
            mNumberOfCels = 27;
            mCurrentCel = 0;

            msPerCel = 30;
            msUntilNextCel = msPerCel;

            mCurrentCelLocation.X = 0;
            mCurrentCelLocation.Y = 0;
            mCurrentCelLocation.Width = 122;
            mCurrentCelLocation.Height = 140;

            mDestinationRect.Width = mCurrentCelLocation.Width;
            mDestinationRect.Height = mCurrentCelLocation.Height;
        }

        public void checkCollisions(List<Sprite> villainList)
        {
            for (int i = 0; i < villainList.Count(); i++)
            {
                if (mDestinationRect.Intersects(villainList[i].getDestinationRect()))
                {
                    if (mDestinationRect.Y + (mDestinationRect.Height - 28) < villainList[i].getDestinationRect().Y)
                    {
                        isGrounded = true;
                        ySpeed = 0;
                    }

                    isColliding = true;
                }
            }

            if (!isColliding && mDestinationRect.Y + mDestinationRect.Height < FLOOR_Y)
            {
                isGrounded = false;
            }

            isColliding = false;
        }

        public void move()
        {
            KeyboardState keyboard;
            keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Right) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
            {
                if (!isRunning)
                {
                    initRunAnimation();
                    isRunning = true;
                    mSpriteEffects = SpriteEffects.None;
                }
                xSpeed = 5;
            }
            else if (keyboard.IsKeyDown(Keys.Left) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
            {
                if (!isRunning)
                {
                    initRunAnimation();
                    isRunning = true;
                    mSpriteEffects = SpriteEffects.FlipHorizontally;
                }
                xSpeed = -5;
            }
            else if ((keyboard.IsKeyDown(Keys.Left) && keyboard.IsKeyDown(Keys.Right)) || !keyboard.IsKeyDown(Keys.Left) || !keyboard.IsKeyDown(Keys.Right))
            {
                if (isRunning)
                {
                    initIdleAnimation();
                    isRunning = false;
                }
                xSpeed = 0;
            }

            if (isGrounded)
            {
                if (keyboard.IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    ySpeed = JUMP_FORCE;
                    isGrounded = false;
                }
            }

            if (!isGrounded)
            {
                ySpeed++;
            }

            mDestinationRect.Y += ySpeed;
            mDestinationRect.X += xSpeed;

            if (mDestinationRect.Y > FLOOR_Y - mCurrentCelLocation.Height)
            {
                mDestinationRect.Y = FLOOR_Y - mCurrentCelLocation.Height;
                ySpeed = 0;
                isGrounded = true;
            }
        }

        public void update(GameTime gameTime, List<Sprite> villainList)
        {
            base.update(gameTime);
            move();
            checkCollisions(villainList);
        }

    }
}
