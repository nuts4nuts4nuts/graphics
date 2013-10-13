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

        private List<Punchee> puncheeList;

        private Camera camera;
        private GetInput input;
        private ParticleEffect runEffect;
        private ParticleEffect explosion;
        private FrameRateParticleCounter frameCounter;

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
            frameCounter = new FrameRateParticleCounter();
            camera = new Camera();
            runEffect = new ParticleEffect(mManager.particleEffect, ParticleEffect.ParticleType.Dust, BlendState.AlphaBlend);
            explosion = new ParticleEffect(mManager.particleEffect, ParticleEffect.ParticleType.Explosion, BlendState.Additive);
            input = new GetInput(PlayerIndex.One);
            player = new Hero(mManager.heroSprites);
            shotList = new List<Shot>();
            puncheeList = new List<Punchee>();
            attachEventListener();

            Punchee punched = new Punchee(mManager.startScreen);
            punched.mPosition.X = 500;
            punched.mPosition.Y = 360;
            puncheeList.Add(punched);
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
            frameCounter.Draw(spriteBatch, mManager.verdana);
            player.draw(spriteBatch);
            spriteBatch.End();
            runEffect.draw(spriteBatch);
            spriteBatch.Begin();

            for (int i = 0; i < shotList.Count; i++)
            {
                shotList[i].draw(spriteBatch);
            }

            for (int i = 0; i < puncheeList.Count; i++)
            {
                puncheeList[i].draw(spriteBatch);
            }
        }

        private void handleMovement(Vector2 value)
        {
            if (player.mPosition.X < 320 || player.mPosition.X > 960 || player.mPosition.Y < 180 || player.mPosition.Y > 540)
            {
                camera.mVelo = player.mVelocity;
            }

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
            Shot aShot = new Shot(mManager.shotSprite);

            float shotScale = 0.10f;
            shotScale += (float)chargeFrames / 120.0f;
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

        public override void update(GameTime gameTime)
        {
            input.update(gameTime);
            frameCounter.Update(gameTime, runEffect.mParticles.Count + explosion.mParticles.Count);
            runEffect.update(gameTime);
            explosion.update(gameTime);
            player.update(gameTime);

            runEffect.mOrigin = player.mPosition;

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

            for (int i = 0; i < shotList.Count; i++)
            {
                shotList[i].update(gameTime);

                for (int j = 0; j < puncheeList.Count; j++)
                {
                    if(shotList[i].mCurrentRect.Intersects(puncheeList[j].mCurrentRect))
                    {
                        puncheeList[j].handleCollision(shotList[i].mVelocity * shotList[i].mForce);
                    }
                }

                if (shotList[i].framesAlive * shotList[i].mSprite.getScale().X > 5)
                {
                    shotList.RemoveAt(i);
                }
            }
        }
    }
}
