using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Todd.Moduls;

namespace Todd.Managers
{
    public class CoinsAndHeartsManager
    {
        public CoinsAndHearts curr;
        private static string FileName = "coinsAndHearts.xml";

        #region constructor
        public CoinsAndHeartsManager() :
            this(new CoinsAndHearts()) { }  //empty constractor that wntwe new list to the constractor
        public CoinsAndHeartsManager(CoinsAndHearts curr)
        {
            this.curr = curr;
        }

        #endregion

        #region load and save 

        public static CoinsAndHeartsManager Load()
        {
            if (!File.Exists(FileName))
                return new CoinsAndHeartsManager();
            using (var reader = new StreamReader(new FileStream(FileName, FileMode.Open)))
            {
                var serilizer = new XmlSerializer(typeof(CoinsAndHearts));
                var cnh = (CoinsAndHearts)serilizer.Deserialize(reader);
                return new CoinsAndHeartsManager(cnh);
            }

        }//return the scores from the xml file

        public static void Save(CoinsAndHeartsManager coinsAndHeartsManager)
        {
            using (var writer = new StreamWriter(new FileStream(FileName, FileMode.Create)))
            {
                var serilizer = new XmlSerializer(typeof(CoinsAndHearts));
                serilizer.Serialize(writer, coinsAndHeartsManager.curr);
            }

        }//save the list of scores to the xml file

        #endregion

        #region update and draw

        public void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            //draw heart and 3 coins
            spriteBatch.Draw(content.Load<Texture2D>("Components/heart"), new Vector2(100, 5), null, Color.White, 0, new Vector2(0, 0), 0.05f, SpriteEffects.None, 0 );
            spriteBatch.Draw(content.Load<Texture2D>("Components/goldCoin"), new Vector2(180, 5), null, Color.White, 0, new Vector2(0, 0), 0.04f, SpriteEffects.None, 0);
            spriteBatch.Draw(content.Load<Texture2D>("Components/silverCoin"), new Vector2(260, 5), null, Color.White, 0, new Vector2(0, 0), 0.04f, SpriteEffects.None, 0);
            spriteBatch.Draw(content.Load<Texture2D>("Components/bronzeCoin"), new Vector2(340, 5), null, Color.White, 0, new Vector2(0, 0), 0.04f, SpriteEffects.None, 0);

            //draw the sum of each of them
            SpriteFont btnTextFont = content.Load<SpriteFont>("Fonts/font");

            spriteBatch.DrawString(btnTextFont, this.curr.hearts.ToString(), new Vector2(130, 10), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(btnTextFont, this.curr.goldCoins.ToString(), new Vector2(210, 10), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(btnTextFont, this.curr.silverCoins.ToString(), new Vector2(290, 10), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(btnTextFont, this.curr.bronzeCoins.ToString(), new Vector2(370, 10), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

        }

        public void Update(CoinsAndHearts cnh)
        {
            curr.hearts += cnh.hearts;
            curr.goldCoins += cnh.goldCoins;
            curr.silverCoins += cnh.silverCoins;
            curr.bronzeCoins += cnh.bronzeCoins;

        }


        #endregion
    }
}
