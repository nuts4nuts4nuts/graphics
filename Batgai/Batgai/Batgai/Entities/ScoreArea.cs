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
    class ScoreArea
    {
        public Vector2 mPosition;
        public Rectangle mRectangle;

        private int rectDimensions = 32;

        public ScoreArea(int xPos, int yPos)
        {
            init(xPos, yPos);
        }

        private void init(int xPos, int yPos)
        {
            mPosition = new Vector2(xPos, yPos);
            mRectangle = new Rectangle((int)mPosition.X, (int)mPosition.Y, rectDimensions, rectDimensions);
        }

        public void handleCollision(Vector2 collisionVect)
        {
            
        }

        public void update(GameTime gameTime)
        {
            
        }
    }
}
