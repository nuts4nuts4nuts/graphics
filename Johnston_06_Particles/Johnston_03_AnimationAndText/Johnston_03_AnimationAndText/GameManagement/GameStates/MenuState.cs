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
    public class MenuState : GameState
    {
        enum CurrentMenuScreen
        {
            MAIN,
            CONTROLS,
            CREDITS,
            LANGUAGE
        }

        private GetInput input;
        private CurrentMenuScreen currentScreen;
        private Sprite menuScreen;
        private SpriteFont menuFont;
        private List<string> menuOptions;
        private int currentlySelected = 0;

        public MenuState()
        {
            init();
        }

        public MenuState(GameManager myManager)
            : base(myManager)
        {
            init();
        }

        public override void init()
        {
            input = new GetInput(PlayerIndex.One);
            attachEventListeners();
            currentScreen = CurrentMenuScreen.MAIN;
            menuScreen = new Sprite(mManager.startScreen, new Rectangle(0, 0, 1280, 720), new Vector2(0));
            menuFont = mManager.segueUIMono;
            menuOptions = new List<string>();
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
            currentScreen = CurrentMenuScreen.MAIN;
            loadTextFile();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            menuScreen.draw(spriteBatch);
            for (int i = 0; i < menuOptions.Count; i++)
            {
                Vector2 fontOrigin = menuFont.MeasureString(menuOptions[i]) / 2;

                if (i == currentlySelected)
                {
                    spriteBatch.DrawString(menuFont, menuOptions[i], new Vector2(640, 90 + 135 * i), Color.White, 0, fontOrigin, (float)1.5, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(menuFont, menuOptions[i], new Vector2(640, 90 + 135 * i), Color.Black, 0, fontOrigin, 1, SpriteEffects.None, 0);
                }
            }
        }

        public void changeMenuOptions()
        {
            if (currentScreen == CurrentMenuScreen.MAIN)
            {
                switch (currentlySelected)
                {
                    case 0:
                        mManager.changeStates(GameManager.GameStateType.PLAY);
                        break;
                    case 1:
                        currentScreen = CurrentMenuScreen.CONTROLS;
                        break;
                    case 2:
                        currentScreen = CurrentMenuScreen.CREDITS;
                        break;
                    case 3:
                        currentScreen = CurrentMenuScreen.LANGUAGE;
                        break;
                    case 4:
                        mManager.changeStates(GameManager.GameStateType.END);
                        break;
                }
            }
            else if (currentScreen == CurrentMenuScreen.LANGUAGE)
            {
                switch (currentlySelected)
                {
                    case 0:
                        mManager.mCurrentLang = GameManager.GameLanguage.ENGLISH;      
                        break;
                    case 1:
                        mManager.mCurrentLang = GameManager.GameLanguage.SPANISH;
                        break;
                }
            }

            loadTextFile();
        }

        public void loadTextFile()
        {
            menuOptions.Clear();
            StreamReader sr;

            if (mManager.mCurrentLang == GameManager.GameLanguage.ENGLISH)
            {
                switch (currentScreen)
                {
                    case CurrentMenuScreen.MAIN:
                        sr = new StreamReader("MainMenuEng.txt");
                        break;
                    case CurrentMenuScreen.CREDITS:
                        sr = new StreamReader("CreditsEng.txt");
                        break;
                    case CurrentMenuScreen.CONTROLS:
                        sr = new StreamReader("ControlsEng.txt");
                        break;
                    case CurrentMenuScreen.LANGUAGE:
                        sr = new StreamReader("Language.txt");
                        break;
                    default:
                        sr = new StreamReader("MainMenuEng.txt");
                        break;
                }
            }
            else if (mManager.mCurrentLang == GameManager.GameLanguage.SPANISH)
            {
                switch (currentScreen)
                {
                    case CurrentMenuScreen.MAIN:
                        sr = new StreamReader("MainMenuSpa.txt");
                        break;
                    case CurrentMenuScreen.CREDITS:
                        sr = new StreamReader("CreditsSpa.txt");
                        break;
                    case CurrentMenuScreen.CONTROLS:
                        sr = new StreamReader("ControlsSpa.txt");
                        break;
                    case CurrentMenuScreen.LANGUAGE:
                        sr = new StreamReader("Language.txt");
                        break;
                    default:
                        sr = new StreamReader("MainMenuSpa.txt");
                        break;
                }
            }
            else
            {
                sr = new StreamReader("MainMenuEng.txt");
            }

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                menuOptions.Add(line);
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
                    currentlySelected = menuOptions.Count - 1;
                }
            }
            else if (directionArray[2])
            {
                if (currentlySelected < menuOptions.Count - 1)
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
