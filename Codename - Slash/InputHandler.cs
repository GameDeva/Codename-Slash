using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Codename___Slash
{
    public class InputHandler
    {
        private Command wButton;
        private Command aButton;
        private Command sButton;
        private Command dButton;
        private Command spaceButton;

        // TODO: methods to bind the commands
        public InputHandler()
        {
            wButton = new MoveUpCommand();
            aButton = new MoveLeftCommand();
            sButton = new MoveDownCommand();
            dButton = new MoveRightCommand();
            spaceButton = new DashCommand();
        }


        public Command HandleInput()
        {
            // TODO: Check for controller input 
            // TODO: maybe, pass in a sensitvity parameter on how hard the button is pressed. 


            if (Keyboard.GetState().IsKeyDown(Keys.W)) return wButton;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) return sButton;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) return aButton;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) return dButton;
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) return spaceButton;

            return null; // If nothing is pressed
        }

    }
}
