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
using Todd.Sprites;

namespace Todd.States
{
    public class miniGameMenuState : State
    {
        #region data

        //array of all the components
        private List<Button> components;

        //background picture
        private Texture2D background;

        //hearts and coins
        CoinsAndHeartsManager coinsAndHeartsManager;

        #endregion

        #region constaructor
        public miniGameMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //load coinsAndHeartsManager from file
            coinsAndHeartsManager = new CoinsAndHeartsManager();
            coinsAndHeartsManager = CoinsAndHeartsManager.Load();

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle the slotMachine button
            var slotMachineButton = new Button(_content.Load<Texture2D>("Buttons/slotMachineBtn"), new Vector2(330, 210),
                Color.Black, Color.White, btnTextFont, "", 100);
            slotMachineButton.Click += slotMachineButtonClick;

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //handle tictactoeButton button
            var tictactoeButton = new Button(_content.Load<Texture2D>("Buttons/TTTBtn"), new Vector2(330, 300),
                Color.Black, Color.White, btnTextFont, "", 100);
            tictactoeButton.Click += tictactoeButtonClick;


            //create the list
            components = new List<Button>()
            {
               slotMachineButton, backButton , tictactoeButton
            };
        }


        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin(); 
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (var component in components)
                component.Draw(spritebatch);

            coinsAndHeartsManager.Draw(spritebatch, _content);
            spritebatch.End();
        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {
            foreach (var component in components)
                component.Update(gametime);
        }
        #endregion

        #region buttons click actions

        private void tictactoeButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict,
            new TicTacToeState(_game, _graphicsDevice, _content, graphics, _soundsDict), 2f));

        }

        private void slotMachineButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("slot machine button pressed, Moving to How To slot machine Window");
            _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict,
            new SlotMachineState(_game, _graphicsDevice, _content, graphics, _soundsDict), 2f));

        }//go to slot machine window

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Back button pressed, Moving to How To Play Window");
            _game.ChangeState(new GameMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//back to main menu
        #endregion




    }
}
