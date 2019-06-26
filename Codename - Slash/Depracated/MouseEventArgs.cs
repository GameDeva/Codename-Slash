using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Codename___Slash.Depracated
{
    public class MouseEventArgs : EventArgs
    {
        public MouseEventArgs(MouseButton button, MouseState currentState, MouseState prevState)
        {
            CurrentState = currentState;
            PrevState = prevState;
            Button = button;
        }

        public readonly MouseState CurrentState;
        public readonly MouseState PrevState;
        public readonly MouseButton Button;
    }
}
