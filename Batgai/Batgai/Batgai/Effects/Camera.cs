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

namespace Batgai.Effects
{
    public class Camera
    {
        protected float mZoom;
        public Matrix mTransform;
        public Vector2 mPos = Vector2.Zero;
        protected float mRotation;
        public Vector2 mViewport;

        public Camera()
        {
            mViewport = new Vector2(0, 0);
            mZoom = 1.0f;
            mRotation = 0.0f;
        }

        public Matrix getTransform(GraphicsDevice graphicsDevice)
        {
            mTransform = Matrix.CreateTranslation(new Vector3(-mPos.X, -mPos.Y, 0)) *
                                            Matrix.CreateRotationZ(mRotation) *
                                            Matrix.CreateScale(new Vector3(mZoom, mZoom, 1)) *
                                            Matrix.CreateTranslation(new Vector3(0, 0, 0));
            return mTransform;
        }
    }
}
