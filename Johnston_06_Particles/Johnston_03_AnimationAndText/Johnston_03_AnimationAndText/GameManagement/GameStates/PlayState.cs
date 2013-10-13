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
using Johnston_06_Particles.Entities;
using Johnston_06_Particles.Input;

namespace Johnston_06_Particles.GameManagement
{
    public class PlayState : GameState
    {
        private Hero player;
        private List<Shot> shotList;
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
            attachEventListener();
        }

        private void action()
        {
            
        }

        private void attachEventListener()
        {
            input.event_backPressed += new GetInput.buttonPressDelegate(pause);
            input.event_actionPressed += new GetInput.buttonPressDelegate(action);
            input.event_directionPressed += new GetInput.directionPressDelegate(handleMovement);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            frameCounter.Draw(spriteBatch, mManager.verdana);
            player.draw(spriteBatch);
            spriteBatch.End();
            runEffect.draw(spriteBatch);
            explosion.draw(spriteBatch);
            spriteBatch.Begin();
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

        public override void update(GameTime gameTime)
        {
            input.update(gameTime);
            frameCounter.Update(gameTime, runEffect.mParticles.Count + explosion.mParticles.Count);
            runEffect.update(gameTime);
            explosion.update(gameTime);
            player.update(gameTime);

            runEffect.mOrigin = player.mPosition;
            runEffect.mOrigin.Y += player.mySprite.getSourceRect().Height;
            runEffect.mOrigin.X += player.mySprite.getSourceRect().Width / 2;
        }
    }
}
