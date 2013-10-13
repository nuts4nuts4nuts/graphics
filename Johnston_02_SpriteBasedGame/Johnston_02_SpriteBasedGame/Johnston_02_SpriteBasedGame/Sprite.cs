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

namespace Johnston_02_SpriteBasedGame
{
    public class Sprite
    {
        //Source Data
        protected Texture2D mTexture;
        protected Rectangle mCurrentCelLocation;

        //Destination Data
        protected Rectangle mDestinationRect;
        protected SpriteEffects mSpriteEffects;
        protected float mRotation = 0;
        protected Vector2 mOrigin = Vector2.Zero;
        protected float mLayerDepth = 0;

        //Animation Data
        protected int mCurrentCel;
        protected int mNumberOfCels;

        protected int msUntilNextCel;
        protected int msPerCel;

        public Sprite()
        {

        }

        public Sprite(Rectangle destinationRect, Rectangle currentCelLocation)
        {
            mDestinationRect = destinationRect;
            mCurrentCelLocation = currentCelLocation;
        }

        public Rectangle getDestinationRect()
        {
            return mDestinationRect;
        }

        public void setTexture(Texture2D theTexture)
        {
            mTexture = theTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mDestinationRect, mCurrentCelLocation, Color.White, mRotation, mOrigin, mSpriteEffects, mLayerDepth);
        }

        public void update(GameTime gameTime)
        {
            msUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;

            if (msUntilNextCel <= 0)
            {
                mCurrentCel++;
                msUntilNextCel = msPerCel;
            }

            if (mCurrentCel >= mNumberOfCels)
            {
                mCurrentCel = 0;
            }

            mCurrentCelLocation.X = mCurrentCelLocation.Width * mCurrentCel;
        }

    }
}
