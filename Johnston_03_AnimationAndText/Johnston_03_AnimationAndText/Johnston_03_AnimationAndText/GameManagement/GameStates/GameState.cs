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

namespace Johnston_03_AnimationAndText.GameManagement
{
    public class GameState
    {
        protected GameManager mManager;

        public GameState() { }
        public GameState(GameManager myManager)
        {
            mManager = myManager;
        }

        public virtual void init() { }
        public virtual void draw(SpriteBatch spriteBatch) { }
        public virtual void load(ContentManager contentManager) { }
        public virtual void unPause() { }
        public virtual void update(GameTime gameTime) { }
    }
}
