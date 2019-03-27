using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Codename___Slash
{
    public class Animator
    {
        public Animation Animation { get; private set; }
        public int FrameIndex { get; private set; }
        public Vector2 Origin { get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight / 2.0f); } }


        private float time;

        public void AttachAnimation(Animation animation)
        {
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;

            // Start the new animation.
            Animation = animation;
            FrameIndex = 0;
            time = 0.0f;
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, Color? color = null)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            // Process passing time.
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    FrameIndex = (FrameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    FrameIndex = Math.Min(FrameIndex + 1, Animation.FrameCount - 1);
                }
            }

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight);

            // Draw the current frame.
            // spriteBatch.Draw(Animation.SpriteStrip, position, source, Color.White);
            spriteBatch.Draw(Animation.SpriteStrip, position, source, color.GetValueOrDefault(Color.White), 0.0f, Origin, 2.0f, spriteEffects, 0.0f);
        }


    }
}
