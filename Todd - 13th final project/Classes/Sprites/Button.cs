using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todd.Sprites
{
    public class Button
    {
        #region data
        //appirance
        SpriteFont BtnFont;
        public String text;
        Color PenColor;
        public Texture2D texture;
        Color color;
        int precentageSize;

        //button states
        private MouseState currentMouseState;
        private bool isHovering;
        private MouseState previousMouseState;
        public event EventHandler Click;
        public bool Clicked { get; private set; }

        //position
        public Vector2 position;
        #endregion

        #region constractor
        public Button(Texture2D texture, Vector2 position, Color textColor, Color color, SpriteFont font, string text, int precentageSize)
        {
            this.precentageSize = precentageSize;
            this.position = position;
            this.color = color;
            this.BtnFont = font;
            this.text = text;
            this.texture = texture;
            this.PenColor = textColor;
        }
        #endregion

        #region rectangle creation

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                    this.texture.Width * precentageSize / 100, this.texture.Height * precentageSize / 100);
            }
        }
        #endregion

        #region update and draw

        public void Draw(SpriteBatch spriteBatch)
        {
            var color_ = this.color;
            var textColor_ = this.PenColor;

            if (isHovering)
            {
                color_ = Color.Gray;
                textColor_ = Color.Red;
            }

            spriteBatch.Draw(this.texture, Rectangle, color_);

            if (!string.IsNullOrEmpty(text))
            {
                var x = ((int)position.X + (Rectangle.Width / 2) - (BtnFont.MeasureString(text).X / 2));
                var y = ((int)position.Y + (Rectangle.Height / 2) - (BtnFont.MeasureString(text).Y / 2));

                spriteBatch.DrawString(BtnFont, text, new Vector2(x, y), textColor_, 0,
                    Vector2.Zero, (float)((float)precentageSize / 110.0),
                    SpriteEffects.None, 0);
            }

        }

        public void Update(GameTime gameTime)
        {

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);



            isHovering = false;
            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouseState.LeftButton == ButtonState.Released &&
                    previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs()); // equal to: if(Click != null) Click(this, new EventArgs();

                }
            }

        }

        #endregion

        #region other functions

        public void changeTexture(Texture2D txtr)
        {
            this.texture = txtr;
        }

        #endregion
    }
}
