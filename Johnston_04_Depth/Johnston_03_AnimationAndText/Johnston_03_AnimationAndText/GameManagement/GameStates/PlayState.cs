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
using Johnston_04_Depth.Entities;
using Johnston_04_Depth.Input;

namespace Johnston_04_Depth.GameManagement
{
    public class PlayState : GameState
    {
        private bool isSpacePressed = true;
        private Sprite background;
        private Hero hero;
        private ViewOffset viewport;
        private List<AnimatedSprite> bullets;
        private List<Sprite> stars;
        private const float FAST_SPEED = 10.0f;
        private const float SLOW_SPEED = 3.0f;

        private const int HORIZON = 240;

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
            bullets = new List<AnimatedSprite>();
            stars = new List<Sprite>();
            background = new Sprite(mManager.playBG, new Rectangle(0, 0, 1280, 720), new Vector2(0));
            hero = new Hero(mManager.heroSprites);
            viewport = new ViewOffset(new Vector2(0, 0));

            Random rand = new Random();

            for (int i = 0; i < 25; i++)
            {
                int randNumX = rand.Next(-1280, 2560);
                int randNumY = rand.Next(HORIZON, 400);
                Vector2 position = new Vector2(randNumX, randNumY);
                float rotation = (float)rand.NextDouble();

                Sprite star = new Sprite(mManager.star, new Rectangle(0, 0, 216, 227), position, new Vector2(1), new Vector2(108, 114), rotation);
                star.setColor(Color.Red);

                stars.Add(star);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            background.draw(spriteBatch);

            for (int i = 0; i < stars.Count; i++)
            {
                if (stars[i].getPosition().Y > HORIZON + viewport.getDimensions().Y - (stars[i].getSourceRect().Height*stars[i].getScale().X))
                {
                    stars[i].draw(spriteBatch, viewport);
                }
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].draw(spriteBatch, viewport);
            }

            hero.draw(spriteBatch, viewport);
        }

        public override void update(GameTime gameTime)
        {
            GetInput input = new GetInput();

            if(input.isKeyHit(Keys.Escape) || input.isButtonPressed(Buttons.Start))
            {
                mManager.changeStates(GameManager.GameStateType.PAUSE);
            }

            bool doesShoot = hero.updateWithBool(gameTime);

            if (doesShoot && !isSpacePressed)
            {
                AnimatedSprite bullet = new AnimatedSprite(mManager.doomBullets, new Rectangle(10, 550, 65, 60),
                    new Vector2((hero.getPosition().X + 80), hero.getPosition().Y), 2, 90);
                bullet.setOrigin(new Vector2(30, 60));
                bullets.Add(bullet);
            }

            for (int i = 0; i < stars.Count; i++)
            {
                float depth = 100 / (hero.getPosition().Y - stars[i].getPosition().Y);
                stars[i].setDepth(depth);

                Color newColor = stars[i].getColor();

                float tempColor = 0.75f + (stars[i].getDepth() * 0.25f);
                newColor = new Color(tempColor, tempColor, tempColor);

                stars[i].setScale(new Vector2(depth));

                stars[i].setColor(newColor);
                stars[i].setRotation(stars[i].getRotation() - 3);
                stars[i].update(gameTime);
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                Rectangle rect1 = new Rectangle(0, 0, 0, 0);
                float depth = (bullets[i].getPosition().Y - (HORIZON + viewport.getDimensions().Y)) / (((720 + viewport.getDimensions().Y) - (HORIZON + viewport.getDimensions().Y)));
                bullets[i].setDepth(depth);
                float parallaxSpeed = MathHelper.Lerp(FAST_SPEED, SLOW_SPEED, depth);

                float tempColor = 0.75f + (bullets[i].getDepth() * 0.25f);
                Color newColor = new Color(tempColor, tempColor, tempColor);

                bullets[i].setPosition(new Vector2((bullets[i].getPosition().X), bullets[i].getPosition().Y - 10));
                bullets[i].setScale(new Vector2(0.20f + (depth * 0.80f)));
                bullets[i].setColor(newColor);
                bullets[i].update(gameTime);

                if (bullets[i] != null)
                {
                    rect1 = new Rectangle((int)bullets[i].getPosition().X, (int)bullets[i].getPosition().Y, bullets[i].getSourceRect().X, bullets[i].getSourceRect().Y);
                }

                if (bullets[i].getPosition().Y < HORIZON - viewport.getDimensions().Y - bullets[i].getSourceRect().Height)
                {
                    bullets.RemoveAt(i);
                }
                else
                {
                    for (int j = 0; j < stars.Count; j++)
                    {
                        Rectangle rect2 = new Rectangle((int)stars[j].getPosition().X, (int)stars[j].getPosition().Y, stars[j].getSourceRect().X, stars[j].getSourceRect().Y);

                        if (rect1.Intersects(rect2))
                        {
                            bullets.RemoveAt(i);
                            stars[j].setColor(Color.Red);
                        }
                    }
                }
            }

            if (input.isKeyHeld(Keys.Right) || input.isButtonHeld(Buttons.DPadRight))
            {
                hero.setVelocity(new Vector2(hero.getVelocity().X + 25f, (hero.getVelocity().Y)));
            }
            else if (input.isKeyHeld(Keys.Left) || input.isButtonHeld(Buttons.DPadLeft))
            {
                hero.setVelocity(new Vector2(hero.getVelocity().X - 25f, (hero.getVelocity().Y)));
            }

            if (input.isKeyHeld(Keys.Down) || input.isButtonHeld(Buttons.DPadDown))
            {
                hero.setVelocity(new Vector2(hero.getVelocity().X, (hero.getVelocity().Y + 25f)));
            }
            else if (input.isKeyHeld(Keys.Up) || input.isButtonHeld(Buttons.DPadUp))
            {
                hero.setVelocity(new Vector2(hero.getVelocity().X, (hero.getVelocity().Y - 25f)));
            }

            if (input.isKeyHeld(Keys.Space) || input.isButtonHeld(Buttons.A))
            {
                isSpacePressed = true;
            }
            else
            {
                isSpacePressed = false;
            }

            viewport.setDimensions(new Vector2(-640 + hero.getPosition().X + (hero.getSourceRect().Width / 2), -720 + hero.getPosition().Y + (hero.getSourceRect().Width / 2)));
        }
    }
}
