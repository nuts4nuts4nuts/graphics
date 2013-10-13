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
    public class Camera
    {
        public Rectangle mDimensions;
        public Vector2 mVelo;

        public Camera()
        {
            mDimensions = new Rectangle(640, 360, 1280, 720);
            mVelo = new Vector2(0);
        }
    }
}
