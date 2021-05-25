using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    public class SlotMachineState : State
    {
        #region data
        Boolean isAnimation = false;
        int numOfChanges;

        //array of all the components
        private List<Button> components;

        //background picture
        private Texture2D background;

        //hearts and coins
        CoinsAndHeartsManager coinsAndHeartsManager;

        //slot machine textures
        private List<Texture2D> smTextures;
        List<Texture2D> txtr3;
        Texture2D insTexture;

        //slot machine texture
        private Texture2D smTexture;

        //error msg texture
        private Texture2D errorTxtr;

        //won textures list
        List<Texture2D> wonMsgs;

        #endregion

        #region constaructor
        public SlotMachineState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //load coinsAndHeartsManager from file
            coinsAndHeartsManager = new CoinsAndHeartsManager();
            coinsAndHeartsManager = CoinsAndHeartsManager.Load();

            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //load won msgs
            wonMsgs = new List<Texture2D>()
            {
                _content.Load<Texture2D>("WonMsgs/goldCoinWon"),
                _content.Load<Texture2D>("WonMsgs/silverCoinsWon"),
                _content.Load<Texture2D>("WonMsgs/bronzeCoinsWon")
            };

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //handle backButton button
            var clickHereButton = new Button(_content.Load<Texture2D>("Buttons/clickHereBtn"), new Vector2(380, 380),
                Color.Black, Color.White, btnTextFont, "", 100);
            clickHereButton.Click += clickHereButtonClick;

            //create the list
            components = new List<Button>()
            {
               backButton, clickHereButton
            };

            //create the list
            smTextures = new List<Texture2D>()
            {
               _content.Load<Texture2D>("SlotMachine/banana"),
               _content.Load<Texture2D>("SlotMachine/bar"),
               _content.Load<Texture2D>("SlotMachine/bell"),
               _content.Load<Texture2D>("SlotMachine/cherry"),
               _content.Load<Texture2D>("SlotMachine/grape"),
               _content.Load<Texture2D>("SlotMachine/lemon"),
               _content.Load<Texture2D>("SlotMachine/plum"),
               _content.Load<Texture2D>("SlotMachine/seven"),

            };

            //handle slot machine texture
            smTexture = _content.Load<Texture2D>("SlotMachine/slotMachine");
            txtr3 = new List<Texture2D> { smTextures[0], smTextures[0], smTextures[0] };
            insTexture = _content.Load<Texture2D>("Instructions/slotMachimeInstructions"); //slotMachimeInstructions

            //handle error texture
            errorTxtr = _content.Load<Texture2D>("Errors/onceADayError");

        }


        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {

            //if the animation of the slot machine is off
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            spritebatch.Draw(smTexture, new Vector2(300, 200), Color.White);
            spritebatch.Draw(txtr3[0], new Vector2(320, 240), Color.White);
            spritebatch.Draw(txtr3[1], new Vector2(420, 240), Color.White);
            spritebatch.Draw(txtr3[2], new Vector2(520, 240), Color.White);
            spritebatch.Draw(insTexture, new Vector2(15, 100), Color.White);
            foreach (var component in components)
                component.Draw(spritebatch);
            coinsAndHeartsManager.Draw(spritebatch, _content);

            spritebatch.End();

            //handle animation
            if (isAnimation == true)
            {
                DateTime date = Load();
                DateTime curr = DateTime.UtcNow;
                Random rand = new Random();
                
                if (date.Day == curr.Day && date.Month == curr.Month && date.Year == curr.Year)
                {
                    if (numOfChanges == 0)
                        handleErrorMsg(spritebatch, gametime);
                    else
                    {
                        Thread.Sleep(4000);//sleep 
                        isAnimation = false;
                    }
                }
                else
                {
                    if (numOfChanges == 0)
                        txtr3 = new List<Texture2D> { smTextures[0], smTextures[0], smTextures[0] };

                    if (numOfChanges < 20)
                    {
                        Thread.Sleep(200);//sleep 
                        int num1 = rand.Next(0, 8);
                        int num2 = rand.Next(0, 8);
                        int num3 = rand.Next(0, 8);
                        txtr3 = new List<Texture2D> { smTextures[num1], smTextures[num2], smTextures[num3] };


                    }
                    else if(numOfChanges == 20)
                    {
                        //update prize
                        if (txtr3[0] == txtr3[1] && txtr3[0] == txtr3[2])
                        {
                            coinsAndHeartsManager.Update(new CoinsAndHearts(1, 0, 0, 0));
                            CoinsAndHeartsManager.Save(coinsAndHeartsManager);
                            spritebatch.Begin();
                            spritebatch.Draw(wonMsgs[0], new Vector2(270, 50), Color.White);
                            spritebatch.End();

                        }
                        else if (txtr3[0] == txtr3[1] || txtr3[0] == txtr3[2] || txtr3[2] == txtr3[1])
                        {
                            coinsAndHeartsManager.Update(new CoinsAndHearts(0, 5, 0, 0));
                            CoinsAndHeartsManager.Save(coinsAndHeartsManager);
                            spritebatch.Begin();
                            spritebatch.Draw(wonMsgs[1], new Vector2(270, 50), Color.White);
                            spritebatch.End();

                        }
                        else
                        {
                            coinsAndHeartsManager.Update(new CoinsAndHearts(0, 0, 10, 0));
                            CoinsAndHeartsManager.Save(coinsAndHeartsManager);
                            spritebatch.Begin();
                            spritebatch.Draw(wonMsgs[2], new Vector2(270, 50), Color.White);
                            spritebatch.End();

                        }
                    }
                    else 
                    {
                        Thread.Sleep(4000);//sleep 
                        Save(curr);
                        isAnimation = false;
                            
                    }
                }
                numOfChanges++;
            }

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

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            Console.WriteLine("Back button pressed, Moving to minigame main menu Window");
            _game.ChangeState(new miniGameMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//back to main menu

        private void clickHereButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            isAnimation = true;
            numOfChanges = 0;
        }//Turns on the slot machine
    #endregion

        #region other functions

        private void handleErrorMsg(SpriteBatch sb, GameTime gametime)
        {
            //error msg
            sb.Begin();
            sb.Draw(errorTxtr, new Vector2(300, 50), Color.White);
            sb.End();
        }//handle the error massage

        public static DateTime Load()
        {
            string FileName = "slotMachineFile.xml";
            if (!File.Exists(FileName))
                return new DateTime(2000,1,1);
            using (var reader = new StreamReader(new FileStream(FileName, FileMode.Open)))
            {
                var serilizer = new XmlSerializer(typeof(DateTime));
                var cnh = (DateTime)serilizer.Deserialize(reader);
                return cnh;
            }

        }//return the last day he plyed the game from the xml file

        public static void Save(DateTime date)
        {
            string FileName = "slotMachineFile.xml";
            using (var writer = new StreamWriter(new FileStream(FileName, FileMode.Create)))
            {
                var serilizer = new XmlSerializer(typeof(DateTime));
                serilizer.Serialize(writer, date);
            }

        }//save the last day he plyed the game to the xml file

        #endregion

    }
}
