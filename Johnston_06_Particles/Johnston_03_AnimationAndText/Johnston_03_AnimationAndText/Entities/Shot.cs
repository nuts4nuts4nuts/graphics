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

namespace Johnston_06_Particles.Entities
{
    class Shot
    {
        private Sprite shotSprite;

        public Shot(Texture2D texture)
        {
            shotSprite.setTexture(texture);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            shotSprite.draw(spriteBatch);
        }
    }
}
