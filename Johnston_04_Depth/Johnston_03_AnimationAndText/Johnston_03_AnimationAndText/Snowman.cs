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
    public class Snowman : Sprite
    {
        private const int FLOOR_Y = 645;

        public Snowman()
        {
            mCurrentCelLocation.X = 60;
            mCurrentCelLocation.Y = 341;
            mCurrentCelLocation.Width = 130;
            mCurrentCelLocation.Height = 182;

            mDestinationRect = new Rectangle(365, FLOOR_Y - (mCurrentCelLocation.Height - 15), mCurrentCelLocation.Width, mCurrentCelLocation.Height);
        }
    }
}
