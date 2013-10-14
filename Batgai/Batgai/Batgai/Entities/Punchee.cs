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
    class Punchee
    {
        public Vector2 mVelocity = new Vector2(0,0);
        public Vector2 mPosition = new Vector2(0,0);
        public Rectangle mCurrentRect = new Rectangle(0,0,0,0);

        public AnimatedSprite mSprite;

        private int collisionCD = 0;

        public Punchee(Texture2D texture)
        {
            init();
            mSprite.setTexture(texture);
        }

        private void init()
        {
            mSprite = new AnimatedSprite();
            mSprite.setOrigin(new Vector2(mSprite.getSourceRect().Width / 2, mSprite.getSourceRect().Height / 2));
        }

        public void draw(SpriteBatch spriteBatch)
        {
            mSprite.draw(spriteBatch);
        }

        public void handleCollision(Vector2 collisionVect)
        {
            if (collisionCD <= 0)
            {
                mVelocity += collisionVect;
                collisionCD = 60;
            }
        }

        public void update(GameTime gameTime)
        {
            if (collisionCD > 0)
            {
                collisionCD--;
            }

            if (Math.Abs(mVelocity.X) > 100 || Math.Abs(mVelocity.Y) > 100)
            {
                mSprite.setRotation(mSprite.getRotation() + 0.1f);
            }

            mVelocity *= 0.985f;

            mPosition.Y += (float)(mVelocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            mPosition.X += (float)(mVelocity.X * gameTime.ElapsedGameTime.TotalSeconds);

            mSprite.setPosition(mPosition);

            mCurrentRect = new Rectangle((int)mPosition.X, (int)mPosition.Y,
                    (int)mSprite.getSourceRect().Width, (int)mSprite.getSourceRect().Height);

            mSprite.update(gameTime);
        }
    }
}
