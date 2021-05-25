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
    class ScoresState : State
    {
        #region data

        //array of all the components
        private List<Button> _components;

        //background picture
        private Texture2D background;

        //games list
        List<string> gamesNamesLst = new List<string>()
        {
            "1","2","3", "Survival", "All"
        };
        int posInGamesNamesLst = 0;
        ScoreManager scoreManager = new ScoreManager(ScoreManager.Load().scores);


        SpriteFont font;
        #endregion

        #region constaructor
        public ScoresState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //font
            font = _content.Load<SpriteFont>("Fonts/font");

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/PrizesBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //handle next button
            var nextButton = new Button(_content.Load<Texture2D>("Buttons/nextBtn"), new Vector2(600, 420),
                Color.Black, Color.White, btnTextFont, "", 15);
            nextButton.Click += nextButtonClick;

            //handle prev button
            var prevButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(250, 420),
                Color.Black, Color.White, btnTextFont, "", 15);
            prevButton.Click += prevButtonClick;

            //create the list
            _components = new List<Button>()
            {
               backButton, nextButton, prevButton
            };
            scoreManager.updateHighScore("time", "1");
        }

        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(spritebatch);


            spritebatch.DrawString(font, "Level:" + gamesNamesLst[posInGamesNamesLst], new Vector2(376, 254), Color.Black, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);

            spritebatch.DrawString(font, "Date", new Vector2(288, 295), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spritebatch.DrawString(font, "Time" , new Vector2(395, 295), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            spritebatch.DrawString(font, "Coins (G S B)", new Vector2(490, 295), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

            List <List<string>> strLst = new List<List<string>>();

            for(int i =0; i<3; i++)
            {//ToString("MM/dd/yyyy")
                try
                {
                    spritebatch.DrawString(font, scoreManager.highscore[i].date.ToString("MM/dd/yyyy"), new Vector2(290, 340 + i * 40), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                    spritebatch.DrawString(font, scoreManager.highscore[i].timeInSeconds.ToString(), new Vector2(385, 340 + i * 40), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                    spritebatch.DrawString(font, scoreManager.highscore[i].coinsAndHearts.goldCoins.ToString() + " " + scoreManager.highscore[i].coinsAndHearts.silverCoins.ToString() + " " + scoreManager.highscore[i].coinsAndHearts.bronzeCoins.ToString(), new Vector2(520, 340 + i * 40), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                }
                catch
                {
                    spritebatch.DrawString(font, "Not Valid", new Vector2(290, 340 + i * 40), Color.Black, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);
                }
            }

            foreach(Score s in scoreManager.highscore)
            {

            }

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
            Console.WriteLine("Back button pressed, Moving to How To Play Window");
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }
        private void prevButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            if (posInGamesNamesLst == 0)
                posInGamesNamesLst = gamesNamesLst.Count() - 1;
            else
                posInGamesNamesLst--;

            ///update data
            scoreManager.updateHighScore("time", gamesNamesLst[posInGamesNamesLst]);

        }

        private void nextButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            posInGamesNamesLst = (posInGamesNamesLst + 1) % gamesNamesLst.Count();

            ///update data
            scoreManager.updateHighScore("time", gamesNamesLst[posInGamesNamesLst]);
        }


        #endregion
    }
}
