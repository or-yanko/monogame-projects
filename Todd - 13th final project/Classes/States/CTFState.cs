using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Todd.Managers;
using Todd.Moduls;
using Todd.Sprites;

namespace Todd.States
{
    class CTFState: State
    {
        #region data

        //font for text
        SpriteFont btnTextFont;

        //hearts and coins
        CoinsAndHeartsManager coinsAndHeartsManager;

        //array of all the components
        private List<Button> _components;

        //background picture
        private Texture2D background;

        //list of ctfs
        List<CTF> ctflist;

        //finish textures
        Texture2D findThePassCtfButtonTexture2;
        Texture2D otherCtfButtonTexture2;

        //strings
        string findThePassCtfButtonStr = "";
        string otherCtfButtonStr = "";

        //note texture
        Texture2D noteTexture;

        //which btn to insert
        int ctfInsertNum = 0;

        #endregion

        #region constaructor
        public CTFState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //load coinsAndHeartsManager from file
            coinsAndHeartsManager = new CoinsAndHeartsManager();
            coinsAndHeartsManager = CoinsAndHeartsManager.Load();

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //font for the text (i dont use it because i dont write text on the button)
             btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle toGitHubButton button
            var toGitHubButton = new Button(_content.Load<Texture2D>("Buttons/toGitHubBtn"), new Vector2(100, 100),
                Color.Black, Color.White, btnTextFont, "", 100);
            toGitHubButton.Click += toGitHubButtonClick;

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //handle otherCtf button
            var otherCtfButton = new Button(_content.Load<Texture2D>("Buttons/otherCtf1btn"), new Vector2(480, 200),
                Color.Black, Color.White, btnTextFont, "", 100);
            otherCtfButton.Click += otherCtfButtonClick;
            otherCtfButtonTexture2 = _content.Load<Texture2D>("Buttons/otherCtf2btn");

            //handle findThePassCtfButton button
            var findThePassCtfButton = new Button(_content.Load<Texture2D>("Buttons/FTPctf1btn"), new Vector2(180, 200),
                Color.Black, Color.White, btnTextFont, "", 75);
            findThePassCtfButton.Click += findThePassCtfButtonClick;
            findThePassCtfButtonTexture2 = _content.Load<Texture2D>("Buttons/FTPctf2btn");


            //create the list
            _components = new List<Button>()
            {
               backButton, toGitHubButton,  findThePassCtfButton, otherCtfButton
            };

            //load the state of the ctf
            ctflist = Load();

            //load note texture
            noteTexture = _content.Load<Texture2D>("Backgrounds/note");

            if (ctflist[1].isDone == true)
            {
                otherCtfButtonStr = "done in \n mm/dd/yyyy: \n" + ctflist[1].date.ToString("MM/dd/yyyy");
                _components[3].changeTexture(otherCtfButtonTexture2);
            }
            if (ctflist[0].isDone == true)
            {
                findThePassCtfButtonStr = "done in \n mm/dd/yyyy: \n" + ctflist[0].date.ToString("MM/dd/yyyy");
                _components[2].changeTexture(findThePassCtfButtonTexture2);
            }
 

        }

        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            spritebatch.Draw(noteTexture, new Vector2(150, 200), Color.White);
            spritebatch.Draw(noteTexture, new Vector2(450, 200), Color.White);
            foreach (var component in _components)
                component.Draw(spritebatch);

            spritebatch.DrawString(btnTextFont, findThePassCtfButtonStr, new Vector2(200, 300), Color.Black);
            spritebatch.DrawString(btnTextFont, otherCtfButtonStr, new Vector2(500, 300), Color.Black);
            coinsAndHeartsManager.Draw(spritebatch, _content);

            spritebatch.End();

        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {
            Save(ctflist);
            
            string ch ="";
            var kb = Keyboard.GetState();
            Keys[] lkb = kb.GetPressedKeys();
            if (lkb.Length>0)
            {
                ch = lkb[0].ToString();
            }

            if (ctfInsertNum == 1 && ctflist[0].isDone==false)
            {
                if(kb.IsKeyDown(Keys.Enter))
                {
                    if(findThePassCtfButtonStr == ">-{You_Got_My_Pass_Greate_job!_U_R_The_GOAT!}-<")
                    {
                        coinsAndHeartsManager.Update(new CoinsAndHearts(5, 37, 11, 0));
                        CoinsAndHeartsManager.Save(coinsAndHeartsManager);

                        ctflist[0].isDone = true;
                        ctflist[0].date = DateTime.UtcNow;
                        findThePassCtfButtonStr = "done in \n mm/dd/yyyy: \n" + DateTime.UtcNow.ToString("dd/MM/yyyy");
                        _components[2].changeTexture(findThePassCtfButtonTexture2);

                    }
                    else
                    {
                        ctflist[0].isDone = false;
                        findThePassCtfButtonStr =  "Wrong Answer.... Try Again!";
                    }
                }
                else if(findThePassCtfButtonStr.Count()<50)
                {
                    findThePassCtfButtonStr += ch;
                    Console.WriteLine(findThePassCtfButtonStr);
                }
            }
            else if(ctfInsertNum == 2&&ctflist[1].isDone == false)
            {
                if (kb.IsKeyDown(Keys.Enter))
                {
                    if (otherCtfButtonStr == "PSS")
                    {
                        coinsAndHeartsManager.Update(new CoinsAndHearts(0, 0, 1, 0));
                        CoinsAndHeartsManager.Save(coinsAndHeartsManager);

                        ctflist[1].isDone = true;
                        ctflist[1].date = DateTime.UtcNow;
                        otherCtfButtonStr = "done in \n mm/dd/yyyy: \n" + DateTime.UtcNow.ToString("dd/MM/yyyy");
                        _components[3].changeTexture(otherCtfButtonTexture2);

                    }
                    else
                    {
                        ctflist[0].isDone = false;
                        otherCtfButtonStr = "Wrong Answer.... Try Again!";
                    }
                }
                else if (otherCtfButtonStr.Count() < 50)
                {
                    otherCtfButtonStr += ch;
                }

            }
            try
            {
                Save(ctflist);
            }
            catch
            { }
            foreach (var component in _components)
                component.Update(gametime);
        }
        #endregion


        #region buttons click actions

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Back button pressed, Moving to How To Play Window");
            _game.ChangeState(new GameMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }

        private void toGitHubButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            System.Diagnostics.Process.Start("https://github.com/or-yanko/my-ctf");
        }

        private void findThePassCtfButtonClick(object sender, EventArgs e)
        {
            if(ctflist[0].isDone == true)
            {
                findThePassCtfButtonStr = "done in \n mm/dd/yyyy: \n" + ctflist[0].date.ToString("MM/dd/yyyy");
                _components[2].changeTexture(findThePassCtfButtonTexture2);
            }
            else
            {
                btnClickSoundAction();
                findThePassCtfButtonStr = "";
                ctfInsertNum = 1;

            }
        }

        private void otherCtfButtonClick(object sender, EventArgs e)
        {
            if (ctflist[1].isDone == true)
            {
                otherCtfButtonStr = "done in \n mm/dd/yyyy: \n" + ctflist[1].date.ToString("MM/dd/yyyy");
                _components[3].changeTexture(otherCtfButtonTexture2);
            }
            else
            {
                btnClickSoundAction();
                otherCtfButtonStr = "";
                ctfInsertNum = 2;

            }
        }

        #endregion

        #region other functions

        public static List<CTF> Load()
        {
            string FileName = "ctfsState.xml";
            if (!File.Exists(FileName))
            {
                var lst = new List<CTF>() { new CTF("find the pass ctf"), new CTF("other ctf") };
                Save(lst);
                return lst;
            }
            using (var reader = new StreamReader(new FileStream(FileName, FileMode.Open)))
            {
                var serilizer = new XmlSerializer(typeof(List<CTF>));
                var ctfs_ = (List<CTF>)serilizer.Deserialize(reader);
                return ctfs_;
            }

        }//return the ctf list from the xml file


        public static void Save(List<CTF> data)
        {
            string FileName = "ctfsState.xml";
            using (var writer = new StreamWriter(new FileStream(FileName, FileMode.Create)))
            {
                var serilizer = new XmlSerializer(typeof(List<CTF>));
                serilizer.Serialize(writer, data);
            }

        }//save the last day he plyed the game to the xml file

        #endregion

    }
}
