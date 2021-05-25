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
    public class MainMenuState : State
    {
        #region data

        //array of all the components
        private List<Button> _components;

        //background picture
        private Texture2D background;

        #endregion

        #region Constructor
        public MainMenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {


            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");
            
            //handle the play button
            var playButton = new Button(_content.Load<Texture2D>("Buttons/playMainMenuBtn"), new Vector2(330, 210),
                Color.Black, Color.White, btnTextFont, "", 100);
            playButton.Click += playButtonClick;

            //handle howToPlayButton button
            var howToPlayButton = new Button(_content.Load<Texture2D>("Buttons/howToPlayMainMenuBtn"), new Vector2(330, 295),
                Color.Black, Color.White, btnTextFont, "", 100);
            howToPlayButton.Click += howToPlayButtonClick;

            //handle scoresButton button
            var scoresButton = new Button(_content.Load<Texture2D>("Buttons/scoresMainMenuBtn"), new Vector2(330, 380)
                , Color.Black, Color.White, btnTextFont, "", 100);
            scoresButton.Click += scoresButtonClick;

            //handle quitGameButton button
            var quitGameButton = new Button(_content.Load<Texture2D>("Buttons/exitBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 20);
            quitGameButton.Click += quitGameButtonClick;

            //handle storeButton button
            var storeButton = new Button(_content.Load<Texture2D>("Buttons/store"), new Vector2(745, 5),
                Color.Black, Color.White, btnTextFont, "", 5);
            storeButton.Click += storeButtonClick;

            //create the list
            _components = new List<Button>()
            {
               playButton,howToPlayButton,scoresButton,quitGameButton, storeButton
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


            spritebatch.End();
        }

        public override void postUpdate(GameTime gametime)
        {
            //remove sprites if they are dont needed
        }

        public override void Update(GameTime gametime)
        {
            foreach (var component in _components)
                component.Update(gametime);
        }
        #endregion

        #region buttons click actions

        private void playButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Play button pressed, Moving to Game Menu");
            _game.ChangeState(new GameMenuState(_game, _graphicsDevice, _content, graphics,_soundsDict));
        }//moves to game menu window

        private void scoresButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Scores button pressed, Moving to How To Play Window");
            _game.ChangeState(new ScoresState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//moves to scores window

        private void howToPlayButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("How to play button pressed, Moving to How To Play Window");
            _game.ChangeState(new HowToPlayState(_game, _graphicsDevice, _content, graphics, _soundsDict));
           // _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict,
             //   new HowToPlayState(_game, _graphicsDevice, _content, graphics, _soundsDict), 2f));
        }//moves to how to play window

        private void quitGameButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Quit Game");
            _game.Exit();
        }//quit the game

        private void storeButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Store button pressed, Moving to How To Store Window");
            _game.ChangeState(new StoreState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//move to the store
        #endregion

    }
}
