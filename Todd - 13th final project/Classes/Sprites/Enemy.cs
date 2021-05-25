using Todd.Managers;
using Todd.Moduls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todd.States;

namespace Todd.Sprites
{
    public class Enemy
    {
        #region data
        public bool isPlayerDead;
        public Rectangle rectangle;
        Vector2 position;
        int fromx;
        int tox;
        public bool isShooting;
        public int rangeOfSight;
        public Vector2 velocity;
        Vector2 baseVelocity;
        float scale;
        public float timer = 0;

        //animation
        Texture2D texture;
        public AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;


        #endregion

        #region constructor
        public Enemy(Dictionary<string, Animation> animations, float scale, Vector2 position, Vector2 baseVelocity, int fromx, int tox, bool isShooting, int rangeOfSight)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            this.scale = scale;
            if (position.X > fromx && position.X < tox)
                this.position = position;
            else
                this.position = new Vector2((fromx + tox) / 2, position.Y);
            this.fromx = fromx;
            this.tox = tox;
            this.baseVelocity = baseVelocity;
            this.isShooting = isShooting;
            this.rangeOfSight = rangeOfSight;
            velocity = baseVelocity;


        }

        #endregion

        #region draw and update

        public  void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), scale / 100f, SpriteEffects.None, 0f);
            else if (_animationManager != null)
            {
                if (velocity.X >= 0)
                    _animationManager.Draw(spriteBatch, scale, "right");
                else
                    _animationManager.Draw(spriteBatch, scale, "left");
            }
            else throw new Exception("This ain't right..!");

        }

        public  void Update(GameTime gameTime, Sprite h)
        {
            //timer for shooting
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //handle movement on the board
            position += velocity;
            if (position.X < fromx || position.X > tox)
                velocity = new Vector2(0, 0) - velocity;

            ////gravity
            if(isShooting == true)
            {
                rectangle = new Rectangle((int)position.X + (int)(180 * scale / 100f), (int)position.Y + (int)(90 * scale / 100f), (int)(140 * scale / 100f), (int)(200 * scale / 100f));
            }
            else
            {
                rectangle = new Rectangle((int)position.X + (int)(312 * scale / 100f), (int)position.Y + (int)(290 * scale / 100f), (int)(270 * scale / 100f), (int)(440 * scale / 100f));
            }

            //animation

            SetAnimations();
            _animationManager.Update(gameTime);
            _animationManager.Position = position;

        }



        #endregion

        #region other functions

        //private void handlrGravity(GameTime gameTime)
        //{
        //    position += velocity;

        //    try
        //    {
        //        rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(scale / 100f * texture.Width), (int)(scale / 100f * texture.Height));
        //    }
        //    catch
        //    {
        //        rectangle = new Rectangle((int)position.X, (int)position.Y, 100, 100);
        //    }

        //    if (Keyboard.GetState().IsKeyDown(Keys.Right)) velocity.X = 3f;
        //    else if (Keyboard.GetState().IsKeyDown(Keys.Left)) velocity.X = -3f; else velocity.X = 0f;

        //    if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasJumped == false)
        //    {
        //        //MediaPlayer.Play(sounds_dict["jump"]);
        //        position.Y -= 10f;
        //        velocity.Y = -5f;
        //        hasJumped = true;
        //    }
        //    if (position.Y >= 10000)
        //    {
        //        hasJumped = false;
        //        isPlayerDead = true;
        //    }
        //    if (hasJumped == true)
        //    {
        //        float i = 1;
        //        velocity.Y += 0.15f * i;

        //    }

        //}
        //private void updateVelovity(Sprite h)
        //{
        //    if (isSuiside && isInSightRange(h))//if seeing hero 
        //    {
        //        if (h.Position.X > position.X)
        //        {
        //            if (h.Position.Y > position.Y)
        //                velocity = new Vector2(baseVelocity.X, baseVelocity.Y);
        //            else
        //                velocity = new Vector2(baseVelocity.X, -baseVelocity.Y);
        //        }
        //        else
        //        {
        //            if (h.Position.Y > position.Y)
        //                velocity = new Vector2(-baseVelocity.X, baseVelocity.Y);
        //            else
        //                velocity = new Vector2(-baseVelocity.X, -baseVelocity.Y);
        //        }
        //        //change place of patrolling
        //        tox = (int)position.X + (tox - fromx);
        //        fromx = (int)position.X;
        //    }
        //    else
        //    {
        //        if (position.X < fromx || position.X > tox)
        //            velocity = new Vector2(0, 0) - velocity;
        //    }
        //}

        //private bool isInSightRange(Sprite h)
        //{
        //    return (rangeOfSight * rangeOfSight >= (h.Position.X - position.X) * (h.Position.X - position.X) + (h.Position.Y - position.Y) * (h.Position.Y - position.Y));
        //}

        protected  void SetAnimations()
        {
            if (velocity.X != 0)
                _animationManager.Play(_animations["WalkRight"], "WalkRight");
        }



        #endregion
    }

}
