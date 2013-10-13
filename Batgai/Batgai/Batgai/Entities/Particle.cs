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

namespace Batgai.Entities
{
    public class Particle
    {
        public int mAge;
        public Vector2 mPos;
        public Vector2 mVel;
        public Vector2 mAcc;

        public float mDampening;
        public float mRotation;
        public float mRotVelocity;
        public float mRotDampening;

        public float mScale;
        public float mScaleVelo;
        public float mScaleAcc;
        public float mScaleMax;

        private Color mColor;
        public Color mInitColor;
        public Color mFinalColor;

        public int mFadeAge;

        public Sprite mSprite;

        public Particle()
        {
            mSprite = new Sprite();
        }

        public void create(Texture2D texture, int ageInMs, Vector2 pos, Vector2 vel, Vector2 acc, float damp, float rot, float rotVelo,
            float rotDamp, float scale, float scaleVelo, float scaleAcc, float maxScale, Color initColor, Color finalColor, int fadeAge)
        {
            mAge = ageInMs;
            mPos = pos;
            mVel = vel;
            mAcc = acc;
            mDampening = damp;

            mRotation = rot;
            mRotVelocity = rotVelo;
            mRotDampening = rotDamp;

            mScale = scale;
            mScaleVelo = scaleVelo;
            mScaleAcc = scaleAcc;
            mScaleMax = maxScale;

            mInitColor = initColor;
            mFinalColor = finalColor;
            mFadeAge = fadeAge;

            mSprite.setTexture(texture);
        }

        public void updateColor(GameTime gameTime)
        {
            if ((mAge > mFadeAge) && (mFadeAge != 0))
            {
                mColor = mInitColor;
            }
            else
            {
                float amtInit = (float)mAge / (float)mFadeAge;
                float amtFinal = 1.0f - amtInit;

                mColor.R = (byte)((amtInit * mInitColor.R) + (amtFinal * mFinalColor.R));
                mColor.G = (byte)((amtInit * mInitColor.G) + (amtFinal * mFinalColor.G));
                mColor.B = (byte)((amtInit * mInitColor.B) + (amtFinal * mFinalColor.B));
                mColor.A = (byte)((amtInit * mInitColor.A) + (amtFinal * mFinalColor.A));
            }

            mSprite.setColor(mColor);
        }

        public void updatePos(GameTime gameTime)
        {
            mVel *= mDampening;
            mVel += (mAcc * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mPos += (mVel * (float)gameTime.ElapsedGameTime.TotalSeconds);

            mSprite.setPosition(mPos);
        }

        public void updateRot(GameTime gameTime)
        {
            mRotation *= mRotDampening;
            mRotation += (mRotVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            mSprite.setRotation(mRotation);
        }

        public void updateScale(GameTime gameTime)
        {
            mScaleVelo += (mScaleAcc * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mScale += (mScaleVelo * (float)gameTime.ElapsedGameTime.TotalSeconds);
            mScale = MathHelper.Clamp(mScale, 0.0f, mScaleMax);

            mSprite.setScale(new Vector2(mScale));
        }

        public void update(GameTime gameTime)
        {
            if (mAge <= 0)
            {
                return;
            }

            mAge -= gameTime.ElapsedGameTime.Milliseconds;
            mSprite.update(gameTime);
            updatePos(gameTime);
            updateRot(gameTime);
            updateScale(gameTime);
            updateColor(gameTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (mAge <= 0)
            {
                return;
            }

            mSprite.draw(spriteBatch);
        }
    }
}
