using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todd.Sprites
{
    public class Platform
    {
        #region data
        //appirance
        Texture2D texture;
        //positioning and moovement
        Vector2 position;
        public Rectangle rectangle;
        //public Vector2 movmentMaxSpeed = new Vector2(0,0);
        //public Vector2 currSpeed = new Vector2(0,0);
        //public Vector2 startingPoint;
        //public Vector2 acceleration;
        #endregion


        #region Constructor
        public Platform(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;

            rectangle = new Rectangle((int)position.X, (int)position.Y,
                texture.Width, texture.Height);
        }
        public Platform(Texture2D newTexture, Vector2 startPoint, Vector2 movmentMaxSpeed_, Vector2 acceleration_)
        {
            texture = newTexture;
            position = startPoint;
            //movmentMaxSpeed = movmentMaxSpeed_;
            //startingPoint = startPoint;
            //acceleration = acceleration_;


            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        #endregion


        #region update and delete
        public void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
        #endregion
    }

    public class CopyOfPlatform
    {
        #region data
        //appirance
        Texture2D texture;
        //positioning and moovement
        Vector2 position;
        public Rectangle rectangle;
        //public Vector2 movmentMaxSpeed = new Vector2(0,0);
        //public Vector2 currSpeed = new Vector2(0,0);
        //public Vector2 startingPoint;
        //public Vector2 acceleration;
        #endregion


        #region Constructor
        public CopyOfPlatform(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;

            rectangle = new Rectangle((int)position.X, (int)position.Y,
                texture.Width, texture.Height);
        }
        public CopyOfPlatform(Texture2D newTexture, Vector2 startPoint, Vector2 movmentMaxSpeed_, Vector2 acceleration_)
        {
            texture = newTexture;
            position = startPoint;
            //movmentMaxSpeed = movmentMaxSpeed_;
            //startingPoint = startPoint;
            //acceleration = acceleration_;


            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        #endregion


        #region update and delete
        public void Update(GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
        #endregion
    }

}
