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

namespace Johnston_04_Depth.Entities
{
    class Sprite
    {
        //Source Data
        protected Texture2D mTexture;
        protected Rectangle mCurrentCelLocation;

        //Destination Data
        protected Vector2 mPosition;
        protected Vector2 mViewPosition;
        protected Vector2 mScale;

        protected SpriteEffects mSpriteEffects = 0;
        protected float mRotation = 0;
        protected Vector2 mOrigin = Vector2.Zero;
        protected float mLayerDepth = 0;
        protected float mDepth = 0;
        protected Color mColor;

        public Sprite()
        {
            mScale = new Vector2(1);
            mColor = Color.White;
        }

        public Sprite(Rectangle currentCelLocation, Vector2 position)
        {
            mScale = new Vector2(1);
            mColor = Color.White;
            mPosition = position;
            mViewPosition = mPosition;
            mCurrentCelLocation = currentCelLocation;
        }

        public Sprite(Texture2D aTexture, Rectangle currentCelLocation, Vector2 position)
        {
            mScale = new Vector2(1);
            mColor = Color.White;
            mTexture = aTexture;
            mPosition = position;
            mViewPosition = mPosition;
            mCurrentCelLocation = currentCelLocation;
        }

        public Sprite(Texture2D aTexture, Rectangle currentCelLocation, Vector2 position, Vector2 scale)
        {
            mColor = Color.White;
            mScale = scale;
            mTexture = aTexture;
            mPosition = position;
            mViewPosition = mPosition;
            mCurrentCelLocation = currentCelLocation;
        }

        public Sprite(Texture2D aTexture, Rectangle currentCelLocation, Vector2 position, Vector2 scale, Vector2 origin)
        {
            mColor = Color.White;
            mScale = scale;
            mTexture = aTexture;
            mPosition = position;
            mViewPosition = mPosition;
            mCurrentCelLocation = currentCelLocation;
            mOrigin = origin;
        }

        public Sprite(Texture2D aTexture, Rectangle currentCelLocation, Vector2 position, Vector2 scale, Vector2 origin, float rotation)
        {
            mColor = Color.White;
            mScale = scale;
            mTexture = aTexture;
            mPosition = position;
            mViewPosition = mPosition;
            mCurrentCelLocation = currentCelLocation;
            mOrigin = origin;
            mRotation = rotation;
        }

        #region Getters
        public Color getColor()
        {
            return mColor;
        }

        public Vector2 getPosition()
        {
            return mPosition;
        }

        public Vector2 getViewPosition()
        {
            return mViewPosition;
        }

        public float getDepth()
        {
            return mDepth;
        }

        public float getRotation()
        {
            return mRotation;
        }

        public Vector2 getScale()
        {
            return mScale;
        }

        public Rectangle getSourceRect()
        {
            return mCurrentCelLocation;
        }

        public Texture2D getTexture()
        {
            return mTexture;
        }
        #endregion

        #region Setters
        public void setColor(Color color)
        {
            mColor = color;
        }

        public void setDepth(float depth)
        {
            mDepth = depth;
        }

        public void setPosition(Vector2 position)
        {
            mPosition = position;
        }

        public void setViewPosition(Vector2 viewPosition)
        {
            mViewPosition = viewPosition;
        }

        public void setOrigin(Vector2 origin)
        {
            mOrigin = origin;
        }

        public void setRotation(float newRotation)
        {
            mRotation = newRotation;
        }

        public void setScale(Vector2 scale)
        {
            mScale = scale;
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

        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, mCurrentCelLocation, mColor, mRotation, mOrigin, mScale, mSpriteEffects, mLayerDepth);
        }

        public virtual void draw(SpriteBatch spriteBatch, ViewOffset viewport)
        {
            mViewPosition.X = mPosition.X - viewport.getDimensions().X;
            //mViewPosition.X = mPosition.X * mScale.X;
            //mViewPosition.X = mPosition.X + (1280 / 2);
            mViewPosition.Y = mPosition.Y - viewport.getDimensions().Y;
            //.Y = mPosition.Y * mScale.Y;
            //mViewPosition.Y = mPosition.Y + (1280 / 2);
            spriteBatch.Draw(mTexture, mViewPosition, mCurrentCelLocation, mColor, mRotation, mOrigin, mScale, mSpriteEffects, mLayerDepth);
        }

        public virtual void update(GameTime gameTime) 
        {
            mViewPosition = mPosition;
        }
    }
}
