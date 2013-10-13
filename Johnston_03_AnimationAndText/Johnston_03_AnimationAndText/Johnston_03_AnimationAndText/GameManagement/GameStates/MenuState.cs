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
using Johnston_03_AnimationAndText.Entities;

namespace Johnston_03_AnimationAndText.GameManagement
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

        private CurrentMenuScreen currentScreen;
        private Sprite menuScreen;
        private SpriteFont menuFont;
        private List<string> menuOptions;
        private int currentlySelected = 0;
        private bool isKeyPressed;

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
            currentScreen = CurrentMenuScreen.MAIN;
            menuScreen = new Sprite(mManager.startScreen, new Rectangle(0, 0, 1280, 720), new Rectangle(0, 0, 1280, 720));
            menuFont = mManager.segueUIMono;
            menuOptions = new List<string>();
            isKeyPressed = true;
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

        public override void update(GameTime gameTime)
        {
            #region WINDOWS
#if WINDOWS
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Up) && !isKeyPressed)
            {
                if (currentlySelected > 0)
                {
                    currentlySelected--;
                }
                else
                {
                    currentlySelected = menuOptions.Count - 1;
                }

                isKeyPressed = true;
            }
            else if (keyboard.IsKeyDown(Keys.Down) && !isKeyPressed)
            {
                if (currentlySelected < menuOptions.Count - 1)
                {
                    currentlySelected++;
                }
                else
                {
                    currentlySelected = 0;
                }

                isKeyPressed = true;
            }
            else if (keyboard.IsKeyDown(Keys.Space) && !isKeyPressed)
            {
                changeMenuOptions();
            }
            else if (keyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyUp(Keys.Space))
            {
                isKeyPressed = false;
            }

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                currentScreen = CurrentMenuScreen.MAIN;
                loadTextFile();
            }
#endif
            #endregion

            #region XBOX
#if XBOX
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0 && !isKeyPressed)
            {
                if (currentlySelected > 0)
                {
                    currentlySelected--;
                }
                else
                {
                    currentlySelected = menuOptions.Count - 1;
                }

                isKeyPressed = true;
            }
            else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0 && !isKeyPressed)
            {
                if (currentlySelected < menuOptions.Count - 1)
                {
                    currentlySelected++;
                }
                else
                {
                    currentlySelected = 0;
                }

                isKeyPressed = true;
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && !isKeyPressed)
            {
                changeMenuOptions();
            }
            else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y == 0 && GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released)
            {
                isKeyPressed = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                currentScreen = CurrentMenuScreen.MAIN;
                loadTextFile();
            }
#endif
            #endregion
        }
    }
}
