using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class Camera
    {
        public static Matrix Transform { get; private set; }

        public Camera()
        {
        }


        // TODO: Change target to Living Entity parent class so camera can also follow 
        // Used to follow the player around during gameplay state
        public static Vector2 Follow(Hero target)
        {
            // TODO: Could be moved to initialise, if there is not real time screen resizing option. 
            // Offset created based on aspect ratio of the screen
            Matrix offset = Matrix.CreateTranslation(Game1.SCREENWIDTH / 2, Game1.SCREENHEIGHT / 2, 0);

            // Position of the camera created 
            Matrix position = Matrix.CreateTranslation(
                -target.Position.X - (target.Animator.Animation.FrameWidth / 2), 
                -target.Position.Y - (target.Animator.Animation.FrameHeight / 2),
                0);

            // Assign transform of camera
            Transform = position * offset;

            return Vector2.Transform(target.Position, Transform);
        }

        public static Vector2 UpdateMousePos(MouseState mouse)
        {
            return Vector2.Transform(new Vector2(mouse.Position.X, mouse.Position.Y), Transform);
        }
    }
}
