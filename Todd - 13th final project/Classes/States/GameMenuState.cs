using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Todd.Managers;
using Todd.Sprites;

namespace Todd.States
{
    public class GameMenuState : State
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
        public GameMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //load coinsAndHeartsManager from file
            coinsAndHeartsManager = new CoinsAndHeartsManager();
            coinsAndHeartsManager = CoinsAndHeartsManager.Load();

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle the story mode button
            var storyModeButton = new Button(_content.Load<Texture2D>("Buttons/storyModeBtn"), new Vector2(200, 210),
                Color.Black, Color.White, btnTextFont, "", 100);
            storyModeButton.Click += storyModeButtonClick;

            //handle the machine learning button
            var survivalButton = new Button(_content.Load<Texture2D>("Buttons/survivalBtn"), new Vector2(450, 210),
                Color.Black, Color.White, btnTextFont, "", 100);
            survivalButton.Click += survivalButtonClick;

            //handle the 1 vs 1 offline button
            var _1vs1OfflineButton = new Button(_content.Load<Texture2D>("Buttons/1vs1OfflineBtn"), new Vector2(200, 295),
                Color.Black, Color.White, btnTextFont, "", 100);
            _1vs1OfflineButton.Click += _1vs1OfflineButtonClick;

            //handle the 1 vs 1 online button
            var _1vs1OnlineButton = new Button(_content.Load<Texture2D>("Buttons/1vs1OnlineBtn"), new Vector2(450, 295),
                Color.Black, Color.White, btnTextFont, "", 100);
            _1vs1OnlineButton.Click += _1vs1OnlineButtonClick;

            //handle the CTF button
            var ctfButton = new Button(_content.Load<Texture2D>("Buttons/CTFbtn"), new Vector2(200, 380),
                Color.Black, Color.White, btnTextFont, "", 100);
            ctfButton.Click += ctfButtonClick;

            //handle the mini games button
            var miniGamesButton = new Button(_content.Load<Texture2D>("Buttons/miniGamesBtn"), new Vector2(450, 380),
                Color.Black, Color.White, btnTextFont, "", 100);
            miniGamesButton.Click += miniGamesButtonClick;

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;


            //create the list
            components = new List<Button>()
            {
               storyModeButton, _1vs1OfflineButton,_1vs1OnlineButton,survivalButton,ctfButton,miniGamesButton, backButton
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

        private void miniGamesButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("miniGames button pressed, Moving to How To minigame menu Window");
            _game.ChangeState(new miniGameMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//go to the minigame page

        private void ctfButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new CTFState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//go to the ctf page

        private void _1vs1OnlineButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new ComingSoonState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//start 1 vs 1 online mode

        private void _1vs1OfflineButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new ComingSoonState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//start 1 vs 1 offline mode

        private void survivalButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict, new SurvivalState(_game, _graphicsDevice, _content, graphics, _soundsDict), 4.5f));
        }//start survival mode

        private void storyModeButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new LevelChooseState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//start story mode

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Back button pressed, Moving to How To Play Window");
            _game.ChangeState(new MainMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//back to main menu

        #endregion

    }
}
