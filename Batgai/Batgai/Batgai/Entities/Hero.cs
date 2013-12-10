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
using xTile;
using xTile.Dimensions;
using xTile.Tiles;
using xTile.Layers;
using Batgai.Input;

namespace Batgai.Entities
{
    class Hero
    {
        public bool isAlive{get; private set;}
        public Vector2 mVelocity;
        public Vector2 mPosition;
        public Microsoft.Xna.Framework.Rectangle mCurrentRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0);

        public AnimatedSprite mSprite;

        private int CelX = 220;
        private int CelY = 174;
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
            isAlive = true;
            mSprite = new AnimatedSprite();
            input = new GetInput(PlayerIndex.One);
            attachEventListener();

            mSprite.setSourceRect(new Microsoft.Xna.Framework.Rectangle(CelX, CelY, CelWidth, CelHeight));

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
            if (((Math.Abs(mVelocity.X) + Math.Abs(mVelocity.Y)) / 2) < 10)
            {
                mSprite.setSourceRect(new Microsoft.Xna.Framework.Rectangle(CelX, CelY, CelWidth, CelHeight));

                mSprite.mCurrentCel = 0;
                mSprite.mNumberOfCels = mCels;
            }

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

        public void killSelf()
        {
            isAlive = false;
        }

        private void slowMovement(int framesHeld)
        {
            mVelocity = new Vector2(mVelocity.X * 0.85f, mVelocity.Y * 0.85f);
        }

        public void update(GameTime gameTime, Map map)
        {
            input.update(gameTime);

            mVelocity *= 0.94f;

            Vector2 mNewPos = mPosition;
            mNewPos.X += (float)(mVelocity.X * gameTime.ElapsedGameTime.TotalSeconds);
            mNewPos.Y += (float)(mVelocity.Y * gameTime.ElapsedGameTime.TotalSeconds);

            checkCollision(ref mNewPos, map);
            mPosition = mNewPos;
            
            mSprite.setPosition(mPosition);

            if (((Math.Abs(mVelocity.X) + Math.Abs(mVelocity.Y)) / 2) < 10)
            {
                mSprite.setSourceRect(new Microsoft.Xna.Framework.Rectangle(CelX, 31, 25, 36));

                mSprite.mCurrentCel = 0;
                mSprite.mNumberOfCels = 1;
            }

            mCurrentRect = new Microsoft.Xna.Framework.Rectangle((int)mPosition.X, (int)mPosition.Y, mSprite.getSourceRect().Width, mSprite.getSourceRect().Height);

            mSprite.update(gameTime);
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