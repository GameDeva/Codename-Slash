using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Codename___Slash
{
    public enum eButtonState
    {
        NONE = 0,
        DOWN,
        UP,
        PRESSED
    }

    public class CommandManager
    {
        private InputListener m_Input;
        private Dictionary<Keys, Action<eButtonState, Vector2>> m_keyBindings = new Dictionary<Keys, Action<eButtonState, Vector2>>();
        private Dictionary<MouseButton, Action<eButtonState, Vector2>> m_MouseButtonBindings = new Dictionary<MouseButton, Action<eButtonState, Vector2>>();
        private Dictionary<Scroll, Action<eButtonState, Vector2>> m_scrollBindings = new Dictionary<Scroll, Action<eButtonState, Vector2>>();

        public CommandManager()
        {
            m_Input = new InputListener();

            m_Input.OnKeyDown += OnkeyDown;
            m_Input.OnKeyPressed += OnKeyPressed;
            m_Input.OnKeyUp += OnKeyUp;

            m_Input.OnButtonDown += OnMouseDown;
            m_Input.OnButtonPressed += OnMousePressed;
            m_Input.OnButtonUp += OnMouseUp;

            m_Input.OnScroll += OnScroll;

        }

        public void Update()
        {
            m_Input.Update();
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
            m_Input.AddKey(key);
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
            m_Input.AddButton(button);
            // Add the binding to the command map            
            m_MouseButtonBindings.Add(button, action);
        }

        public void AddScrollBinding(Scroll scroll, Action<eButtonState, Vector2> action)
        {
            m_Input.AddScroll(scroll);

            m_scrollBindings.Add(scroll, action);
        }
        
        #endregion

    }
}
