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
using Batgai.Entities;

namespace Batgai.AI_Logic
{
    class FollowAI : AI
    {
        public FollowAI(Punchee me, Hero theHero)
            : base(me, theHero)
        {

        }

        public override void update(GameTime gameTime)
        {
            Vector2 newDirection = new Vector2(mHero.mPosition.X - mSelf.mPosition.X, mHero.mPosition.Y - mSelf.mPosition.Y);
            newDirection.Normalize();

            newDirection = newDirection * mSelf.mAcceleration;

            mSelf.mVelocity += newDirection;
        }
    }
}
