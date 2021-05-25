using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todd.Managers;
using Todd.Moduls;

namespace Todd.Sprites
{
    public class Boss
    {
        #region data
        float scale = 100;

        //alive or die
        public Boolean isPlayerDead = false;

        public Rectangle rectangle;

        //animation
        public AnimationManager animationManager;
        protected Dictionary<string, Animation> _animations;

        public Vector2 position;
        public Vector2 velocity;

        protected Texture2D texture;

        #endregion

        #region constructor
        public Boss(float scale, Dictionary<string, Animation> animations, Vector2 position)
        {
            this.scale = scale;
            _animations = animations;
            animationManager = new AnimationManager(_animations.First().Value);
            this.position = position;
            this.rectangle = new Rectangle((int)position.X + 45, (int)position.Y + 35, 40, 55);
        }
        #endregion

        #region draw and update
        public void Draw(SpriteBatch spriteBatch, string side)
        {
            if (side == "left")
                animationManager.Draw(spriteBatch, scale, "left");
            else if (side == "right")
                animationManager.Draw(spriteBatch, scale, "right");

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), scale / 100f, SpriteEffects.None, 0f);
            else if (animationManager != null)
            {
                if (velocity.X >= 0)
                    animationManager.Draw(spriteBatch, scale, "right");
                else
                    animationManager.Draw(spriteBatch, scale, "left");
            }
            else throw new Exception("This ain't right..!");


        }
        public void Update(GameTime gameTime, string command)
        {
            this.rectangle = new Rectangle((int)position.X + 45, (int)position.Y + 35, 40, 55);


            UpdatePosition(gameTime, command);

            this.position += velocity;

            SetAnimations();

            animationManager.Update(gameTime);

            animationManager.Position = position;

        }
        #endregion

        #region other functions
        private void UpdatePosition(GameTime gameTime, string command)
        {
            float i = 5; 
            if (command == "rd")
                velocity = new Vector2(i, i);
            else if (command == "r")
                velocity = new Vector2(i, 0);
            else if (command == "ru")
                velocity = new Vector2(i, -i);
            else if (command == "ld")
                velocity = new Vector2(-i, i);
            else if (command == "l")
                velocity = new Vector2(-i, 0);
            else if (command == "lu")
                velocity = new Vector2(-i, -i);
            else if (command == "d")
                velocity = new Vector2(0, i);
            else if (command == "u")
                velocity = new Vector2(0, -i);
            else
                velocity = new Vector2(0, 0);
        }

        protected virtual void SetAnimations()
        {
            if (velocity.X == 0 && velocity.Y == 0)
                animationManager.Play(_animations["Stand"], "Stand");
            else
                animationManager.Play(_animations["Walk"], "Walk");
        }



        #endregion

    }
}