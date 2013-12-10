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
using Batgai.Effects;
using xTile;
using xTile.Dimensions;

namespace Batgai.Entities
{
    class CrazyPunchee : Punchee
    {
        private float sinusoidalY;
        private float sinusoidalX;

        private float mDirection = 1;
        private float counter;

        public CrazyPunchee(Texture2D texture, ParticleEffect effect)
            : base(texture, effect)
        {
            mSprite.setColor(Color.Aquamarine);
            mSprite.setScale(new Vector2(0.5f, 0.5f));
            mAcceleration = 20;
        }

        public override void update(GameTime gameTime, Map map)
        {
            if (collisionCD <= 0 && mAI.mHero.isAlive)
            {
                mAI.update(gameTime);
            }

            if (collisionCD > 0)
            {
                collisionCD--;
                Color tempTrans = new Color(mSprite.getColor().R, mSprite.getColor().G, mSprite.getColor().B, 0.75f);
                mSprite.setColor(tempTrans);
            }
            else
            {
                Color tempTrans = new Color(mSprite.getColor().R, mSprite.getColor().G, mSprite.getColor().B, 1.0f);
                mSprite.setColor(tempTrans);
            }

            if ((Math.Abs(mVelocity.X) > 30 || Math.Abs(mVelocity.Y) > 30) && collisionCD > 0)
            {
                int rotationDirection;

                if (random.Next(0, 1) == 0)
                {
                    rotationDirection = -1;
                }
                else
                {
                    rotationDirection = 1;
                }

                mSprite.setRotation(mSprite.getRotation() + (rotationDirection * ((mVelocity.X / 500) + (mVelocity.Y / 500)) / 2));
            }

            counter += ((float)gameTime.ElapsedGameTime.TotalSeconds * mDirection * mAcceleration);

            if (counter > Math.PI * 0.5f)
            {
                counter = (float)Math.PI * 0.5f;
                mDirection *= -1;
            }
            else if (counter < -(Math.PI * 0.5f))
            {
                counter = -(float)(Math.PI * 0.05f);
                mDirection *= -1;
            }

            sinusoidalY = (float)Math.Sin(counter);
            sinusoidalX = (float)Math.Cos(counter);

            mVelocity *= 0.985f;

            mPosition.Y += (float)(mVelocity.Y * gameTime.ElapsedGameTime.TotalSeconds) * sinusoidalY;
            mPosition.X += (float)(mVelocity.X * gameTime.ElapsedGameTime.TotalSeconds) * sinusoidalX;

            mSprite.setPosition(mPosition);

            mCurrentRect = new Microsoft.Xna.Framework.Rectangle((int)mPosition.X, (int)mPosition.Y,
                    (int)mSprite.getSourceRect().Width, (int)mSprite.getSourceRect().Height);

            mSprite.update(gameTime);
            mEffect.mOrigin = mSprite.getPosition();
            mEffect.update(gameTime);
        }
    }
}
