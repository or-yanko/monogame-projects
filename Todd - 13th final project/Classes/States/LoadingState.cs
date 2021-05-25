using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Todd.States
{
    class LoadingState : State
    {
        #region data

        // all the components
        Texture2D loadingTexture;
        SpriteFont btnTextFont;


        //background picture
        private Texture2D background;

        //next state
        State nextState;

        //time and texture of loading
        float loadingTimeSec;
        float rotation = 0;
        float currrTime;
        string loadingSentence = "loading = 0%";

        #endregion

        #region constaructor
        public LoadingState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_,
            Dictionary<string, Song> sounds, State next, float loadingTimeInSec) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //handle time and and destination of loading
            nextState = next;
            loadingTimeSec = loadingTimeInSec;
            currrTime = loadingTimeInSec;

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //load textures
             btnTextFont = _content.Load<SpriteFont>("Fonts/font");
            loadingTexture = _content.Load<Texture2D>("Contents/loading");
        }
        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);

            spritebatch.Draw(loadingTexture, new Vector2(400, 300), null, Color.White, rotation, new Vector2(loadingTexture.Width / 2, loadingTexture.Height / 2),0.5f,
                SpriteEffects.None, 0);
            spritebatch.DrawString(btnTextFont, loadingSentence, new Vector2(300, 360), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);


            spritebatch.End();
        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {
            currrTime -= (float)gametime.ElapsedGameTime.TotalSeconds;

            if (currrTime <= 0)
            {
                Console.WriteLine("loading ended");
                _game.ChangeState(nextState);

            }

            rotation += 0.03f;
            float presantage = ((loadingTimeSec - currrTime) * 100 / loadingTimeSec)/1000*1000;
            loadingSentence = "loading = " + presantage.ToString() + "%";




        }
        #endregion

    }
}
