using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Codename___Slash
{
    // State of the keyboard button when interacted with 
    public enum eButtonState
    {
        NONE = 0,
        DOWN,
        UP,
        PRESSED
    }

    // State of the Mouse button when interacted with 
    public enum MouseButton
    {
        NONE = 0x00,
        LEFT = 0x01,
        RIGHT = 0x02,
        MIDDLE = 0x04,
        XBUTTON1 = 0x08,
        XBUTTON2 = 0x10,
    }

    // Scroll enum whether you scroll up or down
    public enum Scroll { UP, DOWN }

    // 
    public class CommandManager
    {
        private InputListener inputListener; // Listens for input

        // Dictionaries matching a given input to a delegate method that will be invoked on input
        private Dictionary<Keys, Action<eButtonState, Vector2>> m_keyBindings = new Dictionary<Keys, Action<eButtonState, Vector2>>();
        private Dictionary<MouseButton, Action<eButtonState, Vector2>> m_MouseButtonBindings = new Dictionary<MouseButton, Action<eButtonState, Vector2>>();
        private Dictionary<Scroll, Action<eButtonState, Vector2>> m_scrollBindings = new Dictionary<Scroll, Action<eButtonState, Vector2>>();

        public CommandManager()
        {
            inputListener = new InputListener();

            //
            // Attach all relevant methods to the inputListener's events

            inputListener.OnKeyDown += OnkeyDown;
            inputListener.OnKeyPressed += OnKeyPressed;
            inputListener.OnKeyUp += OnKeyUp;

            inputListener.OnButtonDown += OnMouseDown;
            inputListener.OnButtonPressed += OnMousePressed;
            inputListener.OnButtonUp += OnMouseUp;

            inputListener.OnScroll += OnScroll;

        }

        // 
        public void Update()
        {
            inputListener.Update();
        }

        #region Key Press Evaluation

        public void OnkeyDown(Keys key)
        {
            // Find appropriate action from the dictionary
            Action<eButtonState, Vector2> action = m_keyBindings[key];
            // Invoke all methods subscribed to action
            action?.Invoke(eButtonState.DOWN, new Vector2(1.0f));
        }

        public void OnKeyUp(Keys key)
        {
            // Find appropriate action from the dictionary
            Action<eButtonState, Vector2> action = m_keyBindings[key];
            // Invoke all methods subscribed to action
            action?.Invoke(eButtonState.UP, new Vector2(1.0f));
        }

        public void OnKeyPressed(Keys key)
        {
            // Find appropriate action from the dictionary
            Action<eButtonState, Vector2> action = m_keyBindings[key];
            // Invoke all methods subscribed to action
            action?.Invoke(eButtonState.PRESSED, new Vector2(1.0f));
        }

        public void AddKeyboardBinding(Keys key, Action<eButtonState, Vector2> action)
        {
            // Add key to listen for when polling            
            inputListener.AddKey(key);
            // Add the binding to the command map            
            m_keyBindings.Add(key, action);
        }

        #endregion

        #region Button (Mouse) Press Evaluation 

        public void OnMouseDown(MouseButton button)
        {
            Action<eButtonState, Vector2> action = m_MouseButtonBindings[button];
            action?.Invoke(eButtonState.DOWN, new Vector2(1.0f));
        }

        public void OnMouseUp(MouseButton button)
        {
            Action<eButtonState, Vector2> action = m_MouseButtonBindings[button];
            action?.Invoke(eButtonState.UP, new Vector2(1.0f));
        }

        public void OnMousePressed(MouseButton button)
        {
            Action<eButtonState, Vector2> action = m_MouseButtonBindings[button];
            action?.Invoke(eButtonState.PRESSED, new Vector2(1.0f));
        }
        
        public void OnScroll(Scroll scroll)
        {
            if(m_scrollBindings.Count != 0)
            {

                Action<eButtonState, Vector2> action = m_scrollBindings[scroll];
                action?.Invoke(eButtonState.NONE, new Vector2(1.0f));
            }
        }

        public void AddMouseBinding(MouseButton button, Action<eButtonState, Vector2> action)
        {
            // Add key to listen for when polling            
            inputListener.AddButton(button);
            // Add the binding to the command map            
            m_MouseButtonBindings.Add(button, action);
        }

        public void AddScrollBinding(Scroll scroll, Action<eButtonState, Vector2> action)
        {
            inputListener.AddScroll(scroll);

            m_scrollBindings.Add(scroll, action);
        }
        
        #endregion

    }
}
