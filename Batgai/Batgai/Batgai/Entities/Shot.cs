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
    class Shot
    {
        public const float SPEED = 25;
        public Vector2 SHOT_DIMENSIONS = new Vector2(296, 216);

        public Sprite mSprite;
        public Vector2 mVelocity = new Vector2(0,0);
        public Vector2 mPosition = new Vector2(0,0);
        public float mForce;
        public Rectangle mCurrentRect = new Rectangle(0,0,0,0);
        public int framesAlive = 0;

        public Shot(Texture2D texture)
        {
            mSprite = new Sprite();
            mSprite.setTexture(texture);
            init();
        }

        public void init()
        {
            mSprite.setSourceRect(new Rectangle(0, 0, (int)SHOT_DIMENSIONS.X, (int)SHOT_DIMENSIONS.Y));
            mSprite.setOrigin(new Vector2(mSprite.getSourceRect().Width / 10, mSprite.getSourceRect().Height / 2));
            Color moreTrans = mSprite.getColor();
            moreTrans.A = 200;
            mSprite.setColor(moreTrans);
            mVelocity = new Vector2((float)Math.Cos(mSprite.getRotation())*SPEED, (float)Math.Sin(mSprite.getRotation())*SPEED);
            mForce = mSprite.getScale().X * (1.5f*(float)Math.Pow(10, 2));
        }

        public void draw(SpriteBatch spriteBatch)
        {
            mSprite.draw(spriteBatch);
        }

        public void update(GameTime gameTime)
        {
            mPosition += mVelocity;
            mSprite.setPosition(mPosition);
            framesAlive++;

            mCurrentRect = new Rectangle((int)mPosition.X, (int)mPosition.Y,
                    (int)(SHOT_DIMENSIONS.X*mSprite.getScale().X), (int)(SHOT_DIMENSIONS.Y*mSprite.getScale().Y));

            mSprite.update(gameTime);
        }
    }
}
