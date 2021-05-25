using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Todd.States;

namespace Todd
{
    public class Game1 : Game
    {
        //original
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //size
        public static int screenHieght;
        public static int screenWidth;

        //states (change screens)
        private State _currentState;
        private State _nextState;

        //sounds dictionry
        Dictionary<string, Song> soundsDict;

        public void ChangeState(State state)
        {
            _nextState = state;
        }//function that change the state

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //set screen size in the vars
            screenHieght = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;

            //create the soundsDict
            soundsDict = new Dictionary<string, Song>() {
                {"coin",Content.Load<Song>("Audio/Coin") },
                {"death",Content.Load<Song>("Audio/Death") },
                {"jump",Content.Load<Song>("Audio/Jump") },
                {"btnClick",Content.Load<Song>("Audio/BtnClick") },
                {"mainMenuMusic1",Content.Load<Song>("Audio/MainMenuMusic1") },
                {"mainMenuMusic2",Content.Load<Song>("Audio/MainMenuMusic2") },
                {"mainMenuMusic3",Content.Load<Song>("Audio/MainMenuMusic3") }
            };

            //create the first screen state of the game
            _currentState = new MainMenuState(this, graphics.GraphicsDevice, Content, graphics, soundsDict);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //if the state have changed' it change it!
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            //update and postupdate the state's content
            _currentState.Update(gameTime);
            _currentState.postUpdate(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //clear to whitw screen
            GraphicsDevice.Clear(Color.White);

            //draw the state's content
            _currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
            
        }
    }
}
