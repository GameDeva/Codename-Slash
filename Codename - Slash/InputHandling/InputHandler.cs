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

        private Command leftMouse;
        private Command scrollUp;
        private Command scrollDown;

        private List<Command> currentCommandsList;

        private int previousScrollVal;

        // TODO: methods to bind the commands
        public InputHandler()
        {
            currentCommandsList = new List<Command>();

            wButton = new MoveUpCommand();
            aButton = new MoveLeftCommand();
            sButton = new MoveDownCommand();
            dButton = new MoveRightCommand();
            spaceButton = new DashCommand();
            
            leftMouse = new ShootCommand();
            scrollUp = new PreviousWeaponCommand();
            scrollDown = new NextWeaponCommand();

        }


        public List<Command> HandleInput()
        {
            // TODO: Check for controller input 
            // TODO: maybe, pass in a sensitvity parameter on how hard the button is pressed. 
            // Clear previous list of commands pressed
            currentCommandsList.Clear();

            // Add any commands issued this frame to the list
            //if (Keyboard.GetState().IsKeyDown(Keys.W)) currentCommandsList.Add(wButton);
            //if (Keyboard.GetState().IsKeyDown(Keys.S)) currentCommandsList.Add(sButton);
            //if (Keyboard.GetState().IsKeyDown(Keys.A)) currentCommandsList.Add(aButton);
            //if (Keyboard.GetState().IsKeyDown(Keys.D)) currentCommandsList.Add(dButton);
            //if (Keyboard.GetState().IsKeyDown(Keys.Space)) currentCommandsList.Add(spaceButton);

            // Mouse input
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) currentCommandsList.Add(leftMouse);

            // Scroll input
            int newScrollVal = Mouse.GetState().ScrollWheelValue;
            if (newScrollVal > previousScrollVal) { currentCommandsList.Add(scrollUp); previousScrollVal = newScrollVal; }
            if (newScrollVal < previousScrollVal) { currentCommandsList.Add(scrollDown); previousScrollVal = newScrollVal; }

            return currentCommandsList; 
        }

    }
}
