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
        private bool isKeyPressed = true;
        private int currentlySelected = 0;
        private CurrentScreen currentScreen;

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
            currentScreen = CurrentScreen.PAUSE;
            pauseScreen = new Sprite(mManager.startScreen, new Rectangle(0, 0, pauseWidth, pauseHeight), new Rectangle(pausePosX, pausePosY, pauseWidth, pauseHeight));
            pauseFont = mManager.segueUIMono;
            pauseOptions = new List<string>();
            loadTextFile();
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
                    currentlySelected = pauseOptions.Count - 1;
                }

                isKeyPressed = true;
            }
            else if (keyboard.IsKeyDown(Keys.Down) && !isKeyPressed)
            {
                if (currentlySelected < pauseOptions.Count - 1)
                {
                    currentlySelected++;
                }
                else
                {
                    currentlySelected = 0;
                }

                isKeyPressed = true;
            }
            else if (keyboard.IsKeyDown(Keys.Escape) && !isKeyPressed)
            {
                currentScreen = CurrentScreen.PAUSE;
                loadTextFile();
            }
            else if(keyboard.IsKeyUp(Keys.Escape) && keyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyUp(Keys.Down))
            {
                isKeyPressed = false;
            }

            if (keyboard.IsKeyDown(Keys.Space) && currentScreen == CurrentScreen.PAUSE)
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
                    currentlySelected = pauseOptions.Count - 1;
                }

                isKeyPressed = true;
            }
            else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0 && !isKeyPressed)
            {
                if (currentlySelected < pauseOptions.Count - 1)
                {
                    currentlySelected++;
                }
                else
                {
                    currentlySelected = 0;
                }

                isKeyPressed = true;
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed && !isKeyPressed)
            {
                currentScreen = CurrentScreen.PAUSE;
                loadTextFile();
            }
            else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y == 0 && GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Released)
            {
                isKeyPressed = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && currentScreen == CurrentScreen.PAUSE)
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
#endif
            #endregion
        }
    }
}
