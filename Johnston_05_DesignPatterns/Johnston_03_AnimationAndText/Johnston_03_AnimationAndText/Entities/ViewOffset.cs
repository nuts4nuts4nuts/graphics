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
    class ViewOffset
    {
        private Vector2 mDimensions;

        public ViewOffset()
        {

        }

        public ViewOffset(Vector2 dimensions)
        {
            mDimensions = dimensions;
        }

        public Vector2 getDimensions()
        {
            return mDimensions;
        }

        public void setDimensions(Vector2 dimensions)
        {
            mDimensions = dimensions;
        }
    }
}
