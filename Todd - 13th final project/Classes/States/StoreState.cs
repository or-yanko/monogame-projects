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
using Todd.Managers;
using Todd.Moduls;
using Todd.Sprites;

namespace Todd.States
{
    class StoreState : State
    {

        #region data
        int isWaiting = 0;

        //array of all the components
        private List<Button> _components;

        //background picture
        private Texture2D background;

        //hearts and coins
        CoinsAndHeartsManager coinsAndHeartsManager;

        //list of buying options
        private List<Texture2D> buyingOptions;
        int posInBuyingOptions;

        //notEnoughMoneyError texture
        Texture2D notEnoughMoneyError;


        #endregion

        #region constaructor
        public StoreState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {   
            //load coinsAndHeartsManager from file
            coinsAndHeartsManager = new CoinsAndHeartsManager();
            coinsAndHeartsManager = CoinsAndHeartsManager.Load();

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/storeBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //handle shop option button
            var shopOptionButton = new Button(_content.Load<Texture2D>("Shop/bToSShop"), new Vector2(100,100),
                Color.Black, Color.White, btnTextFont, "", 100);
            shopOptionButton.Click += shopOptionButtonClick;

            //handle next button
            var nextButton = new Button(_content.Load<Texture2D>("Buttons/nextBtn"), new Vector2(550,420),
                Color.Black, Color.White, btnTextFont, "", 15);
            nextButton.Click += nextButtonClick;

            //handle prev button
            var prevButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(250, 420),
                Color.Black, Color.White, btnTextFont, "", 15);
            prevButton.Click += prevButtonClick;

            //create the list
            _components = new List<Button>()
            {
               backButton, shopOptionButton, nextButton, prevButton
            };

            //load list of bying options
            buyingOptions = new List<Texture2D>()
            {
                _content.Load<Texture2D>("Shop/bToSShop"),
                _content.Load<Texture2D>("Shop/sToGShop"),
                _content.Load<Texture2D>("Shop/sTosShop"),
                _content.Load<Texture2D>("Shop/gToElShop")
            };
            posInBuyingOptions = 0;

            //notEnoughMoneyError texture
            notEnoughMoneyError = _content.Load<Texture2D>("Errors/notEnoughMoneyError");
        }




        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            if (isWaiting == 2)
            {
                Thread.Sleep(3000);
                isWaiting = 0;
            }
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(spritebatch);
            coinsAndHeartsManager.Draw(spritebatch, _content);
            if (isWaiting == 1)
            {
                spritebatch.Draw(notEnoughMoneyError, new Vector2(200, 100), Color.White);
                isWaiting = 2;
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

        private void shopOptionButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            if (posInBuyingOptions == 0)
            {
                if (coinsAndHeartsManager.curr.bronzeCoins < 100)
                    notEnoghMoneyWarning();
                else
                {
                    coinsAndHeartsManager.Update(new CoinsAndHearts(0, 7, -100, 0));
                    CoinsAndHeartsManager.Save(coinsAndHeartsManager);
                }
            }
            else if(posInBuyingOptions == 1)
            {
                if (coinsAndHeartsManager.curr.silverCoins < 83)
                    notEnoghMoneyWarning();
                else
                {
                    coinsAndHeartsManager.Update(new CoinsAndHearts(3, -83, 0, 0));
                    CoinsAndHeartsManager.Save(coinsAndHeartsManager);
                }
            }
            else if (posInBuyingOptions == 2)
            {
                if (coinsAndHeartsManager.curr.silverCoins < 10)
                    notEnoghMoneyWarning();
                else
                {
                    coinsAndHeartsManager.Update(new CoinsAndHearts(0, -10, 0, 0));
                    CoinsAndHeartsManager.Save(coinsAndHeartsManager);
                    DateTime d = SlotMachineState.Load();
                    d = new DateTime(d.Year - 1, d.Month, d.Day);
                    SlotMachineState.Save(d);
                    _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict,
                    new SlotMachineState(_game, _graphicsDevice, _content, graphics, _soundsDict), 2f));
                }
            }
            else if (posInBuyingOptions == 3)
            {
                if (coinsAndHeartsManager.curr.goldCoins < 2)
                    notEnoghMoneyWarning();
                else
                {
                    coinsAndHeartsManager.Update(new CoinsAndHearts(-2, 0, 0, 1));
                    CoinsAndHeartsManager.Save(coinsAndHeartsManager);
                }
            }
        }

        private void prevButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            if (posInBuyingOptions == 0)
                posInBuyingOptions = buyingOptions.Count() - 1;
            else
                posInBuyingOptions--;
            updateOptionTexture();
        }

        private void nextButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            posInBuyingOptions = (posInBuyingOptions + 1) % buyingOptions.Count();
            updateOptionTexture();
        }
        #endregion

        #region other functions


        private void updateOptionTexture()
        {
            _components[1].changeTexture(buyingOptions[posInBuyingOptions]);
        }// update the texture of the button

        private void notEnoghMoneyWarning()
        {
            isWaiting = 1;
        }// show no enough money warning
        #endregion
    }
}
