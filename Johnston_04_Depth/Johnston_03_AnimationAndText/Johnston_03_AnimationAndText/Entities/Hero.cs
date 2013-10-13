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
using Johnston_04_Depth.Input;

namespace Johnston_04_Depth.Entities
{
    class Hero : AnimatedSprite
    {
        protected Vector2 mVelocity;

        private const int FLOOR_Y = 720;
        private const int defaultCelX = 0;
        private const int defaultCelY = 878;
        private const int defaultCelWidth = 172;
        private const int defaultCelHeight = 86;

        private GetInput input;
        private bool animComplete = true;
        Sprite shootSprite;

        public Hero()
        {
            init();
        }

        public Hero(Texture2D heroTexture)
        {
            setTexture(heroTexture);
            init();
        }

        private void init()
        {
            input = new GetInput();

            mCurrentCelLocation.X = defaultCelX;
            mCurrentCelLocation.Y = defaultCelY;
            mCurrentCelLocation.Width = defaultCelWidth;
            mCurrentCelLocation.Height = defaultCelHeight;

            mCurrentCel = 0;
            mNumberOfCels = 3;
            msUntilNextCel = 0;
            msPerCel = 100;

            setScale(new Vector2(1));
            setPosition(new Vector2(640 - (mCurrentCelLocation.Width / 2), FLOOR_Y - defaultCelHeight));
            setViewPosition(mPosition);
        }

        public Vector2 getVelocity()
        {
            return mVelocity;
        }

        public void setVelocity(Vector2 velocity)
        {
            mVelocity = velocity;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            shootSprite.draw(spriteBatch);
        }

        public override void draw(SpriteBatch spriteBatch, ViewOffset viewport)
        {
            base.draw(spriteBatch, viewport);

            if (shootSprite != null)
            {
                shootSprite.draw(spriteBatch, viewport);
            }
        }

        private void shoot(GameTime gameTime)
        {
            base.update(gameTime);

            if (mCurrentCel == 0)
            {
                animComplete = true;
                shootSprite = null;
            }
            else if (mCurrentCel == 1)
            {
                shootSprite = new Sprite(getTexture(), new Rectangle(510, 900, 95, 50), new Vector2(mPosition.X + 50, mPosition.Y - 22), new Vector2(.75f));
            }
            else if (mCurrentCel == 2)
            {
                shootSprite = new Sprite(getTexture(), new Rectangle(603, 870, 150, 90), new Vector2(mPosition.X + 30, mPosition.Y - 38), new Vector2(.75f));
            }
        }

        public bool updateWithBool(GameTime gameTime)
        {
            float MAX_VELO = 1280 / 3.0f;

            mVelocity *= 0.94f;
            mVelocity.X = MathHelper.Clamp(mVelocity.X, -MAX_VELO, MAX_VELO);
            mVelocity.Y = MathHelper.Clamp(mVelocity.Y, -MAX_VELO, MAX_VELO);

            mPosition.Y += (float)(mVelocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            mPosition.X += (float)(mVelocity.X * gameTime.ElapsedGameTime.TotalSeconds);

            if (input.isKeyHit(Keys.Space) || input.isButtonPressed(Buttons.A))
            {
                animComplete = false;
                msUntilNextCel = 0;
                msPerCel = 90;
                return true;
            }
            else if (!animComplete)
            {
                shoot(gameTime);
            }

            mViewPosition = mPosition;
            return false;
        }
    }
}