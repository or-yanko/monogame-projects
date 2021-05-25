using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todd.Moduls;

namespace Todd.Managers
{
    public class AnimationManager
    {
        public Animation _animation;

        private float _timer;

        public string type = "";

        public Vector2 Position { get; set; }

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }
        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            spriteBatch.Draw(_animation.Textures[_animation.CurrentFrame], Position, null, Color.White, 0, new Vector2(0, 0), scale / 100f, SpriteEffects.None, 0f);
        }
        public void Draw(SpriteBatch spriteBatch, float scale, Vector2 v)
        {
            spriteBatch.Draw(_animation.Textures[_animation.CurrentFrame], Position, null, Color.White, 0, v, scale / 100f, SpriteEffects.None, 0f);
        }
        public void Draw(SpriteBatch spriteBatch, float scale, string side)
        {
            if (side == "right")
                spriteBatch.Draw(_animation.Textures[_animation.CurrentFrame], Position, null, Color.White, 0, new Vector2(0, 0), scale / 100f, SpriteEffects.None, 0f);
            else if (side == "left")
                spriteBatch.Draw(_animation.Textures[_animation.CurrentFrame], Position, null, Color.White, 0, new Vector2(0, 0), scale / 100f, SpriteEffects.FlipHorizontally, 0f);
        }

        public void Play(Animation animation, string type)
        {
            this.type = type;
            if (_animation == animation)
                return;
            _animation = animation;

            _animation.CurrentFrame = 0;

            _timer = 0;
        }

        public void Stop()
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;





            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrentFrame++;
                if (type == "Stand")
                {
                    if (_animation.CurrentFrame >= _animation.FrameCount)
                    {
                        Random rand = new Random();
                        int num = rand.Next(0, 3);
                        Console.WriteLine(num == 0);
                        if (num == 0)
                            _animation.CurrentFrame = 0;
                        else
                            _animation.CurrentFrame = 18;

                    }


                }
                else if (type == "JumpUp")
                {
                    if (_animation.CurrentFrame >= _animation.FrameCount)
                        _animation.CurrentFrame = 6;
                }
                else
                {
                    if (_animation.CurrentFrame >= _animation.FrameCount)
                        _animation.CurrentFrame = 0;    //equal to: 
                    //                                               _animation.CurrentFrame %= _animation.FrameCount;

                }

            }
        }

    }

}
