using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codename___Slash.GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash.UIRelated
{
    // MenuUI class, derived from UI 
    //  used for any menu UI system 
    public class MenuUI : UI
    {
        public List<UIElement> UIElements { get; set; } // List of UIElements on the Menu
        public List<Button> Buttons { get; set; } // List of Buttons on the Menu
        private Button buttonOnHover; // Current button mouse is hoovering over

        // Creates new lists and calls UI constructor with given contentManager
        public MenuUI(ContentManager content) : base(content)
        {
            UIElements = new List<UIElement>();
            Buttons = new List<Button>();
        }

        // When a mouse click has been made
        public void OnSelect(eButtonState arg1, Vector2 arg2)
        {
            // Mouse down, Attach current button
            if (arg1 == eButtonState.DOWN && buttonOnHover.onHoover)
            {
                buttonOnHover.onHoover = true;
                buttonOnHover.onPressDown = true;
            }

            // Mouse up on button, Button has been selected
            if (arg1 == eButtonState.UP && buttonOnHover.onHoover)
            {
                buttonOnHover.onPressDown = false;
                buttonOnHover.onSelect = true;

            }
            // Mouse up away from button, cancelled
            else if (arg1 == eButtonState.UP && !buttonOnHover.onHoover)
            {
                buttonOnHover.onHoover = false;
            }
        }

        // Draw UIElements and buttons based on current states
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw ui elements
            foreach (UIElement element in UIElements)
            {
                spriteBatch.Draw(element.texture, element.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            }

            // Draw the hover rect
            if (buttonOnHover.onHoover)
                Game1.DrawRect(spriteBatch, buttonOnHover.destRect);

            // Draw buttons
            foreach (Button element in Buttons)
            {
                spriteBatch.Draw(element.texture, element.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            }
            
            base.Draw(spriteBatch);
        }

        // Loads base UI content
        // TODO: Push button and UIElement loading here, right now difficult as each one has specific parameters
        public override void LoadContent()
        {
            base.LoadContent();
        }

        // Check which button is currently being hovered b the mouse
        public void ButtonsHoverCheck()
        {
            buttonOnHover.onHoover = false;
            // Check if hovering over any UIElement
            foreach (Button button in Buttons)
            {
                // Or is mouse over button
                if (button.destRect.Contains(cursorPos))
                {
                    buttonOnHover.onHoover = false;
                    buttonOnHover.onPressDown = false;
                    buttonOnHover.onSelect = false;
                    buttonOnHover = button;
                    buttonOnHover.onHoover = true;
                    break;
                }
            }
        }
        
        // Update UI 
        public override void Update()
        {
            base.Update();
        }

        // Check if a button has been selected, return appropriate GameState to transition to or return null
        public GameState ButtonsSelectCheck()
        {
            // If button has been selected
            if (buttonOnHover.onSelect)
            {
                // If the buttons have a method to call on click, call that method
                if (buttonOnHover.SelectMethod != null)
                {
                    buttonOnHover.SelectMethod.Invoke();
                }
                // If there is a stat to return, return appropriate Gamstate 
                if (buttonOnHover.stateToReturn != null)
                    return buttonOnHover.stateToReturn; 
            }
            // If none return null
            return null;
        }


    }
}
