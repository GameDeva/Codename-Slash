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
    // Main Menu State of the game 
    public class MainMenuState : GameState
    {
        private MenuUI menuUI; // State's menuUI  
        private GameManager gameManager; // Reference to Gamemanager Singleton

        private Game1 game; // Reference to game1 object, used for exiting

        // On entering state, 
        //  Create menuUI with current state's contentManager
        public override void Enter(Game1 game)
        {
            gameManager = GameManager.Instance;
            menuUI = new MenuUI(stateContentManager);
            this.game = game;

            gameManager.LoadAwardsFile();
            
            base.Enter(game);
        }

        // On exiting the state
        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        // Load relevant content for the state
        // All the menuUI content
        protected override void LoadContent()
        {
            // 
            // Load and add all the UI elements
            menuUI.UIElements.Add(new UIElement(stateContentManager.Load<Texture2D>("UI/Title"), new Rectangle((Game1.SCREENWIDTH / 2) - (700 / 2), 100, 700, 199)));
            
            // Try getting save data on the gamemanager
            // If there is save data then display the continue button
            if (GameManager.Instance.GetSaveData())
                menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/Continue"), new Rectangle((Game1.SCREENWIDTH / 2) - (400 / 2), 450, 400, 94), GameplayState, OnContinueButton));
            // Load all buttons 
            menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/NewGame"), new Rectangle((Game1.SCREENWIDTH / 2) - (454 / 2), 550, 454, 100), GameplayState, OnNewGame));
            menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/Protocol"), new Rectangle((Game1.SCREENWIDTH / 2) - (423 / 2), 650, 423, 84), ProtocolState));
            menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/Awards"), new Rectangle((Game1.SCREENWIDTH / 2) - (360 / 2), 750, 360, 94), AwardsState));
            menuUI.Buttons.Add(new Button(stateContentManager.Load<Texture2D>("UI/Quit"), new Rectangle((Game1.SCREENWIDTH / 2) - (205 / 2), 850, 205, 104), null, OnQuitGame));

            // Let menuUI load its content
            menuUI.LoadContent();

            base.LoadContent();
        }

        // Initialise State's keybindings 
        protected override void InitialiseKeyBindings()
        {
            if (commandManager != null)
            {
                // Attach mouse left click to menuUI's on select, for interactive buttons
                commandManager.AddMouseBinding(MouseButton.LEFT, menuUI.OnSelect);
            }

            base.InitialiseKeyBindings();
        }

        // Update State
        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {
            // Update Menu UI
            menuUI.Update();
            // Check which buttons on menuUI is hovered
            menuUI.ButtonsHoverCheck();
            // Update commandManager's input
            commandManager.Update();
            // Return appropriate GameState or null, based on if button selected
            return menuUI.ButtonsSelectCheck();
        }

        // Draw state 
        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            menuUI.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(deltaTime, spriteBatch);
        }

        // Used to let the gameManager set up a continue game
        private void OnContinueButton()
        {
            GameManager.Instance.OnContinueGame();
        }

        // Used to let the gameManager set up a new game
        private void OnNewGame()
        {
            GameManager.Instance.OnNewGame();
        }

        // Used to quit the game
        private void OnQuitGame()
        {
            game.Exit();
        }

    }
}
