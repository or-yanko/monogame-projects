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
    public class Bomb
    {
        #region data
        Boolean isFast;
        List<Vector2> vecLst;
        Vector2 vec;
        public Vector2 _position;

        protected Texture2D _texture;
        protected Texture2D _exTexture;

        public int isTaken = 0;

        public Rectangle rectangle;

        Boolean isExpload;
        public Boolean isDead = false;
        #endregion

        #region constructor
        public Bomb(Boolean isExplod, Texture2D txtr, Texture2D exploadtxtr, Vector2 pos)
        {
            this.isFast = false;
            this.isExpload = isExplod;
            this._texture = txtr;
            this._position = pos;
            this._exTexture = exploadtxtr;
            if (_position.X != -100 && _position.Y != -100)
            {
                if (isTaken < 50 && isTaken >= 3)
                    this.rectangle = new Rectangle((int)(_position.X + _exTexture.Width / 6), (int)(_position.Y + _exTexture.Height / 6), _exTexture.Width * 4 / 6, _exTexture.Height * 4 / 6);
                else if (isTaken == 0 || isTaken == 1)
                    this.rectangle = new Rectangle((int)(_position.X + _texture.Width / 6), (int)(_position.Y + _texture.Height / 6), _texture.Width * 4 / 6, _texture.Height * 4 / 6);
            }
        }
        public Bomb(Boolean isExplod, Texture2D txtr, Texture2D exploadtxtr, Vector2 pos, Boolean isFast)
        {
            this.isFast = isFast;
            this.isExpload = isExplod;
            this._texture = txtr;
            this._position = pos;
            this._exTexture = exploadtxtr;
            if (_position.X != -100 && _position.Y != -100)
            {
                if (isTaken < 50 && isTaken >= 3)
                    this.rectangle = new Rectangle((int)(_position.X + _exTexture.Width / 6), (int)(_position.Y + _exTexture.Height / 6), _exTexture.Width * 4 / 6, _exTexture.Height * 4 / 6);
                else if (isTaken == 0 || isTaken == 1)
                    this.rectangle = new Rectangle((int)(_position.X + _texture.Width / 6), (int)(_position.Y + _texture.Height / 6), _texture.Width * 4 / 6, _texture.Height * 4 / 6);
            }

        }


        #endregion

        #region draw and update

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isTaken < 50 && isTaken >= 4)
                spriteBatch.Draw(this._exTexture, _position, Color.White);
            else if (isTaken == 0 || isTaken == 1 || isTaken == 2)
                spriteBatch.Draw(this._texture, _position, Color.White);
        }

        public void Update(GameTime gameTime, Sprite hero)
        {
            int speed = 300;
            if (isTaken == 0)
            {
            }
            else if (isTaken == 1)
            {
                if (isFast == false)
                {
                    vec = new Vector2(0, 0) - (_position - new Vector2(hero.rectangle.X, hero.rectangle.Y) - new Vector2(30, 30));
                    isTaken = 2;
                    vecLst = new List<Vector2>();
                    for (int i = 0; i < speed; i++)
                        vecLst.Add(new Vector2(vec.X / speed, vec.Y / speed));

                }
                else
                {
                    vec = new Vector2(0, 0) - (_position - new Vector2(hero.rectangle.X, hero.rectangle.Y) - new Vector2(30, 30));
                    isTaken = 2;
                    vecLst = new List<Vector2>();
                    for (int i = 0; i < 100; i++)
                        vecLst.Add(new Vector2(vec.X / 100, vec.Y / 100));

                }
            }
            else if (isTaken == 2)
            {


                _position += vecLst.First();
                vecLst.Remove(vecLst.First());
                if (!vecLst.Any())
                    isTaken = 3;

            }
            else if (isTaken == 3)
            {
                if (isExpload == false)
                    isDead = true;
                else
                {
                    isTaken = 4;
                }
            }
            else if (isTaken < 50)
            {
                isTaken++;
            }
            else
            {
                isDead = true;
            }

            //rectangle = new Rectangle((int)_position.X + 4, (int)_position.Y + 5, 36, 34);

            if (isTaken < 50 && isTaken >= 4)
                this.rectangle = new Rectangle((int)(_position.X + _exTexture.Width / 6), (int)(_position.Y + _exTexture.Height / 6), _exTexture.Width * 4 / 6, _exTexture.Height * 4 / 6);
            else if (isTaken == 0 || isTaken == 1 || isTaken == 2)
                this.rectangle = new Rectangle((int)(_position.X + _texture.Width / 6), (int)(_position.Y + _texture.Height / 6), _texture.Width * 4 / 6, _texture.Height * 4 / 6);
        }

        #endregion
    }

}
