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

namespace Batgai.GameManagement
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
        public String mLevelName = "NULL";

        public Game mGame;
        public GraphicsDevice mGraphicsDevice;

        //TEXTURES
        public Texture2D startScreen;
        public Texture2D heroSprites;
        public Texture2D shotSprite;
        public Texture2D particleEffect;

        //EFFECTS
        public Effect blur;
        public Effect ripple;

        //FONTS
        public SpriteFont verdana;
        public SpriteFont smallVerdana;
        public SpriteFont segueUIMono;

        //SOUNDS
        public SoundEffect wrangledSound;
        public SoundEffect deathSound;
        public SoundEffect fullChargeSound;
        public SoundEffect shootSound;
        public SoundEffect hitSound;
        public SoundEffect chargingSound;

        public GameManager(Game theGame)
        {
            mGame = theGame;
            mGraphicsDevice = theGame.GraphicsDevice;
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
            //TEXTURES
            startScreen = aContentManager.Load<Texture2D>("StartScreen");
            heroSprites = aContentManager.Load<Texture2D>("sonicSprites");
            shotSprite = aContentManager.Load<Texture2D>("closedFist");
            particleEffect = aContentManager.Load<Texture2D>("particleEffect");

            //FONTS
            verdana = aContentManager.Load<SpriteFont>("Verdana");
            smallVerdana = aContentManager.Load<SpriteFont>("Small Verdana");
            segueUIMono = aContentManager.Load<SpriteFont>("Segoe UI Mono");

            //EFFECTS
            blur = aContentManager.Load<Effect>("blur");
            ripple = aContentManager.Load<Effect>("ripple");

            //SOUNDS
            deathSound = aContentManager.Load<SoundEffect>("Death");
            fullChargeSound = aContentManager.Load<SoundEffect>("Full Charge");
            shootSound = aContentManager.Load<SoundEffect>("Shoot");
            hitSound = aContentManager.Load<SoundEffect>("Hit");
            wrangledSound = aContentManager.Load<SoundEffect>("Wrangled");
            chargingSound = aContentManager.Load<SoundEffect>("Charge");

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
