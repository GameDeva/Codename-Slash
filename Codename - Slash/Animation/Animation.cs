﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public class Animation
    {
        public Texture2D SpriteStrip { get; }
        public float FrameTime { get; } // Duration of time for each frame
        public int FrameWidth {  get { return SpriteStrip.Width / FrameCount; } }    // TODO: Splice width with number of sprites
        public int FrameHeight { get { return SpriteStrip.Height; } }   
        public bool IsLooping { get;  }
        public int FrameCount;

        
        public Animation(Texture2D texture, int frameCount, float frameTime, bool isLooping)
        {
            SpriteStrip = texture;
            FrameTime = frameTime;
            IsLooping = isLooping;
            FrameCount = frameCount;
        }


    }
}
