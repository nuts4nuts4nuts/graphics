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
    class Sprite
    {
        //Source Data
        protected Texture2D mTexture;
        protected Rectangle mCurrentCelLocation;

        //Destination Data
        protected Rectangle mDestinationRect;
        protected SpriteEffects mSpriteEffects = 0;
        protected float mRotation = 0;
        protected Vector2 mOrigin = Vector2.Zero;
        protected float mLayerDepth = 0;

        public Sprite()
        {

        }

        public Sprite(Rectangle currentCelLocation, Rectangle destinationRect)
        {
            mDestinationRect = destinationRect;
            mCurrentCelLocation = currentCelLocation;
        }

        public Sprite(Texture2D aTexture, Rectangle currentCelLocation, Rectangle destinationRect)
        {
            mTexture = aTexture;
            mDestinationRect = destinationRect;
            mCurrentCelLocation = currentCelLocation;
        }

        #region Getters
        public Rectangle getDestinationRect()
        {
            return mDestinationRect;
        }

        public Rectangle getSourceRect()
        {
            return mCurrentCelLocation;
        }
        #endregion

        #region Setters
        public void setDestinationRect(Rectangle aRectangle)
        {
            mDestinationRect = aRectangle;
        }

        public void setOrigin(Vector2 origin)
        {
            mOrigin = origin;
        }

        public void setRotation(float newRotation)
        {
            mRotation = newRotation;
        }

        public void setSourceRect(Rectangle aRectangle)
        {
            mCurrentCelLocation = aRectangle;
        }

        public void setSpriteEffects(SpriteEffects spriteEffect)
        {
            mSpriteEffects = spriteEffect;
        }

        public void setTexture(Texture2D theTexture)
        {
            mTexture = theTexture;
        }
        #endregion

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mDestinationRect, mCurrentCelLocation, Color.White, mRotation, mOrigin, mSpriteEffects, mLayerDepth);
        }

        public virtual void update(GameTime gameTime) { }
    }
}
