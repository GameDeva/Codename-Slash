using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Codename___Slash
{
    public struct UIElement
    {
        public Texture2D texture;
        public Rectangle destRect;

        public UIElement(Texture2D texture, Rectangle destRect)
        {
            this.texture = texture;
            this.destRect = destRect;
        }
    }

    public struct Button
    {
        public Texture2D texture;
        public Rectangle destRect;
        public bool onHoover;
        public bool onPressDown;
        public bool onSelect;
        public GameState stateToReturn;
        public Action cake;

        public Button(Texture2D texture, Rectangle destRect, GameState stateToReturn, Action cake = null)
        {
            this.texture = texture;
            this.destRect = destRect;
            onHoover = false;
            onPressDown = false;
            onSelect = false;
            this.stateToReturn = stateToReturn;
            this.cake = cake;
        }
    }
    
    public class MainMenuState : GameState
    {
        private List<UIElement> uIElements;
        private List<Button> buttons;
        private Button buttonOnHoover;
        private MouseState mouseState;

        private UI ui;
        private GameManager gameManager;

        public override void Enter(Game1 game)
        {
            gameManager = GameManager.Instance;
            ui = new UI(game.Content);
            uIElements = new List<UIElement>();
            buttons = new List<Button>();

            AwardsState.LoadAwardsFile();


            base.Enter(game);
        }

        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        protected override void LoadContent()
        {
            // TODO: Change all button mappings toa appropriate states
            // Load and add all the UI elements
            uIElements.Add(new UIElement(stateContent.Load<Texture2D>("UI/Title"), new Rectangle((Game1.SCREENWIDTH / 2) - (700 / 2), 100, 700, 199)));
            // Try getting save data on the gamemanager
            // If there is save data then display the continue button
            if (GameManager.Instance.GetSaveData()) 
                buttons.Add(new Button(stateContent.Load<Texture2D>("UI/Continue"), new Rectangle((Game1.SCREENWIDTH / 2) - (400 / 2), 450, 400, 94), GameplayState, OnContinueButton));
            buttons.Add(new Button(stateContent.Load<Texture2D>("UI/NewGame"), new Rectangle((Game1.SCREENWIDTH / 2) - (454 / 2), 550, 454, 100), GameplayState, OnNewGame));
            buttons.Add(new Button(stateContent.Load<Texture2D>("UI/Protocol"), new Rectangle((Game1.SCREENWIDTH / 2) - (423 / 2), 650, 423, 84), ProtocolState));
            buttons.Add(new Button(stateContent.Load<Texture2D>("UI/Awards"), new Rectangle((Game1.SCREENWIDTH / 2) - (360 / 2), 750, 360, 94), AwardsState));
            buttons.Add(new Button(stateContent.Load<Texture2D>("UI/Quit"), new Rectangle((Game1.SCREENWIDTH / 2) - (205 / 2), 850, 205, 104), null));

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
            if(arg1 == eButtonState.DOWN && buttonOnHoover.onHoover)
            {
                buttonOnHoover.onHoover = true;
                buttonOnHoover.onPressDown = true;
            }

            // Button has been selected
            if(arg1 == eButtonState.UP && buttonOnHoover.onHoover)
            {
                buttonOnHoover.onPressDown = false;
                buttonOnHoover.onSelect = true;

            } else if(arg1 == eButtonState.UP && !buttonOnHoover.onHoover)
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
            foreach(Button button in buttons)
            {
                // Or is mouse over button
                if (button.destRect.Contains(mousePosition))
                {
                    buttonOnHoover.onHoover = false;
                    buttonOnHoover.onPressDown = false;
                    buttonOnHoover.onSelect = false;
                    buttonOnHoover = button;
                    buttonOnHoover.onHoover = true;
                    break;
                }
            }


            commandManager.Update();
            // If button has been selected
            if (buttonOnHoover.onSelect && buttonOnHoover.stateToReturn != null)
            {
                // If the buttons have a method to call on click, call that method
                if(buttonOnHoover.cake != null)
                {
                    buttonOnHoover.cake.Invoke();
                }

                return buttonOnHoover.stateToReturn;
            }
            // Exmaple return statement:::: return GameState.optionState;

            base.Update(game, deltaTime, ref inputHandler);
            return null;
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

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


        private void OnContinueButton()
        {
            GameManager.Instance.OnContinueGame();
        }

        private void OnNewGame()
        {
            GameManager.Instance.OnNewGame();
        }

    }
}
