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
using Batgai.AI_Logic;
using Batgai.Effects;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;
using xTile.Layers;

namespace Batgai.Entities
{
    class Punchee
    {
        public int mAcceleration = 8;

        public Vector2 mVelocity = new Vector2(0,0);
        public Vector2 mPosition = new Vector2(0,0);
        public Microsoft.Xna.Framework.Rectangle mCurrentRect = new Microsoft.Xna.Framework.Rectangle(0,0,0,0);
        protected Random random = new Random();

        public AnimatedSprite mSprite;
        protected AI mAI;
        protected ParticleEffect mEffect;

        protected int collisionCD = 0;

        public Punchee(Texture2D texture, ParticleEffect effect)
        {
            init();
            mSprite.setTexture(texture);
            mEffect = effect;
        }

        private void init()
        {
            mSprite = new AnimatedSprite();
            mSprite.setOrigin(new Vector2(mSprite.getSourceRect().Width / 2, mSprite.getSourceRect().Height / 2));
        }

        public void addAI(AI ai)
        {
            mAI = ai;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            mSprite.draw(spriteBatch);
        }

        public void drawEffect(SpriteBatch spriteBatch)
        {
            mEffect.draw(spriteBatch);
        }

        public void handleCollision(Vector2 collisionVect, SoundEffect playSound)
        {
            if (collisionCD <= 0)
            {
                mVelocity = collisionVect;
                collisionCD = 60;
                playSound.Play();
                mEffect.init();
            }
        }

        public virtual void update(GameTime gameTime, Map map)
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

            mVelocity *= 0.985f;

            Vector2 mNewPos = mPosition;
            mNewPos.X += (float)(mVelocity.X * gameTime.ElapsedGameTime.TotalSeconds);
            mNewPos.Y += (float)(mVelocity.Y * gameTime.ElapsedGameTime.TotalSeconds);

            checkCollision(ref mNewPos, map);
            mPosition = mNewPos;

            mSprite.setPosition(mPosition);

            mCurrentRect = new Microsoft.Xna.Framework.Rectangle((int)mPosition.X, (int)mPosition.Y,
                    (int)mSprite.getSourceRect().Width, (int)mSprite.getSourceRect().Height);

            mSprite.update(gameTime);
            mEffect.mOrigin = mSprite.getPosition();
            mEffect.update(gameTime);
        }

        private void checkCollision(ref Vector2 mNewPos, Map map)
        {
            Tile tile;
            Location tileLocation;
            Layer collision = map.Layers[0];

            tileLocation = new Location((int)(mNewPos.X - mCurrentRect.Width / 2) / 32,
                (int)(mNewPos.Y) / 32);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
            {
                mNewPos.X = mPosition.X;
            }

            tileLocation = new Location((int)(mNewPos.X + mCurrentRect.Width / 2) / 32,
                (int)(mNewPos.Y) / 32);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
            {
                mNewPos.X = mPosition.X;
            }

            tileLocation = new Location((int)(mNewPos.X) / 32,
                (int)(mNewPos.Y - mCurrentRect.Height / 2) / 32);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
            {
                mNewPos.Y = mPosition.Y;
            }

            tileLocation = new Location((int)(mNewPos.X) / 32,
                (int)(mNewPos.Y + mCurrentRect.Height / 2) / 32);
            tile = collision.Tiles[tileLocation];
            if (tile.TileIndex == 0)
            {
                mNewPos.Y = mPosition.Y;
            }

        }
    }
}
