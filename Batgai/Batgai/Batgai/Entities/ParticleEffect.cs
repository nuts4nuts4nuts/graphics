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

namespace Batgai.Entities
{
    class ParticleEffect
    {
        Random random;

        public enum ParticleType
        {
            Dust,
            Explosion,
            Spiral
        }

        private ParticleType m_pType;

        public Texture2D particleTexture;

        public Vector2 mOrigin;
        public int mOriginRadius;

        public int mEffectDuration;
        public int mNewParticleAmount;
        public int mBurstFrequencyMS;
        public int mBurstCountdownMS;

        public BlendState mBlendType;
        Camera mCamera;
        public List<Particle> mParticles;

        public ParticleEffect(Texture2D texture, ParticleType pType, BlendState blend, Camera camera)
        {
            mCamera = camera;
            mParticles = new List<Particle>();
            particleTexture = texture;
            m_pType = pType;
            mBlendType = blend;
        }

        public void init()
        {
            switch (m_pType)
            {
                case ParticleType.Dust:
                    initDust();
                    break;
                case ParticleType.Explosion:
                    initExplosion();
                    break;
            }
        }

        private void initDust()
        {
            random = new Random();
            mEffectDuration = 3000;
            mNewParticleAmount = 10;
            mBurstFrequencyMS = 8;
            mBurstCountdownMS = mBurstFrequencyMS;
        }

        private void initExplosion()
        {
            random = new Random();
            mEffectDuration = 200;
            mNewParticleAmount = 150;
            mBurstFrequencyMS = 8;
            mBurstCountdownMS = mBurstFrequencyMS;
        }

        public void createDust()
        {
            int initAge = 500;

            Vector2 initPos = mOrigin;
            Vector2 initVelo = new Vector2(((float)(50.0f * Math.Cos(mEffectDuration))), ((float)(50.0f * Math.Sin(mEffectDuration))));

            Vector2 initAcc = new Vector2(0, 75);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = 1.0f;
            float initRotDamp = 0.99f;

            float initScale = 0.3f;
            float initScaleVelo = -0.01f;
            float initScaleAcc = 0.1f;
            float maxScale = 1.0f;

            Color initColor = new Color( (random.Next(0,255)), (random.Next(0,255)), (random.Next(0,255)));
            Color finalColor = new Color( (random.Next(0, 255)), (random.Next(0, 255)), (random.Next(0, 255)));
            finalColor.A = 0;
            int fadeAge = initAge;

            Particle tempParticle = new Particle();
            tempParticle.create(particleTexture, initAge, initPos, initVelo, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVelo, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            mParticles.Add(tempParticle);
        }

        public void createExplosion()
        {
            int initAge = 2000 + (int)random.Next(5000);
            mOriginRadius = 20;

            Vector2 initPos = mOrigin;

            Vector2 offset = Vector2.Zero;
            offset.X = ((float)(random.Next(mOriginRadius) * Math.Cos(random.Next(360))));
            offset.Y = ((float)(random.Next(mOriginRadius) * Math.Sin(random.Next(360))));
            initPos += offset;

            Vector2 initVelo = Vector2.Zero;
            initVelo.X = random.Next(500) + (offset.X * 30);
            initVelo.Y = random.Next(500) + (offset.X * 30);
            Vector2 initAcc = new Vector2(0, 400);

            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = initVelo.X / 50.0f;
            float initRotDamp = 0.99f;

            float initScale = 0.1f + ((float)random.Next(10)) / 50.0f;
            float initScaleVelo = ((float)random.Next(10)) / 50.0f;
            float initScaleAcc = 0.0f;
            float maxScale = 1.0f;

            Color initColor = new Color((random.Next(0, 255)), (random.Next(0, 255)), (random.Next(0, 255)));
            Color finalColor = new Color((random.Next(0, 255)), (random.Next(0, 255)), (random.Next(0, 255)));
            finalColor.A = 0;
            int fadeAge = initAge / 2;

            Particle tempParticle = new Particle();
            tempParticle.create(particleTexture, initAge, initPos, initVelo, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVelo, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            mParticles.Add(tempParticle);
        }

        public void createParticle(ParticleType particleType)
        {
            switch (particleType)
            {
                case ParticleType.Dust:
                    createDust();
                    break;
                case ParticleType.Explosion:
                    createExplosion();
                    break;
                case ParticleType.Spiral:
                    createSpiral();
                    break;
            }
        }

        public void createSpiral()
        {
            int initAge = 3000;

            Vector2 initPos = mOrigin;
            Vector2 initVelo = new Vector2(((float)(100.0f * Math.Cos(mEffectDuration))), ((float)(100.0f * Math.Sin(mEffectDuration))));

            Vector2 initAcc = new Vector2(0, 75);
            float initDamp = 1.0f;

            float initRot = 0.0f;
            float initRotVel = 2.0f;
            float initRotDamp = 0.99f;

            float initScale = 0.2f;
            float initScaleVelo = 0.2f;
            float initScaleAcc = -0.1f;
            float maxScale = 1.0f;

            Color initColor = Color.White;
            Color finalColor = Color.White;
            finalColor.A = 0;
            int fadeAge = initAge;

            Particle tempParticle = new Particle();
            tempParticle.create(particleTexture, initAge, initPos, initVelo, initAcc, initDamp, initRot, initRotVel, initRotDamp, initScale, initScaleVelo, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            mParticles.Add(tempParticle);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, mBlendType, null, null, null, null, mCamera.mTransform);
            foreach(Particle p in mParticles)
            {
                p.draw(spriteBatch);
            }
            spriteBatch.End();
        }

        public void update(GameTime gameTime)
        {
            mEffectDuration -= gameTime.ElapsedGameTime.Milliseconds;
            mBurstCountdownMS -= gameTime.ElapsedGameTime.Milliseconds;

            if ((mBurstCountdownMS <= 0) && (mEffectDuration >= 0))
            {
                for (int i = 0; i < mNewParticleAmount; i++)
                {
                    createParticle(m_pType);
                }

                mBurstCountdownMS = mBurstFrequencyMS;
            }

            for (int i = mParticles.Count - 1; i >= 0; i--)
            {
                mParticles[i].update(gameTime);

                if (mParticles[i].mAge <= 0)
                {
                    mParticles.RemoveAt(i);
                }
            }
        }
    }
}
