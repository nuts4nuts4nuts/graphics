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
    class Greeter : AnimatedSprite
    {
        private const int FLOOR_Y = 645;

        public Greeter()
        {
            initIdleAnimation();
            mDestinationRect = new Rectangle(640, FLOOR_Y - mCurrentCelLocation.Height, mCurrentCelLocation.Width, mCurrentCelLocation.Height);
        }

        public Greeter(Texture2D texture)
        {
            setTexture(texture);
            initIdleAnimation();
            mDestinationRect = new Rectangle(640, FLOOR_Y - mCurrentCelLocation.Height, mCurrentCelLocation.Width, mCurrentCelLocation.Height);
        }

        public void initIdleAnimation()
        {
            mNumberOfCels = 6;
            mCurrentCel = 0;

            msPerCel = 130;
            msUntilNextCel = msPerCel;

            mCurrentCelLocation.X = 0;
            mCurrentCelLocation.Y = 0;
            mCurrentCelLocation.Width = 191;
            mCurrentCelLocation.Height = 183;

            mDestinationRect.Width = mCurrentCelLocation.Width;
            mDestinationRect.Height = mCurrentCelLocation.Height;
        }
    }
}
