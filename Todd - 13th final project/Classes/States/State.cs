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
    public abstract class State
    {

        #region data
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;
        protected Dictionary<string, Song> _soundsDict;
        protected GraphicsDeviceManager graphics;
        private Game1 game;
        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        private GraphicsDeviceManager graphics_;
        private Dictionary<string, Song> sounds;

        #endregion

        #region Constructor
        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            _soundsDict = sounds;
            graphics = graphics_;
        }

        #endregion

        #region update draw and postUpdate
        public abstract void Draw(GameTime gametime, SpriteBatch spritebatch);
        public abstract void postUpdate(GameTime gametime);
        public abstract void Update(GameTime gametime);

        #endregion


        #region help functions
        protected void btnClickSoundAction()
        {
            MediaPlayer.Play(_soundsDict["btnClick"]);
            Thread.Sleep(500);
        }//make sound when button clicked
        #endregion
    }
}
