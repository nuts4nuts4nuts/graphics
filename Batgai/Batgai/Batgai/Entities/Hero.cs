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

        public AnimatedSprite mSprite;

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
            mSprite.setTexture(heroTexture);
        }

        private void init()
        {
            mSprite = new AnimatedSprite();
            input = new GetInput(PlayerIndex.One);
            attachEventListener();

            mSprite.setSourceRect(new Rectangle(CelX, CelY, CelWidth, CelHeight));

            mSprite.mCurrentCel = 0;
            mSprite.mNumberOfCels = mCels;
            mSprite.msUntilNextCel = 100;
            mSprite.msPerCel = 100;

            mSprite.setScale(new Vector2(1.5f));
            mSprite.setOrigin(new Vector2(mSprite.getSourceRect().Width / 2, mSprite.getSourceRect().Height / 2));
            mPosition = new Vector2(640 - (mSprite.getSourceRect().Width / 2), 360 - (mSprite.getSourceRect().Height / 2));
            mVelocity = new Vector2(0);
        }

        private void attachEventListener()
        {
            input.event_directionPressed += new GetInput.directionPressDelegate(handleMovment);
            input.event_actionHeld += new GetInput.buttonHeldDelegate(slowMovement);
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
            mSprite.draw(spriteBatch);
        }

        private void handleMovment(Vector2 value)
        {
            mVelocity += value*5;

            mSprite.setRotation((float)Math.Atan2(value.Y, value.X) );

            if (mVelocity.X < 0)
            {
                mSprite.setSpriteEffects(SpriteEffects.FlipVertically);
            }
            else
            {
                mSprite.setSpriteEffects(SpriteEffects.None);
            }
        }

        private void slowMovement(int framesHeld)
        {
            mVelocity = new Vector2(mVelocity.X * 0.85f, mVelocity.Y * 0.85f);
        }

        public void update(GameTime gameTime)
        {
            input.update(gameTime);

            mVelocity *= 0.94f;

            mPosition.Y += (float)(mVelocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            mPosition.X += (float)(mVelocity.X * gameTime.ElapsedGameTime.TotalSeconds);

            mSprite.setPosition(mPosition);

            mSprite.update(gameTime);
        }
    }
}