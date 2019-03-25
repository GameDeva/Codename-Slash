using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Codename___Slash
{
    class InputListener
    {
        public Action<Keys> OnKeyDown;
        public Action<Keys> OnKeyPressed;
        public Action<Keys> OnKeyUp;

        public Action<MouseButton> OnButtonDown;
        public Action<MouseButton> OnButtonPressed;
        public Action<MouseButton> OnButtonUp;

        public Action<Scroll> OnScroll;

        private KeyboardState PrevKeyboardState { get; set; }
        private KeyboardState CurrentKeyboardState { get; set; }

        private MouseState PrevMouseState { get; set; }
        private MouseState CurrentMouseState { get; set; }

        private int previousScrollVal;

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
            int newScrollVal = Mouse.GetState().ScrollWheelValue;
            if (newScrollVal > previousScrollVal) { OnScroll?.Invoke(Scroll.UP); previousScrollVal = newScrollVal; }
            if (newScrollVal < previousScrollVal) { OnScroll?.Invoke(Scroll.DOWN); previousScrollVal = newScrollVal; }
        }

    }

    public enum Scroll { UP, DOWN }
}
