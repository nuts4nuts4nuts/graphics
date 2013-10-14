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
using Batgai.Entities;
using Batgai.Input;

namespace Batgai.GameManagement
{
    public class PlayState : GameState
    {
        private Hero player;
        private List<Shot> shotList;
        private int chargeFrames;
        private bool isSpacePressed = true;

        private List<Punchee> puncheeList;

        private Camera camera;
        private GetInput input;
        private ParticleEffect runEffect;
        private ParticleEffect explosion;
        //private FrameRateParticleCounter frameCounter;

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
            //frameCounter = new FrameRateParticleCounter();
            camera = new Camera();
            runEffect = new ParticleEffect(mManager.particleEffect, ParticleEffect.ParticleType.Dust, BlendState.AlphaBlend, camera);
            explosion = new ParticleEffect(mManager.particleEffect, ParticleEffect.ParticleType.Explosion, BlendState.Additive, camera);
            input = new GetInput(PlayerIndex.One);
            player = new Hero(mManager.heroSprites);
            shotList = new List<Shot>();
            puncheeList = new List<Punchee>();
            attachEventListener();


            for (int i = 0; i < 40; i++)
            {
                for(int j = 0; j < 22; j++)
                {
                    if (i % 2 == 0)
                    {
                        Punchee punched = new Punchee(mManager.startScreen);
                        punched.mPosition.X = 32 * i;
                        punched.mPosition.Y = 32 * j;
                        puncheeList.Add(punched);
                    }
                }
            }
        }

        private void charge(int numFrames)
        {
            if (numFrames <= 120)
            {
                chargeFrames = numFrames;
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
            GraphicsDevice device = mManager.mGraphicsDevice;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,
                camera.getTransform(device));
            //frameCounter.Draw(spriteBatch, mManager.verdana);
            player.draw(spriteBatch);

            for (int i = 0; i < shotList.Count; i++)
            {
                shotList[i].draw(spriteBatch);
            }

            for (int i = 0; i < puncheeList.Count; i++)
            {
                puncheeList[i].draw(spriteBatch);
            }


            spriteBatch.End();
            runEffect.draw(spriteBatch);
            spriteBatch.Begin();
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
                aShot.mSprite.setScale(new Vector2(shotScale));

                aShot.mPosition = player.mPosition;
                aShot.mSprite.setRotation(player.mSprite.getRotation());

                if (player.mSprite.getSpriteEffects() == SpriteEffects.FlipVertically)
                {
                    aShot.mSprite.setSpriteEffects(SpriteEffects.FlipVertically);
                }

                aShot.init();
                shotList.Add(aShot);
            }
            else
            {
                isSpacePressed = false;
            }
        }

        public override void update(GameTime gameTime)
        {
            input.update(gameTime);
            //frameCounter.Update(gameTime, runEffect.mParticles.Count + explosion.mParticles.Count);
            runEffect.update(gameTime);
            explosion.update(gameTime);
            player.update(gameTime);

            runEffect.mOrigin = player.mPosition;

            updateCamera();
            updatePunchees(gameTime);
            updateShots(gameTime);
        }

        private void updateCamera()
        {
            if (player.mPosition.X < camera.mPos.X + 320)
            {
                camera.mPos.X = player.mPosition.X - 320;
            }
            else if (player.mPosition.X > camera.mPos.X + 960)
            {
                camera.mPos.X = player.mPosition.X - 960;
            }
            if (player.mPosition.Y < camera.mPos.Y + 180)
            {
                camera.mPos.Y = player.mPosition.Y - 180;
            }
            else if (player.mPosition.Y > camera.mPos.Y + 540)
            {
                camera.mPos.Y = player.mPosition.Y - 540;
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
                        puncheeList[j].handleCollision(shotList[i].mVelocity * shotList[i].mForce);
                    }
                }

                if (shotList[i].framesAlive > 240)
                {
                    shotList.RemoveAt(i);
                }
            }
        }

        private void updatePunchees(GameTime gameTime)
        {
            for (int i = 0; i < puncheeList.Count; i++)
            {
                puncheeList[i].update(gameTime);

                if (puncheeList[i].mPosition.X > 1280)
                {
                    puncheeList[i].mPosition.X = 1280;
                    puncheeList[i].mVelocity.X = -puncheeList[i].mVelocity.X;

                }
                else if (puncheeList[i].mPosition.X < 0)
                {
                    puncheeList[i].mPosition.X = 0;
                    puncheeList[i].mVelocity.X = -puncheeList[i].mVelocity.X;
                }

                if (puncheeList[i].mPosition.Y > 720)
                {
                    puncheeList[i].mPosition.Y = 720;
                    puncheeList[i].mVelocity.Y = -puncheeList[i].mVelocity.Y;
                }
                else if (puncheeList[i].mPosition.Y < 0)
                {
                    puncheeList[i].mPosition.Y = 0;
                    puncheeList[i].mVelocity.Y = -puncheeList[i].mVelocity.Y;
                }
            }
        }
    }
}
