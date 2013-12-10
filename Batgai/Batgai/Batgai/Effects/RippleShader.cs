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

namespace Batgai.Effects
{
    class RippleShader: Shader
    {
        public EffectParameter mWave;
        public EffectParameter mDistortion;
        public EffectParameter mCenterCoord;

        public Vector2 centerCoord = new Vector2(0.5f);
        public float distortion = 1.0f;
        public float divisor = 0.75f;
        public float wave = MathHelper.Pi;

        public int counter = 0;

        public RippleShader(GraphicsDevice device, RenderTarget2D renderTarget, Effect effect)
            :base(device, renderTarget, effect)
        {
            mWave = effect.Parameters["wave"];
            mDistortion = effect.Parameters["distortion"];
            mCenterCoord = effect.Parameters["centerCoord"];
        }

        public void update(GameTime gameTime, Vector2 position)
        {
            if (counter > 0)
                divisor += gameTime.ElapsedGameTime.Milliseconds / 200.0f;
            if (counter < 0)
                divisor -= gameTime.ElapsedGameTime.Milliseconds / 200.0f;

            counter++;
            if (counter > 100) counter = -100;

            wave = MathHelper.Pi / divisor;

            mWave.SetValue(wave);
            mDistortion.SetValue(distortion);

            //centerCoord = position;
            mCenterCoord.SetValue(centerCoord);
        }
    }
}
