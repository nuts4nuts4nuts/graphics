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
    class Shader
    {
        private GraphicsDevice mDevice;
        private Effect mEffect;
        private RenderTargetBinding[] mBinding;
        private RenderTarget2D mRenderTarget;

        public Shader(GraphicsDevice device, RenderTarget2D renderTarget, Effect effect)
        {
            mDevice = device;
            mRenderTarget = renderTarget;
            mEffect = effect;

            initDraw();
        }

        public void bindShader(RenderTargetBinding[] binding)
        {
            mBinding = binding;
        }

        public void initDraw()
        {
            mDevice.SetRenderTarget(mRenderTarget);
        }

        public void applyShader(SpriteBatch spriteBatch)
        {
            mDevice.SetRenderTargets(mBinding);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            mEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(mRenderTarget, Vector2.Zero, Color.White);

            spriteBatch.End();
        }
    }
}