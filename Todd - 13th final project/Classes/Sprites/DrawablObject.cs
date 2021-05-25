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
    public class DrawablObject
    {
        Texture2D texture;
        float scale;
        public Rectangle rectangle;
        protected Vector2 position;
        float rotation;
        private float _timer;
        bool isFinishingStar;
        bool isDrawHafuch = false;

        public DrawablObject(Texture2D texture, float scale, Vector2 position, bool isFinishingStar)
        {
            this.texture = texture;
            this.scale = scale;
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, 70, 70);
            this.position = position;
            rotation = 0;
            this.isFinishingStar = isFinishingStar;
        }
        public DrawablObject(Texture2D texture, float scale, Vector2 position, bool isFinishingStar, bool isHafuch)
        {
            this.texture = texture;
            this.scale = scale;
            this.rectangle = new Rectangle((int)position.X, (int)position.Y, 70, 70);
            this.position = position;
            rotation = 0;
            this.isFinishingStar = isFinishingStar;
            this.isDrawHafuch = isHafuch;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(isDrawHafuch == true)
            {
                if (isFinishingStar == true)
                    spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale / 100f, SpriteEffects.FlipHorizontally, 0f);
                else
                    spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), scale / 100f, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                if (isFinishingStar == true)
                    spriteBatch.Draw(texture, position, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale / 100f, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(0, 0), scale / 100f, SpriteEffects.None, 0f);
            }
        }
        public  void Update(GameTime gameTime)
        {
            if (isFinishingStar)
            {
                rotation += 0.01f;

                _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _timer %= 2;
                if (_timer < 1)
                    scale /= 1.01f;
                else
                    scale *= 1.01f;


            }

        }
    }
}
