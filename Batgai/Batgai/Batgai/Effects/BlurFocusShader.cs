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
    class BlurFocusShader: Shader
    {
        EffectParameter mPos;
        EffectParameter mCharge;

        public BlurFocusShader(GraphicsDevice device, RenderTarget2D renderTarget, Effect effect)
            :base(device, renderTarget, effect)
        {
            mPos = effect.Parameters["mFocalPos"];
            mCharge = effect.Parameters["mChargeAmount"];
        }

        public void update(Vector2 focalPos, float chargeAmount)
        {
            Vector2 adjustedPos = focalPos;
            focalPos.X = focalPos.X / 1280.0f;
            focalPos.Y = focalPos.Y / 720.0f;

            mPos.SetValue(focalPos);

            mCharge.SetValue(chargeAmount);
        }
    }
}
