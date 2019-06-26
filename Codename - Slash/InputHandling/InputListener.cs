using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Codename___Slash
{
    // Checks for any sort of input in the lists of possible inputs, and fires events if there is
    class InputListener
    {
        // Key interaction events
        public Action<Keys> OnKeyDown;
        public Action<Keys> OnKeyPressed;
        public Action<Keys> OnKeyUp;

        // Mouse interaction events
        public Action<MouseButton> OnButtonDown;
        public Action<MouseButton> OnButtonPressed;
        public Action<MouseButton> OnButtonUp;

        // Scroll interaction event
        public Action<Scroll> OnScroll;

        // Previous and current Keyboard states
        private KeyboardState PrevKeyboardState { get; set; }
        private KeyboardState CurrentKeyboardState { get; set; }

        // Previous and current Mouse states
        private MouseState PrevMouseState { get; set; }
        private MouseState CurrentMouseState { get; set; }

        // Previous scroll value to compare with current
        private int previousScrollVal;

        // Hashsets of types of input that should be checked 
        public HashSet<Keys> KeyList;
        public HashSet<MouseButton> buttonsList;
        public HashSet<Scroll> scrollList;

        public InputListener()
        {
            // Initialise Mouse and keyboard states
            CurrentKeyboardState = Keyboard.GetState();
            PrevKeyboardState = CurrentKeyboardState;
            CurrentMouseState = Mouse.GetState();
            PrevMouseState = CurrentMouseState;

            // Create the lists
            KeyList = new HashSet<Keys>();
            buttonsList = new HashSet<MouseButton>();
            scrollList = new HashSet<Scroll>();
        }

        // 
        public void Update()
        {
            PrevKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            FireKeyboardEvents();

            PrevMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            FireMouseEvents();

            FireScrollEvents();
        }

        //
        // Add key, mouse and scroll interactions to hashset that needs to be watched

        public void AddKey(Keys key)
        {
            KeyList.Add(key);
        }

        public void AddButton(MouseButton button)
        {
            buttonsList.Add(button);
        }

        public void AddScroll(Scroll scroll)
        {
            scrollList.Add(scroll);
        }

        //
        // Events that should be fired based on the interaction/input received
        
        private void FireKeyboardEvents()
        {
            foreach (Keys key in KeyList)
            {
                // Is key currently down?
                if (CurrentKeyboardState.IsKeyDown(key))
                {
                    OnKeyDown?.Invoke(key);
                }

                // Has the key been released? 
                if (PrevKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key))
                {
                    OnKeyUp?.Invoke(key);
                }

                // Key has been held 
                if (PrevKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyDown(key))
                {
                    OnKeyPressed?.Invoke(key);
                }
            }
        }

        private void FireMouseEvents()
        {
            foreach (MouseButton button in buttonsList)
            {
                // Is key currently down?
                if (CurrentMouseState.LeftButton == ButtonState.Pressed)
                {
                    OnButtonDown?.Invoke(button);
                }

                // Has the key been released? 
                if (PrevMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released)
                {
                    OnButtonUp?.Invoke(button);
                }

                // Key has been held 
                if (PrevMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Pressed)
                {
                    OnButtonPressed?.Invoke(button);
                }
            }
        }

        private void FireScrollEvents()
        {
            // Scroll is determined through changing the value of the scroll wheel higher or lower
            int newScrollVal = Mouse.GetState().ScrollWheelValue;
            if (newScrollVal > previousScrollVal) { OnScroll?.Invoke(Scroll.UP); previousScrollVal = newScrollVal; }
            if (newScrollVal < previousScrollVal) { OnScroll?.Invoke(Scroll.DOWN); previousScrollVal = newScrollVal; }
        }

    }

}
