using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todd.Moduls
{
    public class Animation
    {
        public int CurrentFrame { get; set; }

        public int FrameCount { get; private set; }

        public float FrameSpeed { get; set; }

        public bool IsLooping { get; set; }

        public List<Texture2D> Textures { get; private set; }

        public Animation(List<Texture2D> texture, int frameCount, float changePhotoSpeed)
        {
            Textures = texture;

            FrameCount = frameCount;

            IsLooping = true;

            FrameSpeed = changePhotoSpeed;
        }
    }

}
