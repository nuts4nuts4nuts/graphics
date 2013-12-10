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
using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Tiles;
using xTile.Layers;
using Batgai.Entities;
using Batgai.Input;
using Batgai.Effects;
using Batgai.AI_Logic;

namespace Batgai.GameManagement
{
    public class PlayState : GameState
    {
        private const int LEFTBOUND = 448;
        private const int RIGHTBOUND = 832;
        private const int TOPBOUND = 252;
        private const int BOTTOMBOUND = 468;

        private GraphicsDevice mDevice;

        //Tile map stuff
        private Map map;
        private IDisplayDevice mapDisplayDevice;
        xTile.Dimensions.Rectangle viewport;

        private Hero player;
        private List<Shot> shotList;
        private int chargeFrames;
        private bool isSpacePressed = true;
        private bool chargedThisFrame;
        private SoundEffectInstance chargingSoundEffect;

        private List<Punchee> puncheeList;
        private List<ScoreArea> scoreAreaList;

        private Camera camera;
        private GetInput input;
        private ParticleEffect runEffect;
        private ParticleEffect explosion;
        private ScoreCounter scoreCounter;

        private RenderTargetBinding[] tempBinding;
        private RenderTarget2D tempRenderTarget;

        private BlurFocusShader blurShader;
        //private RippleShader rippleShader;

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
            //Shader stuff
            mDevice = mManager.mGraphicsDevice;
            tempBinding = mDevice.GetRenderTargets();
            tempRenderTarget = new RenderTarget2D(mDevice, 1280, 720);

            blurShader = new BlurFocusShader(mDevice, tempRenderTarget, mManager.blur);
            //rippleShader = new RippleShader(mDevice, tempRenderTarget, mManager.ripple);
            //Shader stuff ends

            //Map stuff
            map = mManager.mGame.Content.Load<Map>("maps\\" + mManager.mLevelName);
            mapDisplayDevice = new XnaDisplayDevice(mManager.mGame.Content, mDevice);
            map.LoadTileSheets(mapDisplayDevice);
            viewport = new xTile.Dimensions.Rectangle(new Size(1280, 720));
            //Map stuff done

            scoreCounter = new ScoreCounter();
            camera = new Camera();
            runEffect = new ParticleEffect(mManager.particleEffect, ParticleEffect.ParticleType.Dust, BlendState.AlphaBlend, camera);
            explosion = new ParticleEffect(mManager.particleEffect, ParticleEffect.ParticleType.Explosion, BlendState.Additive, camera);
            input = new GetInput(PlayerIndex.One);
            player = new Hero(mManager.heroSprites);
            shotList = new List<Shot>();
            puncheeList = new List<Punchee>();
            scoreAreaList = new List<ScoreArea>();
            chargingSoundEffect = mManager.chargingSound.CreateInstance();
            chargingSoundEffect.Pitch = 0.2f;

            attachEventListener();

            for (int i = 0; i < map.Layers[0].LayerWidth; i++)
            {
                for (int j = 0; j < map.Layers[0].LayerHeight; j++)
                {
                    Layer collision = map.Layers[0];
                    Location tileLocation = new Location(i, j);
                    Tile tile = collision.Tiles[tileLocation];

                    if (tile.TileIndex == 90)
                    {
                        ParticleEffect trail = new ParticleEffect(mManager.particleEffect, ParticleEffect.ParticleType.Knockback, BlendState.AlphaBlend, camera);
                        Punchee punched = new Punchee(mManager.startScreen, trail);
                        FollowAI followAI = new FollowAI(punched, player);
                        punched.addAI(followAI);
                        punched.mPosition.X = 32 * i;
                        punched.mPosition.Y = 32 * j;
                        puncheeList.Add(punched);
                    }

                    if (tile.TileIndex == 77)
                    {
                        int posX = i * 32;
                        int posY = j * 32;

                        ScoreArea scoreArea = new ScoreArea(posX, posY);
                        scoreAreaList.Add(scoreArea);
                    }
                }
            }
        }

        private void charge(int numFrames)
        {
            if(numFrames == 120)
            {
                mManager.fullChargeSound.Play();
                chargeFrames = numFrames;
            }
            else if (numFrames <= 120)
            {
                chargeFrames = numFrames;
            }

            chargedThisFrame = true;

            if (chargingSoundEffect.State != SoundState.Playing)
            {
                chargingSoundEffect.Play();
                
                if (numFrames <= 120)
                {
                    chargingSoundEffect.Pitch += 0.05f;
                }
            }
        }

        private void attachEventListener()
        {
            input.event_backPressed += new GetInput.buttonPressDelegate(pause);
            input.event_actionHeld += new GetInput.buttonHeldDelegate(charge);
            input.event_actionReleased += new GetInput.buttonPressDelegate(shoot);
            input.event_directionPressed += new GetInput.directionPressDelegate(handleMovement);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            blurShader.initDraw();
            //rippleShader.initDraw();
            blurShader.bindShader(tempBinding);

            mDevice.Clear(Color.Green);

            if (player.isAlive)
            {
                runEffect.draw(spriteBatch);
            }

            explosion.draw(spriteBatch);

            for (int i = 0; i < puncheeList.Count; i++)
            {
                puncheeList[i].drawEffect(spriteBatch);
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,
                camera.getTransform(mDevice));

            for (int i = 0; i < shotList.Count; i++)
            {
                shotList[i].draw(spriteBatch);
            }

            if (player.isAlive)
            {
                player.draw(spriteBatch);
            }

            for (int i = 0; i < puncheeList.Count; i++)
            {
                puncheeList[i].draw(spriteBatch);
            }

            
            spriteBatch.End();

            //Map stuff
            map.Draw(mapDisplayDevice, viewport);

            blurShader.applyShader(spriteBatch);
            //rippleShader.bindShader(tempBinding);
            //rippleShader.applyShader(spriteBatch);

            spriteBatch.Begin();

            scoreCounter.Draw(spriteBatch, mManager.smallVerdana);
        }

        private void handleMovement(Vector2 value)
        {
            if (runEffect.mParticles.Count <= 300)
            {
                runEffect.init();
            }
        }

        private void pause()
        {
            mManager.changeStates(GameManager.GameStateType.PAUSE);
        }

        private void shoot()
        {
            if (!isSpacePressed)
            {
                Shot aShot = new Shot(mManager.shotSprite);

                float shotScale = 0.10f;
                shotScale += (float)chargeFrames / 130.0f;
                aShot.trueScale = new Vector2(shotScale);

                aShot.mPosition = player.mPosition;
                aShot.mSprite.setRotation(player.mSprite.getRotation());

                if (player.mSprite.getSpriteEffects() == SpriteEffects.FlipVertically)
                {
                    aShot.mSprite.setSpriteEffects(SpriteEffects.FlipVertically);
                }

                aShot.init();
                shotList.Add(aShot);
                mManager.shootSound.Play();
                chargingSoundEffect.Pitch = 0.2f;
            }
            else
            {
                isSpacePressed = false;
            }
        }

        public override void update(GameTime gameTime)
        {
            //Map stuff
            map.Update(gameTime.ElapsedGameTime.Milliseconds);

            chargedThisFrame = false;
            input.update(gameTime);
            if (!chargedThisFrame)
            {
                chargeFrames = 0;
            }
            runEffect.update(gameTime);
            explosion.update(gameTime);

            if (player.isAlive)
            {
                updatePlayer(gameTime, map, viewport);
            }

            runEffect.mOrigin = player.mPosition;

            updateCamera();
            updatePunchees(gameTime, map);
            updateShots(gameTime);

            float minBlur = 300;
            blurShader.update(player.mPosition - camera.mPos, minBlur - 2*chargeFrames);
            //rippleShader.update(gameTime, player.mPosition);
        }

        private void updatePlayer(GameTime gameTime, Map map, xTile.Dimensions.Rectangle viewport)
        {
            player.update(gameTime, map);

            for(int i = 0; i < puncheeList.Count; i++)
            {
                if(player.mCurrentRect.Intersects(puncheeList[i].mCurrentRect))
                {
                    player.killSelf();
                    explosion.mOrigin = player.mPosition;
                    explosion.init();
                    mManager.deathSound.Play();
                }
            }

            for (int i = 0; i < scoreAreaList.Count; i++)
            {
                if (player.mCurrentRect.Intersects(scoreAreaList[i].mRectangle))
                {
                    player.killSelf();
                    explosion.mOrigin = player.mPosition;
                    explosion.init();
                    mManager.deathSound.Play();
                }
            }
        }

        private void updateCamera()
        {
            if (player.mPosition.X < camera.mPos.X + LEFTBOUND)
            {
                camera.mPos.X = player.mPosition.X - LEFTBOUND;
                viewport.X = (int)player.mPosition.X - LEFTBOUND;
            }
            else if (player.mPosition.X > camera.mPos.X + RIGHTBOUND)
            {
                camera.mPos.X = player.mPosition.X - RIGHTBOUND;
                viewport.X = (int)player.mPosition.X - RIGHTBOUND;
            }
            if (player.mPosition.Y < camera.mPos.Y + TOPBOUND)
            {
                camera.mPos.Y = player.mPosition.Y - TOPBOUND;
                viewport.Y = (int)player.mPosition.Y - TOPBOUND;
            }
            else if (player.mPosition.Y > camera.mPos.Y + BOTTOMBOUND)
            {
                camera.mPos.Y = player.mPosition.Y - BOTTOMBOUND;
                viewport.Y = (int)player.mPosition.Y - BOTTOMBOUND;
            }
        } 

        private void updateShots(GameTime gameTime)
        {
            for (int i = 0; i < shotList.Count; i++)
            {
                shotList[i].update(gameTime);

                for (int j = 0; j < puncheeList.Count; j++)
                {
                    if (shotList[i].mCurrentRect.Intersects(puncheeList[j].mCurrentRect))
                    {
                        puncheeList[j].handleCollision(shotList[i].mVelocity * shotList[i].mForce, mManager.hitSound);
                    }
                }

                if (shotList[i].framesAlive > 240)
                {
                    shotList.RemoveAt(i);
                }
            }
        }

        private void updatePunchees(GameTime gameTime, Map map)
        {
            for (int i = 0; i < puncheeList.Count; i++)
            {
                puncheeList[i].update(gameTime, map);

                for (int j = 0; j < scoreAreaList.Count; j++)
                {
                    if (scoreAreaList[j].mRectangle.Contains(new Point((int)puncheeList[i].mPosition.X, (int)puncheeList[i].mPosition.Y)))
                    {
                        puncheeList.RemoveAt(i);

                        scoreCounter.addPoints(100);
                        scoreCounter.iterateMultiplier();
                        mManager.wrangledSound.Play();
                        break;
                    }
                }
            }
        }
    }
}
