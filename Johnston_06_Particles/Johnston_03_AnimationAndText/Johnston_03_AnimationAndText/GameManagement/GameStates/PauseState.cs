using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Johnston_06_Particles.Entities;
using Johnston_06_Particles.Input;

namespace Johnston_06_Particles.GameManagement
{
    public class PauseState : GameState
    {
        enum CurrentScreen
        {
            PAUSE,
            CONTROLS
        }

        private Sprite pauseScreen;
        private SpriteFont pauseFont;
        private List<string> pauseOptions;
        private int currentlySelected = 0;
        private CurrentScreen currentScreen;
        private GetInput input;

        static private int pauseWidth = 800;
        static private int pauseHeight = 360;
        private int pausePosX = 640 - pauseWidth / 2;
        private int pausePosY = 360 - pauseHeight / 2;

        public PauseState()
        {
            init();
        }

        public PauseState(GameManager myManager)
            : base(myManager)
        {
            init();
        }

        public override void init()
        {
            input = new GetInput(PlayerIndex.One);
            attachEventListeners();
            currentScreen = CurrentScreen.PAUSE;
            pauseScreen = new Sprite(mManager.startScreen, new Rectangle(0, 0, pauseWidth, pauseHeight), new Vector2(pausePosX, pausePosY));
            pauseFont = mManager.segueUIMono;
            pauseOptions = new List<string>();
            loadTextFile();
        }

        private void attachEventListeners()
        {
            input.event_actionPressed += new GetInput.buttonPressDelegate(changeMenuOptions);
            input.event_backPressed += new GetInput.buttonPressDelegate(backToMain);
            input.event_directionBoolPressed += new GetInput.directionBoolDelegate(traverseMenu);
        }

        private void backToMain()
        {
            currentScreen = CurrentScreen.PAUSE;
            loadTextFile();
        }

        private void changeMenuOptions()
        {
            switch (currentlySelected)
            {
                case 0:
                    mManager.unPause();
                    break;
                case 1:
                    currentScreen = CurrentScreen.CONTROLS;
                    loadTextFile();
                    break;
                case 2:
                    mManager.changeStates(GameManager.GameStateType.MENU);
                    break;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            pauseScreen.draw(spriteBatch);
            for (int i = 0; i < pauseOptions.Count; i++)
            {
                Vector2 fontOrigin = pauseFont.MeasureString(pauseOptions[i]) / 2;

                if (i == currentlySelected)
                {
                    spriteBatch.DrawString(pauseFont, pauseOptions[i], new Vector2(640, (pausePosY + 100) + 50 * i), Color.White, 0, fontOrigin, (float)1.2, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(pauseFont, pauseOptions[i], new Vector2(640, (pausePosY + 100) + 50 * i), Color.Black, 0, fontOrigin, 1, SpriteEffects.None, 0);
                }
            }
        }

        public void loadTextFile()
        {
            pauseOptions.Clear();

            StreamReader sr;
            if (currentScreen == CurrentScreen.PAUSE)
            {
                if (mManager.mCurrentLang == GameManager.GameLanguage.ENGLISH)
                {
                    sr = new StreamReader("PauseEng.txt");
                }
                else if (mManager.mCurrentLang == GameManager.GameLanguage.SPANISH)
                {
                    sr = new StreamReader("PauseSpa.txt");
                }
                else
                {
                    sr = new StreamReader("PauseEng.txt");
                }
            }
            else if (currentScreen == CurrentScreen.CONTROLS)
            {
                if (mManager.mCurrentLang == GameManager.GameLanguage.ENGLISH)
                {
                    sr = new StreamReader("ControlsEng.txt");
                }
                else if (mManager.mCurrentLang == GameManager.GameLanguage.SPANISH)
                {
                    sr = new StreamReader("ControlsSpa.txt");
                }
                else
                {
                    sr = new StreamReader("ControlsEng.txt");
                }
            }
            else
            {
                sr = new StreamReader("PauseEng.txt");
            }

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                pauseOptions.Add(line);
            }
        }

        private void traverseMenu(bool[] directionArray)
        {
             if (directionArray[0])
             {
                if (currentlySelected > 0)
                {
                    currentlySelected--;
                }
                else
                {
                    currentlySelected = pauseOptions.Count - 1;
                }
            }
            else if (directionArray[2])
            {
                if (currentlySelected < pauseOptions.Count - 1)
                {
                    currentlySelected++;
                }
                else
                {
                    currentlySelected = 0;
                }
            }
        }

        public override void update(GameTime gameTime)
        {
            input.update(gameTime);
        }
    }
}
