using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Codename___Slash.UIRelated;

namespace Codename___Slash
{
    public class NextStageState : GameState
    {
        private SpriteFont hudFont;

        private List<UIElement> uIElements;
        private List<Button> buttons;
        private Button buttonOnHoover;
        private MouseState mouseState;

        private ContentManager content;

        private int finalScore;
        private UI ui;

        public override void Enter(Game1 game)
        {
            content = game.Content;

            ui = new UI(game.Content);
            uIElements = new List<UIElement>();
            buttons = new List<Button>();

            finalScore = GameplayState.UI.Score;
            base.Enter(game);
        }

        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        protected override void LoadContent()
        {
            // Load fonts
            hudFont = content.Load<SpriteFont>("UI/Fonts/Hud");
            // TODO: Change all button mappings toa appropriate states
            // Load and add all the UI elements
            uIElements.Add(new UIElement(stateContentManager.Load<Texture2D>("UI/GameOver"), new Rectangle((Game1.SCREENWIDTH / 2) - (968 / 2), 100, 968, 198)));
            buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/Continue"), new Rectangle((Game1.SCREENWIDTH / 2) - (424 / 2), 850, 424, 67), GameplayState));

            ui.LoadContent();

            base.LoadContent();
        }

        protected override void InitialiseKeyBindings()
        {
            if (commandManager != null)
            {
                commandManager.AddMouseBinding(MouseButton.LEFT, OnSelect);
            }

            base.InitialiseKeyBindings();
        }

        private void OnSelect(eButtonState arg1, Vector2 arg2)
        {
            // Attach current button
            if (arg1 == eButtonState.DOWN && buttonOnHoover.onHoover)
            {
                buttonOnHoover.onHoover = true;
                buttonOnHoover.onPressDown = true;
            }

            // Button has been selected
            if (arg1 == eButtonState.UP && buttonOnHoover.onHoover)
            {
                buttonOnHoover.onPressDown = false;
                buttonOnHoover.onSelect = true;

            }
            else if (arg1 == eButtonState.UP && !buttonOnHoover.onHoover)
            {
                buttonOnHoover.onHoover = false;
            }
        }

        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {

            ui.Update();

            // Get mouse point
            mouseState = Mouse.GetState();
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            buttonOnHoover.onHoover = false;
            // Check if hovering over any UIElement
            foreach (Button button in buttons)
            {
                // Or is mouse over button
                if (button.destRect.Contains(mousePosition))
                {
                    buttonOnHoover = button;
                    buttonOnHoover.onHoover = true;
                    break;
                }
            }


            commandManager.Update();
            // If button has been selected
            if (buttonOnHoover.onSelect)
            {
                buttonOnHoover.onHoover = false;
                buttonOnHoover.onPressDown = false;
                buttonOnHoover.onSelect = false;
                return buttonOnHoover.stateToReturn;
            }
            // Exmaple return statement:::: return GameState.optionState;

            base.Update(game, deltaTime, ref inputHandler);
            return null;
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(hudFont, "Final Score: " + finalScore.ToString(), new Vector2(Game1.SCREENWIDTH / 2 - 100, Game1.SCREENHEIGHT / 2), Color.White);

            // Draw ui elements
            foreach (UIElement element in uIElements)
            {
                spriteBatch.Draw(element.texture, element.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            }

            // Draw the hover rect
            if (buttonOnHoover.onHoover)
                Game1.DrawRect(spriteBatch, buttonOnHoover.destRect);

            // Draw buttons
            foreach (Button element in buttons)
            {
                spriteBatch.Draw(element.texture, element.destRect, null, Color.White, 0.0f, new Vector2(1), SpriteEffects.None, 1.0f);
            }

            ui.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(deltaTime, spriteBatch);
        }
    }
}
