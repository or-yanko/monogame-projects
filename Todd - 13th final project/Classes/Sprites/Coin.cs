using Todd.Managers;
using Todd.Moduls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todd.Sprites
{
    public class Coin
    {
        #region data

        public string type = "";

        float scale;
        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        protected Vector2 _position;

        protected Texture2D _texture;

        public Boolean isTaken = false;

        public Rectangle rectangle;



        #endregion

        #region constructor
        public Coin(Dictionary<string, Animation> animations, float scale, Vector2 _position, string type)
        {
            this._position = _position;
            this.scale = scale;
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            this.type = type;
            int n = 5;
            rectangle = new Rectangle((int)_position.X + n, (int)_position.Y + n, (int)(animations["spin"].Textures[0].Width * scale / 100f) - 2 * n, (int)(animations["spin"].Textures[0].Height * scale / 100f) - n * n);
        }

        #endregion

        #region draw and update

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!isTaken)
            {
                if (_texture != null)
                    spriteBatch.Draw(_texture, _position, Color.White);
                else if (_animationManager != null)
                    _animationManager.Draw(spriteBatch, scale, new Vector2(_animations["spin"].Textures[0].Width / 2, _animations["spin"].Textures[0].Height / 2));
                else throw new Exception("This ain't right..!");

            }
            else
            {
                _position = new Vector2(0, 0);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!isTaken)
            {
                _animationManager.Play(_animations["spin"], "spin");
                _animationManager.Update(gameTime);
                _animationManager.Position = _position;

            }
        }

        #endregion


    }
}