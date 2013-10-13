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
using Batgai.Input;

namespace Batgai.Entities
{
    class Hero
    {
        public Vector2 mVelocity;
        public Vector2 mPosition;

        public AnimatedSprite mySprite;

        private int CelX = 220;
        private int CelY = 176;
        private int CelWidth = 36;
        private int CelHeight = 45;

        private int mCels = 4;

        private GetInput input;

        public Hero()
        {
            init();
        }

        public Hero(Texture2D heroTexture)
        {
            init();
            mySprite.setTexture(heroTexture);
        }

        private void init()
        {
            mySprite = new AnimatedSprite();
            input = new GetInput(PlayerIndex.One);
            attachEventListener();

            mySprite.setSourceRect(new Rectangle(CelX, CelY, CelWidth, CelHeight));

            mySprite.mCurrentCel = 0;
            mySprite.mNumberOfCels = mCels;
            mySprite.msUntilNextCel = 100;
            mySprite.msPerCel = 100;

            mySprite.setScale(new Vector2(1.5f));
            mPosition = new Vector2(640 - (mySprite.getSourceRect().Width / 2), 360 - (mySprite.getSourceRect().Height / 2));
            mVelocity = new Vector2(0);
        }

        private void attachEventListener()
        {
            input.event_directionPressed += new GetInput.directionPressDelegate(handleMovment);
        }

        public Vector2 getVelocity()
        {
            return mVelocity;
        }

        public void setVelocity(Vector2 velocity)
        {
            mVelocity = velocity;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            mySprite.draw(spriteBatch);
        }

        private void handleMovment(Vector2 value)
        {
            mVelocity += value*4;
            if (mVelocity.X < 0)
            {
                mySprite.setSpriteEffects(SpriteEffects.FlipHorizontally);

                if(mySprite.getRotation() < 90 && mySprite.getRotation() > -90)
                {
                    mySprite.setRotation(value.Y*5);
                }
            }
            else
            {
                mySprite.setSpriteEffects(SpriteEffects.None);

                if(mySprite.getRotation() < 90 && mySprite.getRotation() > -90)
                {
                    mySprite.setRotation(-value.Y*5);
                }
            }
        }

        public void update(GameTime gameTime)
        {
            float MAX_VELO = 1280 / 3.0f;

            mVelocity *= 0.94f;
            mVelocity.X = MathHelper.Clamp(mVelocity.X, -MAX_VELO, MAX_VELO);
            mVelocity.Y = MathHelper.Clamp(mVelocity.Y, -MAX_VELO, MAX_VELO);

            mPosition.Y += (float)(mVelocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            mPosition.X += (float)(mVelocity.X * gameTime.ElapsedGameTime.TotalSeconds);

            mySprite.setPosition(mPosition);
            mySprite.setViewPosition(mySprite.getPosition());

            mySprite.update(gameTime);
            input.update(gameTime);
        }
    }
}