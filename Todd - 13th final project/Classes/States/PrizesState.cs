using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todd.Managers;
using Todd.Moduls;
using Todd.Sprites;

namespace Todd.States
{
    public class PrizesState : State
    {
        #region data

        //array of all the components
        private List<Button> _components;

        //background picture
        private Texture2D background;

        private string txt;
        SpriteFont font;
        #endregion



        #region constaructor

        public PrizesState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds, CoinsAndHearts coins, int seconds,string level) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/PrizesBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");
            font = btnTextFont;

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //create the list
            _components = new List<Button>()
            {
               backButton
            };

            // data of coins and level
            txt = "Level: ";
            txt += level;
            txt += "\nTime: ";
            txt += seconds.ToString();
            txt += " sec\nGold: ";
            txt += coins.goldCoins.ToString();
            txt += "\nSilver: ";
            txt += coins.silverCoins.ToString();
            txt += "\nBronze: ";
            txt += coins.bronzeCoins.ToString();

            CoinsAndHeartsManager cahm = CoinsAndHeartsManager.Load();
            cahm.Update(coins);
            CoinsAndHeartsManager.Save(cahm);

            ScoreManager s;
            try
            {
                s = ScoreManager.Load();
            }
            catch
            {
                s = new ScoreManager(new List<Score>());
            }
             
            s.scores.Add(new Score(DateTime.UtcNow, coins, seconds, level));
            ScoreManager.Save(s);


        }
        #endregion



        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(spritebatch);

            spritebatch.DrawString(font, txt, new Vector2(300, 270), Color.Black, 0,new Vector2(0,0), 1.6f, SpriteEffects.None, 0f);

            spritebatch.End();

        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {
            foreach (var component in _components)
                component.Update(gametime);
        }
        #endregion


        #region buttons click actions

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();            
            _game.ChangeState(new GameMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }


        #endregion

    }
}
