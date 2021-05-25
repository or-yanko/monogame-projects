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
using Todd.Sprites;

namespace Todd.States
{
    class HowToPlayState : State
    {
        #region data

        //array of all the components
        private List<Button> _components;

        //background picture
        private Texture2D background;

        //instructionPicture
        Texture2D instructionTxtr;

        #endregion

        #region constaructor
        public HowToPlayState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {

            //first instruction
            instructionTxtr = _content.Load<Texture2D>("Instructions/firstInstructions");

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //handle the story mode button
            var storyModeButton = new Button(_content.Load<Texture2D>("Buttons/storyModeBtn"), new Vector2(60, 40),
                Color.Black, Color.White, btnTextFont, "", 100);
            storyModeButton.Click += storyModeButtonClick;

            //handle the 1 vs 1 offline button
            var _1vs1OfflineButton = new Button(_content.Load<Texture2D>("Buttons/1vs1OfflineBtn"), new Vector2(60, 125),
                Color.Black, Color.White, btnTextFont, "", 100);
            _1vs1OfflineButton.Click += _1vs1OfflineButtonClick;

            //handle the 1 vs 1 online button
            var _1vs1OnlineButton = new Button(_content.Load<Texture2D>("Buttons/1vs1OnlineBtn"), new Vector2(60, 210),
                Color.Black, Color.White, btnTextFont, "", 100);
            _1vs1OnlineButton.Click += _1vs1OnlineButtonClick;

            //handle the CTF button
            var ctfButton = new Button(_content.Load<Texture2D>("Buttons/CTFbtn"), new Vector2(60, 295),
                Color.Black, Color.White, btnTextFont, "", 100);
            ctfButton.Click += ctfButtonClick;

            //handle the survival button
            var survivalButton = new Button(_content.Load<Texture2D>("Buttons/survivalBtn"), new Vector2(60, 380),
                Color.Black, Color.White, btnTextFont, "", 100);
            survivalButton.Click += survivalButtonClick;

            //create the list
            _components = new List<Button>()
            {
               storyModeButton, _1vs1OfflineButton,_1vs1OnlineButton,ctfButton, backButton, survivalButton
            };
        }



        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(spritebatch);

            spritebatch.Draw(instructionTxtr, new Vector2(300, 210), Color.White);
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


        private void ctfButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            instructionTxtr = _content.Load<Texture2D>("Instructions/ctfInstructions");
        }//show ctf instructions

        private void _1vs1OnlineButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            instructionTxtr = _content.Load<Texture2D>("Instructions/keyboardSignal&Online");
        }//show 1 vs 1 online instructions

        private void _1vs1OfflineButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            instructionTxtr = _content.Load<Texture2D>("Instructions/keyboardOffline");
        }//show 1 vs 1 offline instructions

        private void storyModeButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            instructionTxtr = _content.Load<Texture2D>("Instructions/keyboardSignal&Online");
        }//start story mode

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Back button pressed, Moving to How To Play Window");
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }

        private void survivalButtonClick(object sender, EventArgs e)//show survival instructions
        {
            btnClickSoundAction();
            instructionTxtr = _content.Load<Texture2D>("Instructions/keyboardSignal&Online");
        }
        #endregion

    }
}
