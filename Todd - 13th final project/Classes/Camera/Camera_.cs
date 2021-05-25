using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todd.Sprites;

namespace Todd.Camera
{
    public class Camera_
    {
        public Matrix transform { get; private set; }
        private float zoom = 1;
        public void Follow(Sprite target)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                setZoom(getZoom() * 1.01f);
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                setZoom(getZoom() * 0.99f);

            var offset = Matrix.CreateTranslation(Game1.screenWidth / 2, Game1.screenHieght / 2, 0);
            var position = Matrix.CreateTranslation(
                -target._position.X - (target.rectangle.Width / 2),
                -target._position.Y - (target.rectangle.Height / 2), 0) * Matrix.CreateScale(zoom, zoom, 0);
            transform = position * offset;
        }
        public void setZoom(float zoom)
        {
            if (zoom < 0.3f)
                this.zoom = 0.3f;
            else if (zoom > 2f)
                this.zoom = 2f;
            else
                this.zoom = zoom;
        }
        public float getZoom()
        {
            return this.zoom;
        }
    }

}
