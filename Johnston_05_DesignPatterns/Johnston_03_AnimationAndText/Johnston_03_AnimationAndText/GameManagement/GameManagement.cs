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

namespace Johnston_04_Depth.GameManagement
{
    public class GameManager
    {
        public enum GameStateType
        {
            START,
            MENU,
            PLAY,
            PAUSE,
            END
        }

        public enum GameLanguage
        {
            ENGLISH,
            SPANISH
        }

        private List<GameState> mGameStates;
        private GameStateType mCurrentState;
        public GameLanguage mCurrentLang;

        public Texture2D startScreen;
        public Texture2D playBG;
        public Texture2D heroSprites;
        public Texture2D star;
        public Texture2D doomBullets;

        public SpriteFont verdana;
        public SpriteFont segueUIMono;

        public GameManager()
        {
            mGameStates = new List<GameState>();
            mCurrentLang = GameLanguage.ENGLISH;
        }

        public GameStateType getCurrentState()
        {
            return mCurrentState;
        }

        public void changeStates(GameStateType stateType)
        {
            switch (stateType)
            {
                case GameStateType.START:
                    mCurrentState = GameStateType.START;
                    mGameStates.Clear();
                    mGameStates.Add(new StartState(this));
                    break;
                case GameStateType.MENU:
                    mCurrentState = GameStateType.MENU;
                    mGameStates.Clear();
                    mGameStates.Add(new MenuState(this));
                    break;
                case GameStateType.PLAY:
                    mCurrentState = GameStateType.PLAY;
                    mGameStates.Clear();
                    mGameStates.Add(new PlayState(this));
                    break;
                case GameStateType.PAUSE:
                    mCurrentState = GameStateType.PAUSE;
                    mGameStates.Add(new PauseState(this));
                    break;
                case GameStateType.END:
                    mCurrentState = GameStateType.END;
                    mGameStates.Clear();
                    mGameStates.Add(new EndState(this));
                    break;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < mGameStates.Count; i++)
            {
                mGameStates[i].draw(spriteBatch);
            }
        }

        public void initState()
        {
            mGameStates[mGameStates.Count-1].init();
        }

        public void load(ContentManager aContentManager)
        {
            startScreen = aContentManager.Load<Texture2D>("StartScreen");
            verdana = aContentManager.Load<SpriteFont>("Verdana");
            playBG = aContentManager.Load<Texture2D>("background");
            heroSprites = aContentManager.Load<Texture2D>("DoomWeapons");
            doomBullets = aContentManager.Load<Texture2D>("DoomAttacks");
            segueUIMono = aContentManager.Load<SpriteFont>("Segoe UI Mono");
            star = aContentManager.Load<Texture2D>("Star");

            startGame();
        }

        private void startGame()
        {
            changeStates(GameStateType.START);
        }

        public void unPause()
        {
            mGameStates.RemoveAt(mGameStates.Count-1);
            mGameStates[mGameStates.Count-1].unPause();
        }

        public void update(GameTime gameTime)
        {
            mGameStates[mGameStates.Count-1].update(gameTime);
        }
    }
}
