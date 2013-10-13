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
using Johnston_06_Particles.Entities;

namespace Johnston_06_Particles.GameManagement
{
    public class StartState : GameState
    {
        private SpriteFont titleFont;
        private Sprite startScreen;
        private string title = "Particles";

        public StartState()
        {
            init();
        }

        public StartState(GameManager myManager)
            : base(myManager)
        {
            init();
        }

        public override void init()
        {
            startScreen = new Sprite(mManager.startScreen, new Rectangle(0, 0, 1280, 720), new Vector2(0));
            titleFont = mManager.verdana;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Vector2 fontOrigin = titleFont.MeasureString(title) / 2;
            startScreen.draw(spriteBatch);
            spriteBatch.DrawString(titleFont, title, new Vector2(640, 360), Color.Black, 0, fontOrigin, 1, SpriteEffects.None, 0);
        }

        public override void update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Seconds == 3)
            {
                mManager.changeStates(GameManager.GameStateType.MENU);
            }
        }
    }
}
