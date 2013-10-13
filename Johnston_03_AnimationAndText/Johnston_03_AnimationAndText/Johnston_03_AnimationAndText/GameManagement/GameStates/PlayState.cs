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
using Johnston_03_AnimationAndText.Entities;
using Johnston_03_AnimationAndText.Input;

namespace Johnston_03_AnimationAndText.GameManagement
{
    public class PlayState : GameState
    {
        private Hero tim;
        private Sprite background;
        private List<Sprite> villainList;

        public PlayState()
        {
            init();
        }

        public PlayState(GameManager myManager)
            : base(myManager)
        {
            init();
        }

        public override void init()
        {
            background = new Sprite(mManager.playBG, new Rectangle(0, 0, 1280, 720), new Rectangle(0, 0, 1280, 720));
            tim = new Hero(mManager.timSprites);

            villainList = new List<Sprite>();
            villainList.Add(new Snowman(mManager.someSprites));
            villainList.Add(new Greeter(mManager.someSprites));
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            background.draw(spriteBatch);
            for (int i = 0; i < villainList.Count; i++)
            {
                villainList[i].draw(spriteBatch);
            }
            tim.draw(spriteBatch);
        }

        public override void update(GameTime gameTime)
        {
            GetInput input = new GetInput();

            if(input.isKeyHit(Keys.Escape))
            {
                mManager.changeStates(GameManager.GameStateType.PAUSE);
            }

            if(input.isButtonPressed(Buttons.Start))
            {
                mManager.changeStates(GameManager.GameStateType.PAUSE);
            }

            tim.update(gameTime, villainList);
            for (int i = 0; i < villainList.Count; i++)
            {
                villainList[i].update(gameTime);
            }
        }
    }
}
