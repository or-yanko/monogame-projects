using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Todd.Sprites
{
    class EditTextButton : Button
    {
         public EditTextButton(Texture2D texture, Vector2 position, Color textColor, Color color, SpriteFont font, string text, int precentageSize) :
            base(texture, position, textColor, color, font, text, precentageSize)
        {

        }
    }
}
