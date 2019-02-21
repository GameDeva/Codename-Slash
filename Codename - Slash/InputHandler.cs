﻿using System;
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

        private List<Command> currentCommandsList;

        // TODO: methods to bind the commands
        public InputHandler()
        {
            currentCommandsList = new List<Command>();

            wButton = new MoveUpCommand();
            aButton = new MoveLeftCommand();
            sButton = new MoveDownCommand();
            dButton = new MoveRightCommand();
            spaceButton = new DashCommand();
        }


        public List<Command> HandleInput()
        {
            // TODO: Check for controller input 
            // TODO: maybe, pass in a sensitvity parameter on how hard the button is pressed. 
            // Clear previous list of commands pressed
            currentCommandsList.Clear();

            // Add any commands issued this frame to the list
            if (Keyboard.GetState().IsKeyDown(Keys.W)) currentCommandsList.Add(wButton);
            if (Keyboard.GetState().IsKeyDown(Keys.S)) currentCommandsList.Add(sButton);
            if (Keyboard.GetState().IsKeyDown(Keys.A)) currentCommandsList.Add(aButton);
            if (Keyboard.GetState().IsKeyDown(Keys.D)) currentCommandsList.Add(dButton);
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) currentCommandsList.Add(spaceButton);
            
            return currentCommandsList; 
        }

    }
}
