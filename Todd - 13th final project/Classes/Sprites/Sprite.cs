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


namespace Todd.Sprites
{
    public class Sprite
    {

        #region data
        float scale = 100;

        //alive or die
        public Boolean isPlayerDead = false;

        //gravity
        public Vector2 velocity;
        public bool hasJumped;
        public Rectangle rectangle;

        //animation
        public AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;

        public Vector2 _position;

        protected Texture2D _texture;

        #endregion

        #region constructor
        public Sprite(Dictionary<string, Animation> animations, float scale)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            this.scale = scale;
        }
        public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        #endregion

        #region Properties

        public Input Input;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float Speed = 1f;


        #endregion

        #region draw and update

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, null, Color.White, 0, new Vector2(0, 0), scale / 100f, SpriteEffects.None, 0f);
            else if (_animationManager != null)
            {
                if (velocity.X >= 0)
                    _animationManager.Draw(spriteBatch, scale, "right");
                else
                    _animationManager.Draw(spriteBatch, scale, "left");
            }
            else throw new Exception("This ain't right..!");
        }

        public virtual void Update(GameTime gameTime)
        {

            handlrGravity(gameTime);

            SetAnimations();

            _animationManager.Update(gameTime);

            Position = _position;

        }

        #endregion

        #region other functions

        private void handlrGravity(GameTime gameTime)
        {
            _position += velocity;

            rectangle = new Rectangle((int)_position.X + 45, (int)_position.Y + 35, 45, 60);

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) velocity.X = 3f;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left)) velocity.X = -3f; else velocity.X = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && hasJumped == false)
            {
                _position.Y -= 10f;
                velocity.Y = -8.5f;
                hasJumped = true;
            }
            if (_position.Y >= 2300)
            {
                isPlayerDead = true;
            }
            if (hasJumped == true)
            {
                float i = 1;
                if (velocity.Y < 4.85)
                    velocity.Y += 0.15f * i;
            }

        }


        protected virtual void SetAnimations()
        {
            if (velocity.Y > 0)
                _animationManager.Play(_animations["FallDown"], "FallDown");
            else if (velocity.Y < 0)
                _animationManager.Play(_animations["JumpUp"], "JumpUp");
            else if (velocity.X != 0)
                _animationManager.Play(_animations["WalkRight"], "WalkRight");
            else _animationManager.Play(_animations["Stand"], "Stand");
        }

        #endregion

    }

}
